using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security;
using NHibernate.AdoNet;
using NHibernate.Collection;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Intercept;
using NHibernate.Loader.Criteria;
using NHibernate.Loader.Custom;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Stat;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace NHibernate.Impl
{
	/// <summary>
	/// Concrete implementation of an <see cref="NHibernate.ISession" />, also the central, organizing component
	/// of NHibernate's internal implementation.
	/// </summary>
	/// <remarks>
	/// Exposes two interfaces: <see cref="NHibernate.ISession" /> itself, to the application and 
	/// <see cref="ISessionImplementor" /> to other components of NHibernate. This is where the 
	/// hard stuff is... This class is NOT THREADSAFE.
	/// </remarks>
	[Serializable]
	public sealed class SessionImpl : AbstractSessionImpl, IEventSource, ISerializable, IDeserializationCallback
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(SessionImpl));

		private readonly long timestamp;

		private CacheMode cacheMode = CacheMode.Normal;
		private FlushMode flushMode = FlushMode.Auto;

		private readonly IInterceptor interceptor;

		[NonSerialized]
		private readonly EntityMode entityMode = EntityMode.Poco;

		[NonSerialized]
		private FutureCriteriaBatch futureCriteriaBatch;
		[NonSerialized]
		private FutureQueryBatch futureQueryBatch;

		[NonSerialized]
		private readonly EventListeners listeners;

		[NonSerialized]
		private readonly ActionQueue actionQueue;

		private readonly ConnectionManager connectionManager;

		[NonSerialized]
		private int dontFlushFromFind;

		[NonSerialized]
		private readonly IDictionary<string, IFilter> enabledFilters = new Dictionary<string, IFilter>();

		[NonSerialized]
		private readonly List<string> enabledFilterNames = new List<string>();

		[NonSerialized]
		private readonly StatefulPersistenceContext persistenceContext;

		[NonSerialized]
		private object customContext;

		[NonSerialized]
		private readonly ISession rootSession;

		[NonSerialized]
		private IDictionary<EntityMode, ISession> childSessionsByEntityMode;

		[NonSerialized]
		private readonly bool flushBeforeCompletionEnabled;
		[NonSerialized]
		private readonly bool autoCloseSessionEnabled;
		[NonSerialized]
		private readonly bool ignoreExceptionBeforeTransactionCompletion;
		[NonSerialized]
		private readonly ConnectionReleaseMode connectionReleaseMode;

		#region System.Runtime.Serialization.ISerializable Members

		/// <summary>
		/// Constructor used to recreate the Session during the deserialization.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		/// <remarks>
		/// This is needed because we have to do some checking before the serialization process
		/// begins.  I don't know how to add logic in ISerializable.GetObjectData and have .net
		/// write all of the serializable fields out.
		/// </remarks>
		private SessionImpl(SerializationInfo info, StreamingContext context)
		{
			timestamp = info.GetInt64("timestamp");

			SessionFactoryImpl fact = (SessionFactoryImpl)info.GetValue("factory", typeof(SessionFactoryImpl));
			Factory = fact;
			listeners = fact.EventListeners;
			persistenceContext = (StatefulPersistenceContext)info.GetValue("persistenceContext", typeof(StatefulPersistenceContext));
			customContext = info.GetValue("customContext", typeof(object));
			actionQueue = (ActionQueue)info.GetValue("actionQueue", typeof(ActionQueue));

			flushMode = (FlushMode)info.GetValue("flushMode", typeof(FlushMode));
			cacheMode = (CacheMode)info.GetValue("cacheMode", typeof(CacheMode));

			interceptor = (IInterceptor)info.GetValue("interceptor", typeof(IInterceptor));

			enabledFilters = (IDictionary<string, IFilter>)info.GetValue("enabledFilters", typeof(Dictionary<string, IFilter>));
			enabledFilterNames = (List<string>)info.GetValue("enabledFilterNames", typeof(List<string>));

			connectionManager = (ConnectionManager)info.GetValue("connectionManager", typeof(ConnectionManager));
		}

		/// <summary>
		/// Verify the ISession can be serialized and write the fields to the Serializer.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		/// <remarks>
		/// The fields are marked with [NonSerializable] as just a point of reference.  This method
		/// has complete control and what is serialized and those attributes are ignored.  However,
		/// this method should be in sync with the attributes for easy readability.
		/// </remarks>
#if NET_4_0
		[SecurityCritical]
#else
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
#endif
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			log.Debug("writting session to serializer");

			if (!connectionManager.IsReadyForSerialization)
			{
				throw new InvalidOperationException("Cannot serialize a Session while connected");
			}

			info.AddValue("factory", Factory, typeof(SessionFactoryImpl));
			info.AddValue("persistenceContext", persistenceContext, typeof(StatefulPersistenceContext));
			info.AddValue("customContext", customContext);
			info.AddValue("actionQueue", actionQueue, typeof(ActionQueue));
			info.AddValue("timestamp", timestamp);
			info.AddValue("flushMode", flushMode);
			info.AddValue("cacheMode", cacheMode);

			info.AddValue("interceptor", interceptor, typeof(IInterceptor));

			info.AddValue("enabledFilters", enabledFilters, typeof(IDictionary<string, IFilter>));
			info.AddValue("enabledFilterNames", enabledFilterNames, typeof(List<string>));

			info.AddValue("connectionManager", connectionManager, typeof(ConnectionManager));
		}

		#endregion

		#region System.Runtime.Serialization.IDeserializationCallback Members

		/// <summary>
		/// Once the entire object graph has been deserialized then we can hook the
		/// collections, proxies, and entities back up to the ISession.
		/// </summary>
		/// <param name="sender"></param>
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			log.Debug("OnDeserialization of the session.");

			persistenceContext.SetSession(this);

			// OnDeserialization() must be called manually on all Dictionaries and Hashtables,
			// otherwise they are still empty at this point (the .NET deserialization code calls
			// OnDeserialization() on them AFTER it calls the current method).
			((IDeserializationCallback)enabledFilters).OnDeserialization(sender);

			foreach (string filterName in enabledFilterNames)
			{
				FilterImpl filter = (FilterImpl)enabledFilters[filterName];
				filter.AfterDeserialize(Factory.GetFilterDefinition(filterName));
			}
		}

		#endregion

		/// <summary>
		/// Constructor used for OpenSession(...) processing, as well as construction
		/// of sessions for GetCurrentSession().
		/// </summary>
		/// <param name="connection">The user-supplied connection to use for this session.</param>
		/// <param name="factory">The factory from which this session was obtained</param>
		/// <param name="autoclose">NOT USED</param>
		/// <param name="timestamp">The timestamp for this session</param>
		/// <param name="interceptor">The interceptor to be applied to this session</param>
		/// <param name="entityMode">The entity-mode for this session</param>
		/// <param name="flushBeforeCompletionEnabled">Should we auto flush before completion of transaction</param>
		/// <param name="autoCloseSessionEnabled">Should we auto close after completion of transaction</param>
		/// <param name="ignoreExceptionBeforeTransactionCompletion">Should we ignore exceptions in IInterceptor.BeforeTransactionCompletion</param>
		/// <param name="connectionReleaseMode">The mode by which we should release JDBC connections.</param>
		/// <param name="defaultFlushMode">The default flush mode for this session</param>
		/// <param name="customContext">The user supplied context</param>
		internal SessionImpl(
			DbConnection connection,
			SessionFactoryImpl factory,
			bool autoclose,
			long timestamp,
			IInterceptor interceptor,
			EntityMode entityMode,
			bool flushBeforeCompletionEnabled,
			bool autoCloseSessionEnabled,
			bool ignoreExceptionBeforeTransactionCompletion,
			ConnectionReleaseMode connectionReleaseMode,
			FlushMode defaultFlushMode,
			object customContext = null)
			: base(factory)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (interceptor == null)
					throw new AssertionFailure("The interceptor can not be null.");

				rootSession = null;
				this.timestamp = timestamp;
				this.entityMode = entityMode;
				this.interceptor = interceptor;
				listeners = factory.EventListeners;
				actionQueue = new ActionQueue(this);
				persistenceContext = new StatefulPersistenceContext(this);
				this.flushBeforeCompletionEnabled = flushBeforeCompletionEnabled;
				this.autoCloseSessionEnabled = autoCloseSessionEnabled;
				this.connectionReleaseMode = connectionReleaseMode;
				this.ignoreExceptionBeforeTransactionCompletion = ignoreExceptionBeforeTransactionCompletion;
				connectionManager = new ConnectionManager(this, connection, connectionReleaseMode, interceptor);
				this.flushMode = defaultFlushMode;
				this.customContext = customContext;

				if (factory.Statistics.IsStatisticsEnabled)
				{
					factory.StatisticsImplementor.OpenSession();
				}

				if (log.IsDebugEnabled)
				{
					log.DebugFormat("[session-id={0}] opened session at timestamp: {1}, for session factory: [{2}/{3}]",
						SessionId, timestamp, factory.Name, factory.Uuid);
				}

				CheckAndUpdateSessionStatus();
			}
		}

		/// <summary>
		/// Constructor used in building "child sessions".
		/// </summary>
		/// <param name="parent">The parent Session</param>
		/// <param name="entityMode">The entity mode</param>
		private SessionImpl(SessionImpl parent, EntityMode entityMode)
			: base(parent.Factory, parent.SessionId)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				rootSession = parent;
				timestamp = parent.timestamp;
				connectionManager = parent.connectionManager;
				interceptor = parent.interceptor;
				listeners = parent.listeners;
				actionQueue = new ActionQueue(this);
				this.entityMode = entityMode;
				persistenceContext = new StatefulPersistenceContext(this);
				flushBeforeCompletionEnabled = false;
				autoCloseSessionEnabled = false;
				connectionReleaseMode = parent.ConnectionReleaseMode; // NH different

				if (Factory.Statistics.IsStatisticsEnabled)
					Factory.StatisticsImplementor.OpenSession();

				log.Debug("opened session [" + entityMode + "]");

				CheckAndUpdateSessionStatus();
			}
		}

		public override FutureCriteriaBatch FutureCriteriaBatch
		{
			get
			{
				if (futureCriteriaBatch == null)
					futureCriteriaBatch = new FutureCriteriaBatch(this);
				return futureCriteriaBatch;
			}
			protected internal set
			{
				futureCriteriaBatch = value;
			}
		}

		public override FutureQueryBatch FutureQueryBatch
		{
			get
			{
				if (futureQueryBatch == null)
					futureQueryBatch = new FutureQueryBatch(this);
				return futureQueryBatch;
			}
			protected internal set
			{
				futureQueryBatch = value;
			}
		}

		/// <summary></summary>
		public override IBatcher Batcher
		{
			get
			{
				CheckAndUpdateSessionStatus();
				return connectionManager.Batcher;
			}
		}

		/// <summary></summary>
		public override long Timestamp
		{
			get { return timestamp; }
		}

		public ConnectionReleaseMode ConnectionReleaseMode
		{
			get { return connectionReleaseMode; }
		}

		public bool IsAutoCloseSessionEnabled
		{
			get { return autoCloseSessionEnabled; }
		}

		public bool ShouldAutoClose
		{
			get { return IsAutoCloseSessionEnabled && !IsClosed; }
		}

		public override object CustomContext
		{
			get { return customContext; }
		}

		/// <summary>
		/// Close the session and release all resources
		/// <remarks>
		/// Do not call this method inside a transaction scope, use <c>Dispose</c> instead, since
		/// Close() is not aware of distributed transactions
		/// </remarks>
		/// </summary>
		public IDbConnection Close()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				log.Debug("closing session");
				if (IsClosed)
				{
					throw new SessionException("Session was already closed");
				}

				if (Factory.Statistics.IsStatisticsEnabled)
				{
					Factory.StatisticsImplementor.CloseSession();
				}

				try
				{
					try
					{
						if (childSessionsByEntityMode != null)
						{
							foreach (KeyValuePair<EntityMode, ISession> pair in childSessionsByEntityMode)
							{
								pair.Value.Close();
							}
						}
					}
					catch
					{
						// just ignore
					}

					if (rootSession == null)
						return connectionManager.Close();
					else
						return null;
				}
				finally
				{
					SetClosed();
					Cleanup();
				}
			}
		}

		/// <summary>
		/// Ensure that the locks are downgraded to <see cref="LockMode.None"/>
		/// and that all of the softlocks in the <see cref="Cache"/> have
		/// been released.
		/// </summary>
		public override async Task AfterTransactionCompletion(bool success, ITransaction tx)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				log.Debug("transaction completion");
				if (Factory.Statistics.IsStatisticsEnabled)
				{
					Factory.StatisticsImplementor.EndTransaction(success);
				}

				connectionManager.AfterTransaction();
				persistenceContext.AfterTransactionCompletion();
				await actionQueue.AfterTransactionCompletion(success).ConfigureAwait(false);
				if (rootSession == null)
				{
					try
					{
						interceptor.AfterTransactionCompletion(tx);
					}
					catch (Exception t)
					{
						log.Error("exception in interceptor afterTransactionCompletion()", t);
					}
				}


				//if (autoClear)
				//	Clear();
			}
		}

		private void Cleanup()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				persistenceContext.Clear();
			}
		}

		public LockMode GetCurrentLockMode(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				if (obj == null)
				{
					throw new ArgumentNullException("obj", "null object passed to GetCurrentLockMode");
				}

				if (obj.IsProxy())
				{
					var proxy = obj as INHibernateProxy;
					obj = proxy.HibernateLazyInitializer.GetImplementation(this);
					if (obj == null)
					{
						return LockMode.None;
					}
				}

				EntityEntry e = persistenceContext.GetEntry(obj);
				if (e == null)
				{
					throw new TransientObjectException("Given object not associated with the session");
				}

				if (e.Status != Status.Loaded)
				{
					throw new ObjectDeletedException("The given object was deleted", e.Id, e.EntityName);
				}
				return e.LockMode;
			}
		}

		public override bool IsOpen
		{
			get { return !IsClosed; }
		}

		/// <summary>
		/// Save a transient object. An id is generated, assigned to the object and returned
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public object Save(object obj)
		{
			return SaveAsync(obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<object> SaveAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await FireSave(new SaveOrUpdateEvent(null, obj, this)).ConfigureAwait(false);
			}
		}

		public object Save(string entityName, object obj)
		{
			return SaveAsync(entityName, obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<object> SaveAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await FireSave(new SaveOrUpdateEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public void Save(string entityName, object obj, object id)
		{
			SaveAsync(entityName, obj, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task SaveAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireSave(new SaveOrUpdateEvent(entityName, obj, id, this)).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Save a transient object with a manually assigned ID
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="id"></param>
		public void Save(object obj, object id)
		{
			SaveAsync(obj, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Save a transient object with a manually assigned ID
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="id"></param>
		public async Task SaveAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireSave(new SaveOrUpdateEvent(null, obj, id, this)).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Delete a persistent object
		/// </summary>
		/// <param name="obj"></param>
		public void Delete(object obj)
		{
			DeleteAsync(obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Delete a persistent object
		/// </summary>
		/// <param name="obj"></param>
		public async Task DeleteAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireDelete(new DeleteEvent(obj, this)).ConfigureAwait(false);
			}
		}

		/// <summary> Delete a persistent object (by explicit entity name)</summary>
		public void Delete(string entityName, object obj)
		{
			DeleteAsync(entityName, obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary> Delete a persistent object (by explicit entity name)</summary>
		public async Task DeleteAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireDelete(new DeleteEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public void Update(object obj)
		{
			UpdateAsync(obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task UpdateAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireUpdate(new SaveOrUpdateEvent(null, obj, this)).ConfigureAwait(false);
			}
		}

		public void Update(string entityName, object obj)
		{
			UpdateAsync(entityName, obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task UpdateAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireUpdate(new SaveOrUpdateEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public void Update(string entityName, object obj, object id)
		{
			UpdateAsync(entityName, obj, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task UpdateAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireUpdate(new SaveOrUpdateEvent(entityName, obj, id, this)).ConfigureAwait(false);
			}
		}

		public void SaveOrUpdate(object obj)
		{
			SaveOrUpdateAsync(obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task SaveOrUpdateAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireSaveOrUpdate(new SaveOrUpdateEvent(null, obj, this)).ConfigureAwait(false);
			}
		}

		public void SaveOrUpdate(string entityName, object obj)
		{
			SaveOrUpdateAsync(entityName, obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task SaveOrUpdateAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireSaveOrUpdate(new SaveOrUpdateEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public void SaveOrUpdate(string entityName, object obj, object id)
		{
			SaveOrUpdateAsync(entityName, obj, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task SaveOrUpdateAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireSaveOrUpdate(new SaveOrUpdateEvent(entityName, obj, id, this)).ConfigureAwait(false);
			}
		}

		public void Update(object obj, object id)
		{
			UpdateAsync(obj, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task UpdateAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireUpdate(new SaveOrUpdateEvent(null, obj, id, this)).ConfigureAwait(false);
			}
		}

		private static readonly object[] NoArgs = new object[0];
		private static readonly IType[] NoTypes = new IType[0];

		async Task<IList> Find(string query, object[] values, IType[] types)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await ListAsync(query.ToQueryExpression(), new QueryParameters(types, values)).ConfigureAwait(false);
			}
		}

		public override void CloseSessionFromDistributedTransaction()
		{
			Dispose(true);
		}

		public override async Task ListAsync(IQueryExpression queryExpression, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				var plan = GetHQLQueryPlan(queryExpression, false);
				await AutoFlushIfRequired(plan.QuerySpaces).ConfigureAwait(false);

				bool success = false;
				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					await plan.PerformList(queryParameters, this, results).ConfigureAwait(false);
					success = true;
				}
				catch (HibernateException)
				{
					// Do not call Convert on HibernateExceptions
					throw;
				}
				catch (Exception e)
				{
					throw Convert(e, "Could not execute query");
				}
				finally
				{
					dontFlushFromFind--;
					await AfterOperation(success).ConfigureAwait(false);
				}
			}
		}

		public override async Task<IQueryTranslator[]> GetQueries(IQueryExpression query, bool scalar)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var plan = Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, enabledFilters);
				await AutoFlushIfRequired(plan.QuerySpaces).ConfigureAwait(false);
				return plan.Translators;
			}
		}

		public override IEnumerable<T> Enumerable<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			return EnumerableAsync<T>(queryExpression, queryParameters).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				var plan = GetHQLQueryPlan(queryExpression, true);
				await AutoFlushIfRequired(plan.QuerySpaces).ConfigureAwait(false);

				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					return await plan.PerformIterate<T>(queryParameters, this).ConfigureAwait(false);
				}
				finally
				{
					dontFlushFromFind--;
				}
			}
		}

		public override IEnumerable Enumerable(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			return EnumerableAsync(queryExpression, queryParameters).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task<IEnumerable> EnumerableAsync(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				var plan = GetHQLQueryPlan(queryExpression, true);
				await AutoFlushIfRequired(plan.QuerySpaces).ConfigureAwait(false);

				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					return await plan.PerformIterate(queryParameters, this).ConfigureAwait(false);
				}
				finally
				{
					dontFlushFromFind--;
				}
			}
		}

		// TODO: Scroll(string query, QueryParameters queryParameters)

		public int Delete(string query)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Delete(query, NoArgs, NoTypes);
			}
		}

		public async Task<int> DeleteAsync(string query)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await DeleteAsync(query, NoArgs, NoTypes).ConfigureAwait(false);
			}
		}

		public int Delete(string query, object value, IType type)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Delete(query, new[] { value }, new[] { type });
			}
		}

		public async Task<int> DeleteAsync(string query, object value, IType type)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await DeleteAsync(query, new[] { value }, new[] { type }).ConfigureAwait(false);
			}
		}

		public int Delete(string query, object[] values, IType[] types)
		{
			return DeleteAsync(query, values, types).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<int> DeleteAsync(string query, object[] values, IType[] types)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (string.IsNullOrEmpty(query))
				{
					throw new ArgumentNullException("query", "attempt to perform delete-by-query with null query");
				}

				CheckAndUpdateSessionStatus();

				if (log.IsDebugEnabled)
				{
					log.Debug("delete: " + query);
					if (values.Length != 0)
					{
						log.Debug("parameters: " + StringHelper.ToString(values));
					}
				}

				IList list = await Find(query, values, types).ConfigureAwait(false);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					await DeleteAsync(list[i]).ConfigureAwait(false);
				}
				return count;
			}
		}

		public void Lock(object obj, LockMode lockMode)
		{
			LockAsync(obj, lockMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task LockAsync(object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireLock(new LockEvent(obj, lockMode, this)).ConfigureAwait(false);
			}
		}

		public void Lock(string entityName, object obj, LockMode lockMode)
		{
			LockAsync(entityName, obj, lockMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task LockAsync(string entityName, object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireLock(new LockEvent(entityName, obj, lockMode, this)).ConfigureAwait(false);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="queryString"></param>
		/// <returns></returns>
		public IQuery CreateFilter(object collection, string queryString)
		{
			return CreateFilterAsync(collection, queryString).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="queryString"></param>
		/// <returns></returns>
		public async Task<IQuery> CreateFilterAsync(object collection, string queryString)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				CheckAndUpdateSessionStatus();
				CollectionFilterImpl filter =
					new CollectionFilterImpl(queryString, collection, this,
											 (await GetFilterQueryPlanAsync(collection, queryString, null, false).ConfigureAwait(false)).ParameterMetadata);
				//filter.SetComment(queryString);
				return filter;
			}
		}

		private async Task<FilterQueryPlan> GetFilterQueryPlanAsync(object collection, string filter, QueryParameters parameters, bool shallow)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (collection == null)
				{
					throw new ArgumentNullException("collection", "null collection passed to filter");
				}

				CollectionEntry entry = persistenceContext.GetCollectionEntryOrNull(collection);
				ICollectionPersister roleBeforeFlush = (entry == null) ? null : entry.LoadedPersister;

				FilterQueryPlan plan;
				if (roleBeforeFlush == null)
				{
					// if it was previously unreferenced, we need to flush in order to
					// get its state into the database in order to execute query
					await FlushAsync().ConfigureAwait(false);
					entry = persistenceContext.GetCollectionEntryOrNull(collection);
					ICollectionPersister roleAfterFlush = (entry == null) ? null : entry.LoadedPersister;
					if (roleAfterFlush == null)
					{
						throw new QueryException("The collection was unreferenced");
					}
					plan = Factory.QueryPlanCache.GetFilterQueryPlan(filter, roleAfterFlush.Role, shallow, EnabledFilters);
				}
				else
				{
					// otherwise, we only need to flush if there are in-memory changes
					// to the queried tables
					plan = Factory.QueryPlanCache.GetFilterQueryPlan(filter, roleBeforeFlush.Role, shallow, EnabledFilters);

					var res = await AutoFlushIfRequired(plan.QuerySpaces).ConfigureAwait(false);
					if (res)
					{
						// might need to run a different filter entirely after the flush
						// because the collection role may have changed
						entry = persistenceContext.GetCollectionEntryOrNull(collection);
						ICollectionPersister roleAfterFlush = (entry == null) ? null : entry.LoadedPersister;
						if (roleBeforeFlush != roleAfterFlush)
						{
							if (roleAfterFlush == null)
							{
								throw new QueryException("The collection was dereferenced");
							}
							plan = Factory.QueryPlanCache.GetFilterQueryPlan(filter, roleAfterFlush.Role, shallow, EnabledFilters);
						}
					}
				}

				if (parameters != null)
				{
					parameters.PositionalParameterValues[0] = entry.LoadedKey;
					parameters.PositionalParameterTypes[0] = entry.LoadedPersister.KeyType;
				}

				return plan;
			}
		}

		public override object Instantiate(string clazz, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Instantiate(Factory.GetEntityPersister(clazz), id);
			}
		}

		/// <summary> Get the ActionQueue for this session</summary>
		public ActionQueue ActionQueue
		{
			get
			{
				CheckAndUpdateSessionStatus();
				return actionQueue;
			}
		}

		/// <summary>
		/// Give the interceptor an opportunity to override the default instantiation
		/// </summary>
		/// <param name="persister"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public object Instantiate(IEntityPersister persister, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				ErrorIfClosed();
				object result = interceptor.Instantiate(persister.EntityName, entityMode, id);
				if (result == null)
				{
					result = persister.Instantiate(id, entityMode);
				}
				return result;
			}
		}

		#region IEventSource Members
		/// <summary> Force an immediate flush</summary>
		public async Task ForceFlush(EntityEntry entityEntry)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				if (log.IsDebugEnabled)
				{
					log.Debug("flushing to force deletion of re-saved object: " +
							  MessageHelper.InfoString(entityEntry.Persister, entityEntry.Id, Factory));
				}

				if (persistenceContext.CascadeLevel > 0)
				{
					throw new ObjectDeletedException(
						"deleted object would be re-saved by cascade (remove deleted object from associations)",
						entityEntry.Id,
						entityEntry.EntityName);
				}

				await FlushAsync().ConfigureAwait(false);
			}
		}

		/// <summary> Cascade merge an entity instance</summary>
		public async Task Merge(string entityName, object obj, IDictionary copiedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireMerge(copiedAlready, new MergeEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		/// <summary> Cascade persist an entity instance</summary>
		public async Task Persist(string entityName, object obj, IDictionary createdAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FirePersist(createdAlready, new PersistEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		/// <summary> Cascade persist an entity instance during the flush process</summary>
		public async Task PersistOnFlush(string entityName, object obj, IDictionary copiedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FirePersistOnFlush(copiedAlready, new PersistEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public async Task Refresh(object obj, IDictionary refreshedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireRefresh(refreshedAlready, new RefreshEvent(obj, this)).ConfigureAwait(false);
			}
		}

		/// <summary> Cascade delete an entity instance</summary>
		public async Task Delete(string entityName, object child, bool isCascadeDeleteEnabled, ISet<object> transientEntities)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireDelete(new DeleteEvent(entityName, child, isCascadeDeleteEnabled, this), transientEntities).ConfigureAwait(false);
			}
		}

		#endregion

		public object Merge(string entityName, object obj)
		{
			return MergeAsync(entityName, obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<object> MergeAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await FireMerge(new MergeEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public T Merge<T>(T entity) where T : class
		{
			return (T)Merge((object)entity);
		}

		public async Task<T> MergeAsync<T>(T entity) where T : class
		{
			return (T)await MergeAsync((object)entity).ConfigureAwait(false);
		}

		public T Merge<T>(string entityName, T entity) where T : class
		{
			return (T)Merge(entityName, (object)entity);
		}

		public async Task<T> MergeAsync<T>(string entityName, T entity) where T : class
		{
			return (T)await MergeAsync(entityName, (object)entity).ConfigureAwait(false);
		}

		public object Merge(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Merge(null, obj);
			}
		}

		public async Task<object> MergeAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await MergeAsync(null, obj).ConfigureAwait(false);
			}
		}

		public void Persist(string entityName, object obj)
		{
			PersistAsync(entityName, obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task PersistAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FirePersist(new PersistEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public void Persist(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				Persist(null, obj);
			}
		}

		public async Task PersistAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await PersistAsync(null, obj).ConfigureAwait(false);
			}
		}

		public async Task PersistOnFlush(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FirePersistOnFlush(new PersistEvent(entityName, obj, this)).ConfigureAwait(false);
			}
		}

		public void PersistOnFlush(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				Persist(null, obj);
			}
		}

		/// <summary></summary>
		public override FlushMode FlushMode
		{
			get { return flushMode; }
			set { flushMode = value; }
		}

		public bool FlushBeforeCompletionEnabled
		{
			get { return flushBeforeCompletionEnabled; }
		}

		public override string BestGuessEntityName(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (entity.IsProxy())
				{
					INHibernateProxy proxy = entity as INHibernateProxy;
					ILazyInitializer initializer = proxy.HibernateLazyInitializer;

					// it is possible for this method to be called during flush processing,
					// so make certain that we do not accidently initialize an uninitialized proxy
					if (initializer.IsUninitialized)
					{
						return initializer.PersistentClass.FullName;
					}
					entity = initializer.GetImplementation();
				}
				if (FieldInterceptionHelper.IsInstrumented(entity))
				{
					// NH: support of field-interceptor-proxy
					IFieldInterceptor interceptor = FieldInterceptionHelper.ExtractFieldInterceptor(entity);
					return interceptor.EntityName;
				}
				EntityEntry entry = persistenceContext.GetEntry(entity);
				if (entry == null)
				{
					return GuessEntityName(entity);
				}
				else
				{
					return entry.Persister.EntityName;
				}
			}
		}

		public override string GuessEntityName(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				string entityName = interceptor.GetEntityName(entity);
				if (entityName == null)
				{
					System.Type t = entity.GetType();
					entityName = Factory.TryGetGuessEntityName(t) ?? t.FullName;
				}
				return entityName;
			}
		}

		public override bool IsEventSource
		{
			get
			{
				return true;
			}
		}

		public override object GetEntityUsingInterceptor(EntityKey key)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				// todo : should this get moved to PersistentContext?
				// logically, is PersistentContext the "thing" to which an interceptor gets attached?
				object result = persistenceContext.GetEntity(key);

				if (result == null)
				{
					object newObject = interceptor.GetEntity(key.EntityName, key.Identifier);
					if (newObject != null)
					{
						Lock(newObject, LockMode.None);
					}
					return newObject;
				}
				else
				{
					return result;
				}
			}
		}

		public override IPersistenceContext PersistenceContext
		{
			get
			{
				CheckAndUpdateSessionStatus();
				return persistenceContext;
			}
		}

		/// <summary>
		/// detect in-memory changes, determine if the changes are to tables
		/// named in the query and, if so, complete execution the flush
		/// </summary>
		/// <param name="querySpaces"></param>
		/// <returns></returns>
		private async Task<bool> AutoFlushIfRequired(ISet<string> querySpaces)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				if (!ConnectionManager.IsInActiveTransaction)
				{
					// do not auto-flush while outside a transaction
					return false;
				}
				AutoFlushEvent autoFlushEvent = new AutoFlushEvent(querySpaces, this);
				IAutoFlushEventListener[] autoFlushEventListener = listeners.AutoFlushEventListeners;
				for (int i = 0; i < autoFlushEventListener.Length; i++)
				{
					await autoFlushEventListener[i].OnAutoFlush(autoFlushEvent).ConfigureAwait(false);
				}
				return autoFlushEvent.FlushRequired;
			}
		}

		#region load()/get() operations

		public void Load(object obj, object id)
		{
			LoadAsync(obj, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task LoadAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				LoadEvent loadEvent = new LoadEvent(id, obj, this);
				await FireLoad(loadEvent, LoadEventListener.Reload).ConfigureAwait(false);
			}
		}

		public T Load<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)Load(typeof(T), id);
			}
		}

		public async Task<T> LoadAsync<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await LoadAsync(typeof(T), id).ConfigureAwait(false);
			}
		}

		public T Load<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)Load(typeof(T), id, lockMode);
			}
		}

		public async Task<T> LoadAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await LoadAsync(typeof(T), id, lockMode).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Load the data for the object with the specified id into a newly created object
		/// using "for update", if supported. A new key will be assigned to the object.
		/// This should return an existing proxy where appropriate.
		///
		/// If the object does not exist in the database, an exception is thrown.
		/// </summary>
		/// <param name="entityClass"></param>
		/// <param name="id"></param>
		/// <param name="lockMode"></param>
		/// <returns></returns>
		/// <exception cref="ObjectNotFoundException">
		/// Thrown when the object with the specified id does not exist in the database.
		/// </exception>
		public object Load(System.Type entityClass, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Load(entityClass.FullName, id, lockMode);
			}
		}

		public async Task<object> LoadAsync(System.Type entityClass, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await LoadAsync(entityClass.FullName, id, lockMode).ConfigureAwait(false);
			}
		}

		public object Load(string entityName, object id)
		{
			return LoadAsync(entityName, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<object> LoadAsync(string entityName, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (id == null)
				{
					throw new ArgumentNullException("id", "null is not a valid identifier");
				}

				var @event = new LoadEvent(id, entityName, false, this);
				bool success = false;
				try
				{
					await FireLoad(@event, LoadEventListener.Load).ConfigureAwait(false);
					if (@event.Result == null)
					{
						Factory.EntityNotFoundDelegate.HandleEntityNotFound(entityName, id);
					}
					success = true;
					return @event.Result;
				}
				finally
				{
					await AfterOperation(success).ConfigureAwait(false);
				}
			}
		}

		public object Load(string entityName, object id, LockMode lockMode)
		{
			return LoadAsync(entityName, id, lockMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<object> LoadAsync(string entityName, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var @event = new LoadEvent(id, entityName, lockMode, this);
				await FireLoad(@event, LoadEventListener.Load).ConfigureAwait(false);
				return @event.Result;
			}
		}

		public object Load(System.Type entityClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Load(entityClass.FullName, id);
			}
		}

		public async Task<object> LoadAsync(System.Type entityClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await LoadAsync(entityClass.FullName, id).ConfigureAwait(false);
			}
		}

		public T Get<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)Get(typeof(T), id);
			}
		}

		public async Task<T> GetAsync<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await GetAsync(typeof(T), id).ConfigureAwait(false);
			}
		}

		public T Get<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)Get(typeof(T), id, lockMode);
			}
		}

		public async Task<T> GetAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await GetAsync(typeof(T), id, lockMode).ConfigureAwait(false);
			}
		}

		public object Get(System.Type entityClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Get(entityClass.FullName, id);
			}
		}

		public async Task<object> GetAsync(System.Type entityClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await GetAsync(entityClass.FullName, id).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Load the data for the object with the specified id into a newly created object
		/// using "for update", if supported. A new key will be assigned to the object.
		/// This should return an existing proxy where appropriate.
		///
		/// If the object does not exist in the database, null is returned.
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="id"></param>
		/// <param name="lockMode"></param>
		/// <returns></returns>
		public object Get(System.Type clazz, object id, LockMode lockMode)
		{
			return this.GetAsync(clazz, id, lockMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Load the data for the object with the specified id into a newly created object
		/// using "for update", if supported. A new key will be assigned to the object.
		/// This should return an existing proxy where appropriate.
		///
		/// If the object does not exist in the database, null is returned.
		/// </summary>
		/// <param name="clazz"></param>
		/// <param name="id"></param>
		/// <param name="lockMode"></param>
		/// <returns></returns>
		public async Task<object> GetAsync(System.Type clazz, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				LoadEvent loadEvent = new LoadEvent(id, clazz.FullName, lockMode, this);
				await FireLoad(loadEvent, LoadEventListener.Get).ConfigureAwait(false);
				return loadEvent.Result;
			}
		}
		public string GetEntityName(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				if (obj.IsProxy())
				{
					var proxy = obj as INHibernateProxy;

					if (!persistenceContext.ContainsProxy(proxy))
					{
						throw new TransientObjectException("proxy was not associated with the session");
					}
					ILazyInitializer li = proxy.HibernateLazyInitializer;

					obj = li.GetImplementation();
				}

				EntityEntry entry = persistenceContext.GetEntry(obj);
				if (entry == null)
				{
					throw new TransientObjectException(
						"object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave: "
						+ obj.GetType().FullName);
				}
				return entry.Persister.EntityName;
			}
		}

		public object Get(string entityName, object id)
		{
			return this.GetAsync(entityName, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<object> GetAsync(string entityName, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				LoadEvent loadEvent = new LoadEvent(id, entityName, false, this);
				bool success = false;
				try
				{
					await FireLoad(loadEvent, LoadEventListener.Get).ConfigureAwait(false);
					success = true;
					return loadEvent.Result;
				}
				finally
				{
					await AfterOperation(success).ConfigureAwait(false);
				}
			}
		}
		/// <summary>
		/// Load the data for the object with the specified id into a newly created object.
		/// This is only called when lazily initializing a proxy.
		/// Do NOT return a proxy.
		/// </summary>
		public override object ImmediateLoad(string entityName, object id)
		{
			return ImmediateLoadAsync(entityName, id).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		/// <summary>
		/// Load the data for the object with the specified id into a newly created object.
		/// This is only called when lazily initializing a proxy.
		/// Do NOT return a proxy.
		/// </summary>
		public override async Task<object> ImmediateLoadAsync(string entityName, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (log.IsDebugEnabled)
				{
					IEntityPersister persister = Factory.GetEntityPersister(entityName);
					log.Debug("initializing proxy: " + MessageHelper.InfoString(persister, id, Factory));
				}

				LoadEvent loadEvent = new LoadEvent(id, entityName, true, this);
				await FireLoad(loadEvent, LoadEventListener.ImmediateLoad).ConfigureAwait(false);
				return loadEvent.Result;
			}
		}

		/// <summary>
		/// Return the object with the specified id or throw exception if no row with that id exists. Defer the load,
		/// return a new proxy or return an existing proxy if possible. Do not check if the object was deleted.
		/// </summary>
		public override object InternalLoad(string entityName, object id, bool eager, bool isNullable)
		{
			return InternalLoadAsync(entityName, id, eager, isNullable).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Return the object with the specified id or throw exception if no row with that id exists. Defer the load,
		/// return a new proxy or return an existing proxy if possible. Do not check if the object was deleted.
		/// </summary>
		public override async Task<object> InternalLoadAsync(string entityName, object id, bool eager, bool isNullable)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				// todo : remove
				LoadType type = isNullable
									? LoadEventListener.InternalLoadNullable
									: (eager ? LoadEventListener.InternalLoadEager : LoadEventListener.InternalLoadLazy);
				LoadEvent loadEvent = new LoadEvent(id, entityName, true, this);
				await FireLoad(loadEvent, type).ConfigureAwait(false);
				if (!isNullable)
				{
					UnresolvableObjectException.ThrowIfNull(loadEvent.Result, id, entityName);
				}
				return loadEvent.Result;
			}
		}

		#endregion

		public void Refresh(object obj)
		{
			RefreshAsync(obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task RefreshAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireRefresh(new RefreshEvent(obj, this)).ConfigureAwait(false);
			}
		}

		public void Refresh(object obj, LockMode lockMode)
		{
			RefreshAsync(obj, lockMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task RefreshAsync(object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireRefresh(new RefreshEvent(obj, lockMode, this)).ConfigureAwait(false);
			}
		}

		public ITransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return BeginTransactionAsync(isolationLevel).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (rootSession != null)
				{
					// Todo : should seriously consider not allowing a txn to begin from a child session
					//      can always route the request to the root session...
					log.Warn("Transaction started on non-root session");
				}

				CheckAndUpdateSessionStatus();
				return await connectionManager.BeginTransaction(isolationLevel).ConfigureAwait(false);
			}
		}

		public ITransaction BeginTransaction()
		{
			return BeginTransactionAsync().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public Task<ITransaction> BeginTransactionAsync()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (rootSession != null)
				{
					// Todo : should seriously consider not allowing a txn to begin from a child session
					//      can always route the request to the root session...
					log.Warn("Transaction started on non-root session");
				}

				CheckAndUpdateSessionStatus();
				return connectionManager.BeginTransaction();
			}
		}

		public ITransaction Transaction
		{
			get { return connectionManager.Transaction; }
		}

		/// <summary>
		///
		/// </summary>
		/// <remarks>
		/// This can be called from commit() or at the start of a List() method.
		/// <para>
		/// Perform all the necessary SQL statements in a sensible order, to allow
		/// users to respect foreign key constraints:
		/// <list type="">
		///		<item>Inserts, in the order they were performed</item>
		///		<item>Updates</item>
		///		<item>Deletion of collection elements</item>
		///		<item>Insertion of collection elements</item>
		///		<item>Deletes, in the order they were performed</item>
		/// </list>
		/// </para>
		/// <para>
		/// Go through all the persistent objects and look for collections they might be
		/// holding. If they had a nonpersistable collection, substitute a persistable one
		/// </para>
		/// </remarks>
		public override void Flush()
		{
			FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task FlushAsync()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				if (persistenceContext.CascadeLevel > 0)
				{
					throw new HibernateException("Flush during cascade is dangerous");
				}
				IFlushEventListener[] flushEventListener = listeners.FlushEventListeners;
				for (int i = 0; i < flushEventListener.Length; i++)
				{
					await flushEventListener[i].OnFlush(new FlushEvent(this)).ConfigureAwait(false);
				}
			}
		}

		public override bool TransactionInProgress
		{
			get { return ConnectionManager.IsInActiveTransaction; }
		}

		public bool IsDirty()
		{
			return IsDirtyAsync().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task<bool> IsDirtyAsync()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				log.Debug("checking session dirtiness");
				if (actionQueue.AreInsertionsOrDeletionsQueued)
				{
					log.Debug("session dirty (scheduled updates and insertions)");
					return true;
				}
				else
				{
					DirtyCheckEvent dcEvent = new DirtyCheckEvent(this);
					IDirtyCheckEventListener[] dirtyCheckEventListener = listeners.DirtyCheckEventListeners;
					for (int i = 0; i < dirtyCheckEventListener.Length; i++)
					{
						await dirtyCheckEventListener[i].OnDirtyCheck(dcEvent).ConfigureAwait(false);
					}
					return dcEvent.Dirty;
				}
			}
		}

		/// <summary>
		/// Not for internal use
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public object GetIdentifier(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				// Actually the case for proxies will probably work even with
				// the session closed, but do the check here anyway, so that
				// the behavior is uniform.

				if (obj.IsProxy())
				{
					var proxy = obj as INHibernateProxy;

					ILazyInitializer li = proxy.HibernateLazyInitializer;
					if (li.Session != this)
					{
						throw new TransientObjectException("The proxy was not associated with this session");
					}
					return li.Identifier;
				}

				EntityEntry entry = persistenceContext.GetEntry(obj);
				if (entry == null)
				{
					throw new TransientObjectException("the instance was not associated with this session");
				}
				return entry.Id;
			}
		}

		/// <summary>
		/// Get the id value for an object that is actually associated with the session.
		/// This is a bit stricter than GetEntityIdentifierIfNotUnsaved().
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override object GetContextEntityIdentifier(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (obj.IsProxy())
				{
					INHibernateProxy proxy = obj as INHibernateProxy;

					return proxy.HibernateLazyInitializer.Identifier;
				}
				else
				{
					EntityEntry entry = persistenceContext.GetEntry(obj);
					return (entry != null) ? entry.Id : null;
				}
			}
		}

		internal ICollectionPersister GetCollectionPersister(string role)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Factory.GetCollectionPersister(role);
			}
		}

		/// <summary>
		/// called by a collection that wants to initialize itself
		/// </summary>
		/// <param name="collection"></param>
		/// <param name="writing"></param>
		public override async Task InitializeCollection(IPersistentCollection collection, bool writing)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IInitializeCollectionEventListener[] listener = listeners.InitializeCollectionEventListeners;
				for (int i = 0; i < listener.Length; i++)
				{
					await listener[i].OnInitializeCollection(new InitializeCollectionEvent(collection, this)).ConfigureAwait(false);
				}
			}
		}

		public override IDbConnection Connection
		{
			get { return connectionManager.GetConnection().ConfigureAwait(false).GetAwaiter().GetResult(); }
		}

		public override Task<DbConnection> GetConnection()
		{
			return connectionManager.GetConnection();
		}

		/// <summary>
		/// Gets if the ISession is connected.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the ISession is connected.
		/// </value>
		/// <remarks>
		/// An ISession is considered connected if there is an <see cref="IDbConnection"/> (regardless
		/// of its state) or if it the field <c>connect</c> is true.  Meaning that it will connect
		/// at the next operation that requires a connection.
		/// </remarks>
		public override bool IsConnected
		{
			get { return connectionManager.IsConnected; }
		}

		/// <summary></summary>
		public IDbConnection Disconnect()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				log.Debug("disconnecting session");
				return connectionManager.Disconnect();
			}
		}

		public void Reconnect()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				log.Debug("reconnecting session");
				connectionManager.Reconnect();
			}
		}

		public void Reconnect(IDbConnection conn)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				log.Debug("reconnecting session");
				connectionManager.Reconnect((DbConnection)conn);
			}
		}

		#region System.IDisposable Members

		private string fetchProfile;

		/// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
		~SessionImpl()
		{
			Dispose(false);
		}

		/// <summary>
		/// Perform a soft (distributed transaction aware) close of the session
		/// </summary>
		public void Dispose()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				log.Debug(string.Format("[session-id={0}] running ISession.Dispose()", SessionId));
				if (TransactionContext != null)
				{
					TransactionContext.ShouldCloseSessionOnDistributedTransactionCompleted = true;
					return;
				}
				Dispose(true);
			}
		}

		/// <summary>
		/// Takes care of freeing the managed and unmanaged resources that
		/// this class is responsible for.
		/// </summary>
		/// <param name="isDisposing">Indicates if this Session is being Disposed of or Finalized.</param>
		/// <remarks>
		/// If this Session is being Finalized (<c>isDisposing==false</c>) then make sure not
		/// to call any methods that could potentially bring this Session back to life.
		/// </remarks>
		private void Dispose(bool isDisposing)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (IsAlreadyDisposed)
				{
					// don't dispose of multiple times.
					return;
				}

				log.Debug(string.Format("[session-id={0}] executing real Dispose({1})", SessionId, isDisposing));

				// free managed resources that are being managed by the session if we
				// know this call came through Dispose()
				if (isDisposing && !IsClosed)
				{
					Close();
				}

				// free unmanaged resources here
				if(customContext != null)
				{
					var disposableContext = customContext as IDisposable;
					if(disposableContext != null)
					{
						disposableContext.Dispose();
					}
					customContext = null;
				}

				IsAlreadyDisposed = true;
				// nothing for Finalizer to do - so tell the GC to ignore it
				GC.SuppressFinalize(this);
			}
		}

		#endregion

		private async Task FilterAsync(object collection, string filter, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await GetFilterQueryPlanAsync(collection, filter, queryParameters, false).ConfigureAwait(false);

				bool success = false;
				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					await plan.PerformList(queryParameters, this, results).ConfigureAwait(false);
					success = true;
				}
				catch (HibernateException)
				{
					// Do not call Convert on HibernateExceptions
					throw;
				}
				catch (Exception e)
				{
					throw Convert(e, "could not execute query");
				}
				finally
				{
					dontFlushFromFind--;
					await AfterOperation(success).ConfigureAwait(false);
				}
			}
		}

		public override IList ListFilter(object collection, string filter, QueryParameters queryParameters)
		{
			return ListFilterAsync(collection, filter, queryParameters).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task<IList> ListFilterAsync(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<object>();
				await FilterAsync(collection, filter, queryParameters, results).ConfigureAwait(false);
				return results;
			}
		}

		public override IList<T> ListFilter<T>(object collection, string filter, QueryParameters queryParameters)
		{
			return ListFilterAsync<T>(collection, filter, queryParameters).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				List<T> results = new List<T>();
				await FilterAsync(collection, filter, queryParameters, results).ConfigureAwait(false);
				return results;
			}
		}

		public override IEnumerable EnumerableFilter(object collection, string filter, QueryParameters queryParameters)
		{
			return EnumerableFilterAsync(collection, filter, queryParameters).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await GetFilterQueryPlanAsync(collection, filter, queryParameters, true).ConfigureAwait(false);
				return await plan.PerformIterate(queryParameters, this).ConfigureAwait(false);
			}
		}

		public override IEnumerable<T> EnumerableFilter<T>(object collection, string filter, QueryParameters queryParameters)
		{
			return EnumerableFilterAsync<T>(collection, filter, queryParameters).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await GetFilterQueryPlanAsync(collection, filter, queryParameters, true).ConfigureAwait(false);
				return await plan.PerformIterate<T>(queryParameters, this).ConfigureAwait(false);
			}
		}

		public ICriteria CreateCriteria<T>() where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return CreateCriteria(typeof(T));
			}
		}

		public ICriteria CreateCriteria(System.Type persistentClass)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				return new CriteriaImpl(persistentClass, this);
			}
		}

		public ICriteria CreateCriteria<T>(string alias) where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return CreateCriteria(typeof(T), alias);
			}
		}

		public ICriteria CreateCriteria(System.Type persistentClass, string alias)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				return new CriteriaImpl(persistentClass, alias, this);
			}
		}

		public ICriteria CreateCriteria(string entityName, string alias)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new CriteriaImpl(entityName, alias, this);
			}
		}

		public ICriteria CreateCriteria(string entityName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new CriteriaImpl(entityName, this);
			}
		}

		public IQueryOver<T, T> QueryOver<T>() where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new QueryOver<T, T>(new CriteriaImpl(typeof(T), this));
			}
		}

		public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				string aliasPath = ExpressionProcessor.FindMemberExpression(alias.Body);
				return new QueryOver<T, T>(new CriteriaImpl(typeof(T), aliasPath, this));
			}
		}

		public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new QueryOver<T, T>(new CriteriaImpl(entityName, this));
			}
		}

		public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				string aliasPath = ExpressionProcessor.FindMemberExpression(alias.Body);
				return new QueryOver<T, T>(new CriteriaImpl(entityName, aliasPath, this));
			}
		}

		public override async Task ListAsync(CriteriaImpl criteria, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				string[] implementors = Factory.GetImplementors(criteria.EntityOrClassName);
				int size = implementors.Length;

				CriteriaLoader[] loaders = new CriteriaLoader[size];
				ISet<string> spaces = new HashSet<string>();

				for (int i = 0; i < size; i++)
				{
					loaders[i] = new CriteriaLoader(
						GetOuterJoinLoadable(implementors[i]),
						Factory,
						criteria,
						implementors[i],
						enabledFilters
						);

					spaces.UnionWith(loaders[i].QuerySpaces);
				}

				await AutoFlushIfRequired(spaces).ConfigureAwait(false);

				dontFlushFromFind++;

				bool success = false;
				try
				{
					for (int i = size - 1; i >= 0; i--)
					{
						ArrayHelper.AddAll(results, await loaders[i].List(this).ConfigureAwait(false));
					}
					success = true;
				}
				catch (HibernateException)
				{
					// Do not call Convert on HibernateExceptions
					throw;
				}
				catch (Exception sqle)
				{
					throw Convert(sqle, "Unable to perform find");
				}
				finally
				{
					dontFlushFromFind--;
					await AfterOperation(success).ConfigureAwait(false);
				}
			}
		}

		public bool Contains(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				if (obj.IsProxy())
				{
					var proxy = obj as INHibernateProxy;

					//do not use proxiesByKey, since not all
					//proxies that point to this session's
					//instances are in that collection!
					ILazyInitializer li = proxy.HibernateLazyInitializer;
					if (li.IsUninitialized)
					{
						//if it is an uninitialized proxy, pointing
						//with this session, then when it is accessed,
						//the underlying instance will be "contained"
						return li.Session == this;
					}
					else
					{
						//if it is initialized, see if the underlying
						//instance is contained, since we need to
						//account for the fact that it might have been
						//evicted
						obj = li.GetImplementation();
					}
				}
				// A session is considered to contain an entity only if the entity has
				// an entry in the session's persistence context and the entry reports
				// that the entity has not been removed
				EntityEntry entry = persistenceContext.GetEntry(obj);
				return entry != null && entry.Status != Status.Deleted && entry.Status != Status.Gone;
			}
		}

		/// <summary>
		/// remove any hard references to the entity that are held by the infrastructure
		/// (references held by application or other persistant instances are okay)
		/// </summary>
		/// <param name="obj"></param>
		public void Evict(object obj)
		{
			EvictAsync(obj).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// remove any hard references to the entity that are held by the infrastructure
		/// (references held by application or other persistant instances are okay)
		/// </summary>
		/// <param name="obj"></param>
		public async Task EvictAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireEvict(new EvictEvent(obj, this)).ConfigureAwait(false);
			}
		}

		public override ISQLQuery CreateSQLQuery(string sql)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return base.CreateSQLQuery(sql);
			}
		}

		public override async Task ListCustomQueryAsync(ICustomQuery customQuery, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				CustomLoader loader = new CustomLoader(customQuery, Factory);
				await AutoFlushIfRequired(loader.QuerySpaces).ConfigureAwait(false);

				bool success = false;
				dontFlushFromFind++;
				try
				{
					ArrayHelper.AddAll(results, await loader.List(this, queryParameters).ConfigureAwait(false));
					success = true;
				}
				finally
				{
					dontFlushFromFind--;
					await AfterOperation(success).ConfigureAwait(false);
				}
			}
		}

		/// <summary></summary>
		public void Clear()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				actionQueue.Clear();
				persistenceContext.Clear();
			}
		}

		public void Replicate(object obj, ReplicationMode replicationMode)
		{
			ReplicateAsync(obj, replicationMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task ReplicateAsync(object obj, ReplicationMode replicationMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireReplicate(new ReplicateEvent(obj, replicationMode, this)).ConfigureAwait(false);
			}
		}

		public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
		{
			ReplicateAsync(entityName, obj, replicationMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public async Task ReplicateAsync(string entityName, object obj, ReplicationMode replicationMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await FireReplicate(new ReplicateEvent(entityName, obj, replicationMode, this)).ConfigureAwait(false);
			}
		}

		public ISessionFactory SessionFactory
		{
			get { return Factory; }
		}

		public void CancelQuery()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				Batcher.CancelLastQuery();
			}
		}

		public IFilter GetEnabledFilter(string filterName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IFilter result;
				enabledFilters.TryGetValue(filterName, out result);
				return result;
			}
		}

		public IFilter EnableFilter(string filterName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterImpl filter = new FilterImpl(Factory.GetFilterDefinition(filterName));
				enabledFilters[filterName] = filter;
				if (!enabledFilterNames.Contains(filterName))
				{
					enabledFilterNames.Add(filterName);
				}
				return filter;
			}
		}

		public void DisableFilter(string filterName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				enabledFilters.Remove(filterName);
				enabledFilterNames.Remove(filterName);
			}
		}

		public override Object GetFilterParameterValue(string filterParameterName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				string[] parsed = ParseFilterParameterName(filterParameterName);
				IFilter ifilter;
				enabledFilters.TryGetValue(parsed[0], out ifilter);
				FilterImpl filter = ifilter as FilterImpl;
				if (filter == null)
				{
					throw new ArgumentException("Filter [" + parsed[0] + "] currently not enabled");
				}
				return filter.GetParameter(parsed[1]);
			}
		}

		public override IType GetFilterParameterType(string filterParameterName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				string[] parsed = ParseFilterParameterName(filterParameterName);
				FilterDefinition filterDef = Factory.GetFilterDefinition(parsed[0]);
				if (filterDef == null)
				{
					throw new ArgumentNullException(parsed[0], "Filter [" + parsed[0] + "] not defined");
				}
				IType type = filterDef.GetParameterType(parsed[1]);
				if (type == null)
				{
					// this is an internal error of some sort...
					throw new ArgumentNullException(parsed[1], "Unable to locate type for filter parameter");
				}
				return type;
			}
		}

		public override IDictionary<string, IFilter> EnabledFilters
		{
			get
			{
				CheckAndUpdateSessionStatus();

				foreach (IFilter filter in enabledFilters.Values)
				{
					filter.Validate();
				}

				return enabledFilters;
			}
		}

		private string[] ParseFilterParameterName(string filterParameterName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				int dot = filterParameterName.IndexOf(".");
				if (dot <= 0)
				{
					throw new ArgumentException("Invalid filter-parameter name format", "filterParameterName");
				}
				string filterName = filterParameterName.Substring(0, dot);
				string parameterName = filterParameterName.Substring(dot + 1);
				return new[] { filterName, parameterName };
			}
		}

		public override ConnectionManager ConnectionManager
		{
			get { return connectionManager; }
		}

		public IMultiQuery CreateMultiQuery()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return new MultiQueryImpl(this);
			}
		}

		public IMultiCriteria CreateMultiCriteria()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return new MultiCriteriaImpl(this, Factory);
			}
		}

		/// <summary> Get the statistics for this session.</summary>
		public ISessionStatistics Statistics
		{
			get
			{
				return new SessionStatisticsImpl(this);
			}
		}

		public override void AfterTransactionBegin(ITransaction tx)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				interceptor.AfterTransactionBegin(tx);
			}
		}

		public override async Task BeforeTransactionCompletion(ITransaction tx)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				log.Debug("before transaction completion");
				await actionQueue.BeforeTransactionCompletion().ConfigureAwait(false);
				if (rootSession == null)
				{
					try
					{
						interceptor.BeforeTransactionCompletion(tx);
					}
					catch (Exception e)
					{
						log.Error("exception in interceptor BeforeTransactionCompletion()", e);

						if (ignoreExceptionBeforeTransactionCompletion == false)
							throw;
					}
				}
			}
		}

		public ISession SetBatchSize(int batchSize)
		{
			Batcher.BatchSize = batchSize;
			return this;
		}


		public ISessionImplementor GetSessionImplementation()
		{
			return this;
		}

		public ISession GetSession(EntityMode entityMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				// This is explicitly removed to allow support
				// for child sessions that want to flush during
				// the parent session lifecycle. See NH-1714,
				// and the suggested audit examples.
				//
				//if (this.entityMode.Equals(entityMode))
				//{
				//    return this;
				//}

				if (rootSession != null)
				{
					return rootSession.GetSession(entityMode);
				}

				CheckAndUpdateSessionStatus();

				ISession rtn = null;
				if (childSessionsByEntityMode == null)
				{
					childSessionsByEntityMode = new Dictionary<EntityMode, ISession>();
				}
				else
				{
					childSessionsByEntityMode.TryGetValue(entityMode, out rtn);
				}

				if (rtn == null)
				{
					log.DebugFormat("Creating child session with {0}", entityMode);
					rtn = new SessionImpl(this, entityMode);
					childSessionsByEntityMode.Add(entityMode, rtn);
				}

				return rtn;
			}
		}

		public override IInterceptor Interceptor
		{
			get { return interceptor; }
		}

		/// <summary> Retrieves the configured event listeners from this event source. </summary>
		public override EventListeners Listeners
		{
			get { return listeners; }
		}

		public override int DontFlushFromFind
		{
			get { return dontFlushFromFind; }
		}

		public override CacheMode CacheMode
		{
			get { return cacheMode; }
			set
			{
				CheckAndUpdateSessionStatus();
				if (log.IsDebugEnabled)
				{
					log.Debug("setting cache mode to: " + value);
				}
				cacheMode = value;
			}
		}

		public override EntityMode EntityMode
		{
			get { return entityMode; }
		}

		public EntityMode ActiveEntityMode
		{
			get { return entityMode; }
		}

		public override string FetchProfile
		{
			get { return fetchProfile; }
			set
			{
				CheckAndUpdateSessionStatus();
				fetchProfile = value;
			}
		}

		/// <inheritdoc />
		public void SetReadOnly(object entityOrProxy, bool readOnly)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				persistenceContext.SetReadOnly(entityOrProxy, readOnly);
			}
		}

		/// <inheritdoc />
		public bool DefaultReadOnly
		{
			get { return persistenceContext.DefaultReadOnly; }
			set { persistenceContext.DefaultReadOnly = value; }
		}

		/// <inheritdoc />
		public bool IsReadOnly(object entityOrProxy)
		{
			ErrorIfClosed();
			// CheckTransactionSynchStatus();
			return persistenceContext.IsReadOnly(entityOrProxy);
		}

		private async Task FireDelete(DeleteEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IDeleteEventListener[] deleteEventListener = listeners.DeleteEventListeners;
				for (int i = 0; i < deleteEventListener.Length; i++)
				{
					await deleteEventListener[i].OnDelete(@event).ConfigureAwait(false);
				}
			}
		}

		private async Task FireDelete(DeleteEvent @event, ISet<object> transientEntities)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IDeleteEventListener[] deleteEventListener = listeners.DeleteEventListeners;
				for (int i = 0; i < deleteEventListener.Length; i++)
				{
					await deleteEventListener[i].OnDelete(@event, transientEntities).ConfigureAwait(false);
				}
			}
		}

		private async Task FireEvict(EvictEvent evictEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEvictEventListener[] evictEventListener = listeners.EvictEventListeners;
				for (int i = 0; i < evictEventListener.Length; i++)
				{
					await evictEventListener[i].OnEvict(evictEvent).ConfigureAwait(false);
				}
			}
		}

		private async Task FireLoad(LoadEvent @event, LoadType loadType)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ILoadEventListener[] loadEventListener = listeners.LoadEventListeners;
				for (int i = 0; i < loadEventListener.Length; i++)
				{
					await loadEventListener[i].OnLoad(@event, loadType).ConfigureAwait(false);
				}
			}
		}

		private async Task FireLock(LockEvent lockEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ILockEventListener[] lockEventListener = listeners.LockEventListeners;
				for (int i = 0; i < lockEventListener.Length; i++)
				{
					await lockEventListener[i].OnLock(lockEvent).ConfigureAwait(false);
				}
			}
		}

		private async Task<object> FireMerge(MergeEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IMergeEventListener[] mergeEventListener = listeners.MergeEventListeners;
				for (int i = 0; i < mergeEventListener.Length; i++)
				{
					await mergeEventListener[i].OnMerge(@event).ConfigureAwait(false);
				}
				return @event.Result;
			}
		}

		private async Task FireMerge(IDictionary copiedAlready, MergeEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IMergeEventListener[] mergeEventListener = listeners.MergeEventListeners;
				for (int i = 0; i < mergeEventListener.Length; i++)
				{
					await mergeEventListener[i].OnMerge(@event, copiedAlready).ConfigureAwait(false);
				}
			}
		}

		private async Task FirePersist(IDictionary copiedAlready, PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] persistEventListener = listeners.PersistEventListeners;
				for (int i = 0; i < persistEventListener.Length; i++)
				{
					await persistEventListener[i].OnPersist(@event, copiedAlready).ConfigureAwait(false);
				}
			}
		}

		private async Task FirePersist(PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] createEventListener = listeners.PersistEventListeners;
				for (int i = 0; i < createEventListener.Length; i++)
				{
					await createEventListener[i].OnPersist(@event).ConfigureAwait(false);
				}
			}
		}

		private async Task FirePersistOnFlush(IDictionary copiedAlready, PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] persistEventListener = listeners.PersistOnFlushEventListeners;
				for (int i = 0; i < persistEventListener.Length; i++)
				{
					await persistEventListener[i].OnPersist(@event, copiedAlready).ConfigureAwait(false);
				}
			}
		}

		private async Task FirePersistOnFlush(PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] createEventListener = listeners.PersistOnFlushEventListeners;
				for (int i = 0; i < createEventListener.Length; i++)
				{
					await createEventListener[i].OnPersist(@event).ConfigureAwait(false);
				}
			}
		}

		private async Task FireRefresh(RefreshEvent refreshEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IRefreshEventListener[] refreshEventListener = listeners.RefreshEventListeners;
				for (int i = 0; i < refreshEventListener.Length; i++)
				{
					await refreshEventListener[i].OnRefresh(refreshEvent).ConfigureAwait(false);
				}
			}
		}

		private async Task FireRefresh(IDictionary refreshedAlready, RefreshEvent refreshEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IRefreshEventListener[] refreshEventListener = listeners.RefreshEventListeners;
				for (int i = 0; i < refreshEventListener.Length; i++)
				{
					await refreshEventListener[i].OnRefresh(refreshEvent, refreshedAlready).ConfigureAwait(false);
				}
			}
		}

		private async Task FireReplicate(ReplicateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IReplicateEventListener[] replicateEventListener = listeners.ReplicateEventListeners;
				for (int i = 0; i < replicateEventListener.Length; i++)
				{
					await replicateEventListener[i].OnReplicate(@event).ConfigureAwait(false);
				}
			}
		}

		private async Task<object> FireSave(SaveOrUpdateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ISaveOrUpdateEventListener[] saveEventListener = listeners.SaveEventListeners;
				for (int i = 0; i < saveEventListener.Length; i++)
				{
					await saveEventListener[i].OnSaveOrUpdate(@event).ConfigureAwait(false);
				}
				return @event.ResultId;
			}
		}

		private async Task FireSaveOrUpdate(SaveOrUpdateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ISaveOrUpdateEventListener[] saveOrUpdateEventListener = listeners.SaveOrUpdateEventListeners;
				for (int i = 0; i < saveOrUpdateEventListener.Length; i++)
				{
					await saveOrUpdateEventListener[i].OnSaveOrUpdate(@event).ConfigureAwait(false);
				}
			}
		}

		private async Task FireUpdate(SaveOrUpdateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ISaveOrUpdateEventListener[] updateEventListener = listeners.UpdateEventListeners;
				for (int i = 0; i < updateEventListener.Length; i++)
				{
					await updateEventListener[i].OnSaveOrUpdate(@event).ConfigureAwait(false);
				}
			}
		}

		public override async Task<int> ExecuteNativeUpdateAsync(NativeSQLQuerySpecification nativeQuerySpecification, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				NativeSQLQueryPlan plan = GetNativeSQLQueryPlan(nativeQuerySpecification);

				await AutoFlushIfRequired(plan.CustomQuery.QuerySpaces).ConfigureAwait(false);

				bool success = false;
				int result;
				try
				{
					result = await plan.PerformExecuteUpdate(queryParameters, this).ConfigureAwait(false);
					success = true;
				}
				finally
				{
					await AfterOperation(success).ConfigureAwait(false);
				}
				return result;
			}
		}

		public override async Task<int> ExecuteUpdateAsync(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				var plan = GetHQLQueryPlan(queryExpression, false);
				await AutoFlushIfRequired(plan.QuerySpaces).ConfigureAwait(false);

				bool success = false;
				int result;
				try
				{
					result = await plan.PerformExecuteUpdate(queryParameters, this).ConfigureAwait(false);
					success = true;
				}
				finally
				{
					await AfterOperation(success).ConfigureAwait(false);
				}
				return result;
			}
		}

		public override IEntityPersister GetEntityPersister(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				if (entityName == null)
				{
					return Factory.GetEntityPersister(GuessEntityName(obj));
				}
				else
				{
					// try block is a hack around fact that currently tuplizers are not
					// given the opportunity to resolve a subclass entity name.  this
					// allows the (we assume custom) interceptor the ability to
					// influence this decision if we were not able to based on the
					// given entityName
					try
					{
						return Factory.GetEntityPersister(entityName).GetSubclassEntityPersister(obj, Factory,
																								 entityMode);
					}
					catch (HibernateException)
					{
						try
						{
							return GetEntityPersister(null, obj);
						}
						catch (HibernateException)
						{
							// we ignore this exception and re-throw the
							// original one
						}
						throw;
					}
				}
			}
		}
	}
}
