#if NET_4_5
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

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class SessionImpl : AbstractSessionImpl, IEventSource, ISerializable, IDeserializationCallback
	{
		/// <summary>
		/// Save a transient object. An id is generated, assigned to the object and returned
		/// </summary>
		/// <param name = "obj"></param>
		/// <returns></returns>
		public async Task<object> SaveAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (FireSaveAsync(new SaveOrUpdateEvent(null, obj, this)));
			}
		}

		public async Task<object> SaveAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (FireSaveAsync(new SaveOrUpdateEvent(entityName, obj, this)));
			}
		}

		public async Task SaveAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveAsync(new SaveOrUpdateEvent(entityName, obj, id, this)));
			}
		}

		/// <summary>
		/// Save a transient object with a manually assigned ID
		/// </summary>
		/// <param name = "obj"></param>
		/// <param name = "id"></param>
		public async Task SaveAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveAsync(new SaveOrUpdateEvent(null, obj, id, this)));
			}
		}

		/// <summary>
		/// Delete a persistent object
		/// </summary>
		/// <param name = "obj"></param>
		public async Task DeleteAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireDeleteAsync(new DeleteEvent(obj, this)));
			}
		}

		/// <summary> Delete a persistent object (by explicit entity name)</summary>
		public async Task DeleteAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireDeleteAsync(new DeleteEvent(entityName, obj, this)));
			}
		}

		public async Task UpdateAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(null, obj, this)));
			}
		}

		public async Task UpdateAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(entityName, obj, this)));
			}
		}

		public async Task UpdateAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(entityName, obj, id, this)));
			}
		}

		public async Task SaveOrUpdateAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveOrUpdateAsync(new SaveOrUpdateEvent(null, obj, this)));
			}
		}

		public async Task SaveOrUpdateAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveOrUpdateAsync(new SaveOrUpdateEvent(entityName, obj, this)));
			}
		}

		public async Task SaveOrUpdateAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveOrUpdateAsync(new SaveOrUpdateEvent(entityName, obj, id, this)));
			}
		}

		public async Task UpdateAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(null, obj, id, this)));
			}
		}

		async Task<IList> FindAsync(string query, object[] values, IType[] types)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (ListAsync(query.ToQueryExpression(), new QueryParameters(types, values)));
			}
		}

		public override async Task ListAsync(IQueryExpression queryExpression, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				var plan = GetHQLQueryPlan(queryExpression, false);
				await (AutoFlushIfRequiredAsync(plan.QuerySpaces));
				bool success = false;
				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					await (plan.PerformListAsync(queryParameters, this, results));
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
					AfterOperation(success);
				}
			}
		}

		public override async Task<IQueryTranslator[]> GetQueriesAsync(IQueryExpression query, bool scalar)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var plan = Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, enabledFilters);
				await (AutoFlushIfRequiredAsync(plan.QuerySpaces));
				return plan.Translators;
			}
		}

		public override async Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				var plan = GetHQLQueryPlan(queryExpression, true);
				await (AutoFlushIfRequiredAsync(plan.QuerySpaces));
				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					return plan.PerformIterate<T>(queryParameters, this);
				}
				finally
				{
					dontFlushFromFind--;
				}
			}
		}

		public override async Task<IEnumerable> EnumerableAsync(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				var plan = GetHQLQueryPlan(queryExpression, true);
				await (AutoFlushIfRequiredAsync(plan.QuerySpaces));
				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					return plan.PerformIterate(queryParameters, this);
				}
				finally
				{
					dontFlushFromFind--;
				}
			}
		}

		// TODO: Scroll(string query, QueryParameters queryParameters)
		public async Task<int> DeleteAsync(string query)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (DeleteAsync(query, NoArgs, NoTypes));
			}
		}

		public async Task<int> DeleteAsync(string query, object value, IType type)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (DeleteAsync(query, new[]{value}, new[]{type}));
			}
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

				IList list = await (FindAsync(query, values, types));
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					await (DeleteAsync(list[i]));
				}

				return count;
			}
		}

		public async Task LockAsync(object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireLockAsync(new LockEvent(obj, lockMode, this)));
			}
		}

		public async Task LockAsync(string entityName, object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireLockAsync(new LockEvent(entityName, obj, lockMode, this)));
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name = "collection"></param>
		/// <param name = "queryString"></param>
		/// <returns></returns>
		public async Task<IQuery> CreateFilterAsync(object collection, string queryString)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				CheckAndUpdateSessionStatus();
				CollectionFilterImpl filter = new CollectionFilterImpl(queryString, collection, this, (await (GetFilterQueryPlanAsync(collection, queryString, null, false))).ParameterMetadata);
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
					await (FlushAsync());
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
					if (await (AutoFlushIfRequiredAsync(plan.QuerySpaces)))
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

		/// <summary> Force an immediate flush</summary>
		public async Task ForceFlushAsync(EntityEntry entityEntry)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				if (log.IsDebugEnabled)
				{
					log.Debug("flushing to force deletion of re-saved object: " + MessageHelper.InfoString(entityEntry.Persister, entityEntry.Id, Factory));
				}

				if (persistenceContext.CascadeLevel > 0)
				{
					throw new ObjectDeletedException("deleted object would be re-saved by cascade (remove deleted object from associations)", entityEntry.Id, entityEntry.EntityName);
				}

				await (FlushAsync());
			}
		}

		/// <summary> Cascade merge an entity instance</summary>
		public async Task MergeAsync(string entityName, object obj, IDictionary copiedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireMergeAsync(copiedAlready, new MergeEvent(entityName, obj, this)));
			}
		}

		/// <summary> Cascade persist an entity instance</summary>
		public async Task PersistAsync(string entityName, object obj, IDictionary createdAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistAsync(createdAlready, new PersistEvent(entityName, obj, this)));
			}
		}

		/// <summary> Cascade persist an entity instance during the flush process</summary>
		public async Task PersistOnFlushAsync(string entityName, object obj, IDictionary copiedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistOnFlushAsync(copiedAlready, new PersistEvent(entityName, obj, this)));
			}
		}

		/// <summary> Cascade refresh an entity instance</summary>
		public async Task RefreshAsync(object obj, IDictionary refreshedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireRefreshAsync(refreshedAlready, new RefreshEvent(obj, this)));
			}
		}

		/// <summary> Cascade delete an entity instance</summary>
		public async Task DeleteAsync(string entityName, object child, bool isCascadeDeleteEnabled, ISet<object> transientEntities)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireDeleteAsync(new DeleteEvent(entityName, child, isCascadeDeleteEnabled, this), transientEntities));
			}
		}

		public async Task<object> MergeAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (FireMergeAsync(new MergeEvent(entityName, obj, this)));
			}
		}

		public async Task<T> MergeAsync<T>(T entity)where T : class
		{
			return (T)await (MergeAsync((object)entity));
		}

		public async Task<T> MergeAsync<T>(string entityName, T entity)where T : class
		{
			return (T)await (MergeAsync(entityName, (object)entity));
		}

		public async Task<object> MergeAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (MergeAsync(null, obj));
			}
		}

		public async Task PersistAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistAsync(new PersistEvent(entityName, obj, this)));
			}
		}

		public async Task PersistAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (PersistAsync(null, obj));
			}
		}

		public async Task PersistOnFlushAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistOnFlushAsync(new PersistEvent(entityName, obj, this)));
			}
		}

		public async Task PersistOnFlushAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (PersistAsync(null, obj));
			}
		}

		public override async Task<string> BestGuessEntityNameAsync(object entity)
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

					entity = await (initializer.GetImplementationAsync());
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

		public override async Task<object> GetEntityUsingInterceptorAsync(EntityKey key)
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
						await (LockAsync(newObject, LockMode.None));
					}

					return newObject;
				}
				else
				{
					return result;
				}
			}
		}

		/// <summary>
		/// detect in-memory changes, determine if the changes are to tables
		/// named in the query and, if so, complete execution the flush
		/// </summary>
		/// <param name = "querySpaces"></param>
		/// <returns></returns>
		private async Task<bool> AutoFlushIfRequiredAsync(ISet<string> querySpaces)
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
					await (autoFlushEventListener[i].OnAutoFlushAsync(autoFlushEvent));
				}

				return autoFlushEvent.FlushRequired;
			}
		}

		public async Task LoadAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				LoadEvent loadEvent = new LoadEvent(id, obj, this);
				await (FireLoadAsync(loadEvent, LoadEventListener.Reload));
			}
		}

		public async Task<T> LoadAsync<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (LoadAsync(typeof (T), id));
			}
		}

		public async Task<T> LoadAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (LoadAsync(typeof (T), id, lockMode));
			}
		}

		/// <summary>
		/// Load the data for the object with the specified id into a newly created object
		/// using "for update", if supported. A new key will be assigned to the object.
		/// This should return an existing proxy where appropriate.
		///
		/// If the object does not exist in the database, an exception is thrown.
		/// </summary>
		/// <param name = "entityClass"></param>
		/// <param name = "id"></param>
		/// <param name = "lockMode"></param>
		/// <returns></returns>
		/// <exception cref = "ObjectNotFoundException">
		/// Thrown when the object with the specified id does not exist in the database.
		/// </exception>
		public async Task<object> LoadAsync(System.Type entityClass, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (LoadAsync(entityClass.FullName, id, lockMode));
			}
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
					await (FireLoadAsync(@event, LoadEventListener.Load));
					if (@event.Result == null)
					{
						Factory.EntityNotFoundDelegate.HandleEntityNotFound(entityName, id);
					}

					success = true;
					return @event.Result;
				}
				finally
				{
					AfterOperation(success);
				}
			}
		}

		public async Task<object> LoadAsync(string entityName, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var @event = new LoadEvent(id, entityName, lockMode, this);
				await (FireLoadAsync(@event, LoadEventListener.Load));
				return @event.Result;
			}
		}

		public async Task<object> LoadAsync(System.Type entityClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (LoadAsync(entityClass.FullName, id));
			}
		}

		public async Task<T> GetAsync<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (GetAsync(typeof (T), id));
			}
		}

		public async Task<T> GetAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (GetAsync(typeof (T), id, lockMode));
			}
		}

		public async Task<object> GetAsync(System.Type entityClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (GetAsync(entityClass.FullName, id));
			}
		}

		/// <summary>
		/// Load the data for the object with the specified id into a newly created object
		/// using "for update", if supported. A new key will be assigned to the object.
		/// This should return an existing proxy where appropriate.
		///
		/// If the object does not exist in the database, null is returned.
		/// </summary>
		/// <param name = "clazz"></param>
		/// <param name = "id"></param>
		/// <param name = "lockMode"></param>
		/// <returns></returns>
		public async Task<object> GetAsync(System.Type clazz, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				LoadEvent loadEvent = new LoadEvent(id, clazz.FullName, lockMode, this);
				await (FireLoadAsync(loadEvent, LoadEventListener.Get));
				return loadEvent.Result;
			}
		}

		public async Task<string> GetEntityNameAsync(object obj)
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
					obj = await (li.GetImplementationAsync());
				}

				EntityEntry entry = persistenceContext.GetEntry(obj);
				if (entry == null)
				{
					throw new TransientObjectException("object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave: " + obj.GetType().FullName);
				}

				return entry.Persister.EntityName;
			}
		}

		public async Task<object> GetAsync(string entityName, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				LoadEvent loadEvent = new LoadEvent(id, entityName, false, this);
				bool success = false;
				try
				{
					await (FireLoadAsync(loadEvent, LoadEventListener.Get));
					success = true;
					return loadEvent.Result;
				}
				finally
				{
					AfterOperation(success);
				}
			}
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
				await (FireLoadAsync(loadEvent, LoadEventListener.ImmediateLoad));
				return loadEvent.Result;
			}
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
				LoadType type = isNullable ? LoadEventListener.InternalLoadNullable : (eager ? LoadEventListener.InternalLoadEager : LoadEventListener.InternalLoadLazy);
				LoadEvent loadEvent = new LoadEvent(id, entityName, true, this);
				await (FireLoadAsync(loadEvent, type));
				if (!isNullable)
				{
					UnresolvableObjectException.ThrowIfNull(loadEvent.Result, id, entityName);
				}

				return loadEvent.Result;
			}
		}

		public async Task RefreshAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireRefreshAsync(new RefreshEvent(obj, this)));
			}
		}

		public async Task RefreshAsync(object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireRefreshAsync(new RefreshEvent(obj, lockMode, this)));
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <remarks>
		/// This can be called from commit() or at the start of a List() method.
		/// <para>
		/// Perform all the necessary SQL statements in a sensible order, to allow
		/// users to respect foreign key constraints:
		/// <list type = "">
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
					await (flushEventListener[i].OnFlushAsync(new FlushEvent(this)));
				}
			}
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
						await (dirtyCheckEventListener[i].OnDirtyCheckAsync(dcEvent));
					}

					return dcEvent.Dirty;
				}
			}
		}

		/// <summary>
		/// called by a collection that wants to initialize itself
		/// </summary>
		/// <param name = "collection"></param>
		/// <param name = "writing"></param>
		public override async Task InitializeCollectionAsync(IPersistentCollection collection, bool writing)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IInitializeCollectionEventListener[] listener = listeners.InitializeCollectionEventListeners;
				for (int i = 0; i < listener.Length; i++)
				{
					await (listener[i].OnInitializeCollectionAsync(new InitializeCollectionEvent(collection, this)));
				}
			}
		}

		private async Task FilterAsync(object collection, string filter, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await (GetFilterQueryPlanAsync(collection, filter, queryParameters, false));
				bool success = false;
				dontFlushFromFind++; //stops flush being called multiple times if this method is recursively called
				try
				{
					await (plan.PerformListAsync(queryParameters, this, results));
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
					AfterOperation(success);
				}
			}
		}

		public override async Task<IList> ListFilterAsync(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<object>();
				await (FilterAsync(collection, filter, queryParameters, results));
				return results;
			}
		}

		public override async Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				List<T> results = new List<T>();
				await (FilterAsync(collection, filter, queryParameters, results));
				return results;
			}
		}

		public override async Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await (GetFilterQueryPlanAsync(collection, filter, queryParameters, true));
				return plan.PerformIterate(queryParameters, this);
			}
		}

		public override async Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await (GetFilterQueryPlanAsync(collection, filter, queryParameters, true));
				return plan.PerformIterate<T>(queryParameters, this);
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
					loaders[i] = new CriteriaLoader(GetOuterJoinLoadable(implementors[i]), Factory, criteria, implementors[i], enabledFilters);
					spaces.UnionWith(loaders[i].QuerySpaces);
				}

				await (AutoFlushIfRequiredAsync(spaces));
				dontFlushFromFind++;
				bool success = false;
				try
				{
					for (int i = size - 1; i >= 0; i--)
					{
						ArrayHelper.AddAll(results, await (loaders[i].ListAsync(this)));
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
					AfterOperation(success);
				}
			}
		}

		public async Task<bool> ContainsAsync(object obj)
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
						obj = await (li.GetImplementationAsync());
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
		/// <param name = "obj"></param>
		public async Task EvictAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireEvictAsync(new EvictEvent(obj, this)));
			}
		}

		public override async Task ListCustomQueryAsync(ICustomQuery customQuery, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				CustomLoader loader = new CustomLoader(customQuery, Factory);
				await (AutoFlushIfRequiredAsync(loader.QuerySpaces));
				bool success = false;
				dontFlushFromFind++;
				try
				{
					ArrayHelper.AddAll(results, await (loader.ListAsync(this, queryParameters)));
					success = true;
				}
				finally
				{
					dontFlushFromFind--;
					AfterOperation(success);
				}
			}
		}

		public async Task ReplicateAsync(object obj, ReplicationMode replicationMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireReplicateAsync(new ReplicateEvent(obj, replicationMode, this)));
			}
		}

		public async Task ReplicateAsync(string entityName, object obj, ReplicationMode replicationMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireReplicateAsync(new ReplicateEvent(entityName, obj, replicationMode, this)));
			}
		}

		/// <inheritdoc/>
		public async Task SetReadOnlyAsync(object entityOrProxy, bool readOnly)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await (persistenceContext.SetReadOnlyAsync(entityOrProxy, readOnly));
			}
		}

		private async Task FireDeleteAsync(DeleteEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IDeleteEventListener[] deleteEventListener = listeners.DeleteEventListeners;
				for (int i = 0; i < deleteEventListener.Length; i++)
				{
					await (deleteEventListener[i].OnDeleteAsync(@event));
				}
			}
		}

		private async Task FireDeleteAsync(DeleteEvent @event, ISet<object> transientEntities)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IDeleteEventListener[] deleteEventListener = listeners.DeleteEventListeners;
				for (int i = 0; i < deleteEventListener.Length; i++)
				{
					await (deleteEventListener[i].OnDeleteAsync(@event, transientEntities));
				}
			}
		}

		private async Task FireEvictAsync(EvictEvent evictEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEvictEventListener[] evictEventListener = listeners.EvictEventListeners;
				for (int i = 0; i < evictEventListener.Length; i++)
				{
					await (evictEventListener[i].OnEvictAsync(evictEvent));
				}
			}
		}

		private async Task FireLoadAsync(LoadEvent @event, LoadType loadType)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ILoadEventListener[] loadEventListener = listeners.LoadEventListeners;
				for (int i = 0; i < loadEventListener.Length; i++)
				{
					await (loadEventListener[i].OnLoadAsync(@event, loadType));
				}
			}
		}

		private async Task FireLockAsync(LockEvent lockEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ILockEventListener[] lockEventListener = listeners.LockEventListeners;
				for (int i = 0; i < lockEventListener.Length; i++)
				{
					await (lockEventListener[i].OnLockAsync(lockEvent));
				}
			}
		}

		private async Task<object> FireMergeAsync(MergeEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IMergeEventListener[] mergeEventListener = listeners.MergeEventListeners;
				for (int i = 0; i < mergeEventListener.Length; i++)
				{
					await (mergeEventListener[i].OnMergeAsync(@event));
				}

				return @event.Result;
			}
		}

		private async Task FireMergeAsync(IDictionary copiedAlready, MergeEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IMergeEventListener[] mergeEventListener = listeners.MergeEventListeners;
				for (int i = 0; i < mergeEventListener.Length; i++)
				{
					await (mergeEventListener[i].OnMergeAsync(@event, copiedAlready));
				}
			}
		}

		private async Task FirePersistAsync(IDictionary copiedAlready, PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] persistEventListener = listeners.PersistEventListeners;
				for (int i = 0; i < persistEventListener.Length; i++)
				{
					await (persistEventListener[i].OnPersistAsync(@event, copiedAlready));
				}
			}
		}

		private async Task FirePersistAsync(PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] createEventListener = listeners.PersistEventListeners;
				for (int i = 0; i < createEventListener.Length; i++)
				{
					await (createEventListener[i].OnPersistAsync(@event));
				}
			}
		}

		private async Task FirePersistOnFlushAsync(IDictionary copiedAlready, PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] persistEventListener = listeners.PersistOnFlushEventListeners;
				for (int i = 0; i < persistEventListener.Length; i++)
				{
					await (persistEventListener[i].OnPersistAsync(@event, copiedAlready));
				}
			}
		}

		private async Task FirePersistOnFlushAsync(PersistEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IPersistEventListener[] createEventListener = listeners.PersistOnFlushEventListeners;
				for (int i = 0; i < createEventListener.Length; i++)
				{
					await (createEventListener[i].OnPersistAsync(@event));
				}
			}
		}

		private async Task FireRefreshAsync(RefreshEvent refreshEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IRefreshEventListener[] refreshEventListener = listeners.RefreshEventListeners;
				for (int i = 0; i < refreshEventListener.Length; i++)
				{
					await (refreshEventListener[i].OnRefreshAsync(refreshEvent));
				}
			}
		}

		private async Task FireRefreshAsync(IDictionary refreshedAlready, RefreshEvent refreshEvent)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IRefreshEventListener[] refreshEventListener = listeners.RefreshEventListeners;
				for (int i = 0; i < refreshEventListener.Length; i++)
				{
					await (refreshEventListener[i].OnRefreshAsync(refreshEvent, refreshedAlready));
				}
			}
		}

		private async Task FireReplicateAsync(ReplicateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IReplicateEventListener[] replicateEventListener = listeners.ReplicateEventListeners;
				for (int i = 0; i < replicateEventListener.Length; i++)
				{
					await (replicateEventListener[i].OnReplicateAsync(@event));
				}
			}
		}

		private async Task<object> FireSaveAsync(SaveOrUpdateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ISaveOrUpdateEventListener[] saveEventListener = listeners.SaveEventListeners;
				for (int i = 0; i < saveEventListener.Length; i++)
				{
					await (saveEventListener[i].OnSaveOrUpdateAsync(@event));
				}

				return @event.ResultId;
			}
		}

		private async Task FireSaveOrUpdateAsync(SaveOrUpdateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ISaveOrUpdateEventListener[] saveOrUpdateEventListener = listeners.SaveOrUpdateEventListeners;
				for (int i = 0; i < saveOrUpdateEventListener.Length; i++)
				{
					await (saveOrUpdateEventListener[i].OnSaveOrUpdateAsync(@event));
				}
			}
		}

		private async Task FireUpdateAsync(SaveOrUpdateEvent @event)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				ISaveOrUpdateEventListener[] updateEventListener = listeners.UpdateEventListeners;
				for (int i = 0; i < updateEventListener.Length; i++)
				{
					await (updateEventListener[i].OnSaveOrUpdateAsync(@event));
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
				await (AutoFlushIfRequiredAsync(plan.CustomQuery.QuerySpaces));
				bool success = false;
				int result;
				try
				{
					result = await (plan.PerformExecuteUpdateAsync(queryParameters, this));
					success = true;
				}
				finally
				{
					AfterOperation(success);
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
				await (AutoFlushIfRequiredAsync(plan.QuerySpaces));
				bool success = false;
				int result;
				try
				{
					result = await (plan.PerformExecuteUpdateAsync(queryParameters, this));
					success = true;
				}
				finally
				{
					AfterOperation(success);
				}

				return result;
			}
		}
	}
}
#endif
