using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Id;
using NHibernate.Loader.Criteria;
using NHibernate.Loader.Custom;
using NHibernate.Loader.Custom.Sql;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[Serializable]
	public class StatelessSessionImpl : AbstractSessionImpl, IStatelessSession
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(StatelessSessionImpl));
		[NonSerialized]
		private readonly ConnectionManager connectionManager;
		[NonSerialized]
		private readonly StatefulPersistenceContext temporaryPersistenceContext;

		internal StatelessSessionImpl(DbConnection connection, SessionFactoryImpl factory)
			: base(factory)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				temporaryPersistenceContext = new StatefulPersistenceContext(this);
				connectionManager = new ConnectionManager(this, connection, ConnectionReleaseMode.AfterTransaction,
														  new EmptyInterceptor());

				if (log.IsDebugEnabled)
				{
					log.DebugFormat("[session-id={0}] opened session for session factory: [{1}/{2}]",
						SessionId, factory.Name, factory.Uuid);
				}

				CheckAndUpdateSessionStatus();
			}
		}

		public override async Task InitializeCollection(IPersistentCollection collection, bool writing)
		{
			if (temporaryPersistenceContext.IsLoadFinished)
			{
				throw new SessionException("Collections cannot be fetched by a stateless session. You can eager load it through specific query.");
			}
			CollectionEntry ce = temporaryPersistenceContext.GetCollectionEntry(collection);
			if (!collection.WasInitialized)
			{
				await ce.LoadedPersister.Initialize(ce.LoadedKey, this).ConfigureAwait(false);
			}
		}

		public override object InternalLoad(string entityName, object id, bool eager, bool isNullable)
		{
			return InternalLoadAsync(entityName, id, eager, isNullable).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task<object> InternalLoadAsync(string entityName, object id, bool eager, bool isNullable)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = Factory.GetEntityPersister(entityName);
				object loaded = temporaryPersistenceContext.GetEntity(GenerateEntityKey(id, persister, EntityMode.Poco));
				if (loaded != null)
				{
					return loaded;
				}
				if (!eager && persister.HasProxy)
				{
					return persister.CreateProxy(id, this);
				}
				//TODO: if not loaded, throw an exception
				return await GetAsync(entityName, id).ConfigureAwait(false);
			}
		}

		public override object ImmediateLoad(string entityName, object id)
		{
			throw new SessionException("proxies cannot be fetched by a stateless session");
		}

		public override Task<object> ImmediateLoadAsync(string entityName, object id)
		{
			return TaskHelper.FromException<object>(new SessionException("proxies cannot be fetched by a stateless session"));
		}

		public override long Timestamp
		{
			get { throw new NotSupportedException(); }
		}

		public override IBatcher Batcher
		{
			get
			{
				CheckAndUpdateSessionStatus();
				return connectionManager.Batcher;
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

				bool success = false;
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
					await AfterOperation(success).ConfigureAwait(false);
				}
				temporaryPersistenceContext.Clear();
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
				for (int i = 0; i < size; i++)
				{
					loaders[i] = new CriteriaLoader(GetOuterJoinLoadable(implementors[i]), Factory,
													criteria, implementors[i], EnabledFilters);
				}

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
					await AfterOperation(success).ConfigureAwait(false);
				}
				temporaryPersistenceContext.Clear();
			}
		}

		public override IEnumerable Enumerable(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			throw new NotImplementedException();
		}

		public override Task<IEnumerable> EnumerableAsync(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<T> Enumerable<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			throw new NotImplementedException();
		}

		public override Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			throw new NotImplementedException();
		}

		public override IList ListFilter(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override Task<IList> ListFilterAsync(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override IList<T> ListFilter<T>(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override IEnumerable EnumerableFilter(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override IEnumerable<T> EnumerableFilter<T>(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override void AfterTransactionBegin(ITransaction tx)
		{
		}

		public override Task BeforeTransactionCompletion(ITransaction tx)
		{
			return TaskHelper.CompletedTask;
		}

		public override Task AfterTransactionCompletion(bool successful, ITransaction tx)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				connectionManager.AfterTransaction();
			}
			return TaskHelper.CompletedTask;
		}

		public override object GetContextEntityIdentifier(object obj)
		{
			CheckAndUpdateSessionStatus();
			return null;
		}

		public override object Instantiate(string clazz, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return Factory.GetEntityPersister(clazz).Instantiate(id, EntityMode.Poco);
			}
		}

		public override async Task ListCustomQueryAsync(ICustomQuery customQuery, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();

				var loader = new CustomLoader(customQuery, Factory);

				var success = false;
				try
				{
					ArrayHelper.AddAll(results, await loader.List(this, queryParameters).ConfigureAwait(false));
					success = true;
				}
				finally
				{
					await AfterOperation(success).ConfigureAwait(false);
				}
				temporaryPersistenceContext.Clear();
			}
		}

		public override object GetFilterParameterValue(string filterParameterName)
		{
			throw new NotSupportedException();
		}

		public override IType GetFilterParameterType(string filterParameterName)
		{
			throw new NotSupportedException();
		}

		public override IDictionary<string, IFilter> EnabledFilters
		{
			get { return new CollectionHelper.EmptyMapClass<string, IFilter>(); }
		}

		public override Task<IQueryTranslator[]> GetQueries(IQueryExpression query, bool scalar)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				// take the union of the query spaces (ie the queried tables)
				var plan = Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, EnabledFilters);
				return Task.FromResult(plan.Translators);
			}
		}

		public override IInterceptor Interceptor
		{
			get { return new EmptyInterceptor(); }
		}

		public override EventListeners Listeners
		{
			get { throw new NotSupportedException(); }
		}

		public override int DontFlushFromFind
		{
			get { return 0; }
		}

		public override ConnectionManager ConnectionManager
		{
			get { return connectionManager; }
		}

		public override bool IsEventSource
		{
			get { return false; }
		}

		public override object GetEntityUsingInterceptor(EntityKey key)
		{
			CheckAndUpdateSessionStatus();
			// while a pending Query we should use existing temporary entities so a join fetch does not create multiple instances
			// of the same parent item (NH-3015, NH-3705).
			object obj;
			if (temporaryPersistenceContext.EntitiesByKey.TryGetValue(key, out obj))
				return obj;

			return null;
		}

		public override IPersistenceContext PersistenceContext
		{
			get { return temporaryPersistenceContext; }
		}

		public override bool IsOpen
		{
			get { return !IsClosed; }
		}

		public override bool IsConnected
		{
			get { return connectionManager.IsConnected; }
		}

		public override FlushMode FlushMode
		{
			get { return FlushMode.Commit; }
			set { throw new NotSupportedException(); }
		}

		public override string BestGuessEntityName(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (entity.IsProxy())
				{
					var proxy = entity as INHibernateProxy;
					entity = proxy.HibernateLazyInitializer.GetImplementation();
				}
				return GuessEntityName(entity);
			}
		}

		public override string GuessEntityName(object entity)
		{
			CheckAndUpdateSessionStatus();
			return entity.GetType().FullName;
		}

		public override IDbConnection Connection
		{
			get { return connectionManager.GetConnection().ConfigureAwait(false).GetAwaiter().GetResult(); }
		}

		public override Task<DbConnection> GetConnection()
		{
			return connectionManager.GetConnection();
		}

		public IStatelessSession SetBatchSize(int batchSize)
		{
			Batcher.BatchSize = batchSize;
			return this;
		}

		public override void Flush()
		{
			FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public override async Task FlushAsync()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await ManagedFlush().ConfigureAwait(false); // NH Different behavior since ADOContext.Context is not implemented
			}
		}

		public async Task ManagedFlush()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await Batcher.ExecuteBatch().ConfigureAwait(false);
			}
		}

		public override bool TransactionInProgress
		{
			get { return Transaction.IsActive; }
		}

		#region IStatelessSession Members

		/// <summary> Get the current Hibernate transaction.</summary>
		public ITransaction Transaction
		{
			get { return connectionManager.Transaction; }
		}

		public override CacheMode CacheMode
		{
			get { return CacheMode.Ignore; }
			set { throw new NotSupportedException(); }
		}

		public override EntityMode EntityMode
		{
			get { return NHibernate.EntityMode.Poco; }
		}

		public override string FetchProfile
		{
			get { return null; }
			set { }
		}

		/// <summary>
		/// Gets the stateless session implementation.
		/// </summary>
		/// <remarks>
		/// This method is provided in order to get the <b>NHibernate</b> implementation of the session from wrapper implementations.
		/// Implementors of the <seealso cref="IStatelessSession"/> interface should return the NHibernate implementation of this method.
		/// </remarks>
		/// <returns>
		/// An NHibernate implementation of the <seealso cref="ISessionImplementor"/> interface
		/// </returns>
		public ISessionImplementor GetSessionImplementation()
		{
			return this;
		}

		/// <summary> Close the stateless session and release the ADO.NET connection.</summary>
		public void Close()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				ManagedClose();
			}
		}

		public void ManagedClose()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (IsClosed)
				{
					throw new SessionException("Session was already closed!");
				}
				ConnectionManager.Close();
				SetClosed();
			}
		}

		/// <summary> Insert a entity.</summary>
		/// <param name="entity">A new transient instance </param>
		/// <returns> the identifier of the instance </returns>
		public object Insert(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return Insert(null, entity);
			}
		}

		/// <summary> Insert a entity.</summary>
		/// <param name="entity">A new transient instance </param>
		/// <returns> the identifier of the instance </returns>
		public async Task<object> InsertAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return await InsertAsync(null, entity).ConfigureAwait(false);
			}
		}

		/// <summary> Insert a row. </summary>
		/// <param name="entityName">The entityName for the entity to be inserted </param>
		/// <param name="entity">a new transient instance </param>
		/// <returns> the identifier of the instance </returns>
		public object Insert(string entityName, object entity)
		{
			return InsertAsync(entityName, entity).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary> Insert a row. </summary>
		/// <param name="entityName">The entityName for the entity to be inserted </param>
		/// <param name="entity">a new transient instance </param>
		/// <returns> the identifier of the instance </returns>
		public async Task<object> InsertAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = await persister.IdentifierGenerator.Generate(this, entity).ConfigureAwait(false);
				object[] state = persister.GetPropertyValues(entity, EntityMode.Poco);
				if (persister.IsVersioned)
				{
					object versionValue = state[persister.VersionProperty];
					bool substitute = Versioning.SeedVersion(state, persister.VersionProperty, persister.VersionType,
															 persister.IsUnsavedVersion(versionValue), this);
					if (substitute)
					{
						persister.SetPropertyValues(entity, state, EntityMode.Poco);
					}
				}
				if (id == IdentifierGeneratorFactory.PostInsertIndicator)
				{
					id = await persister.Insert(state, entity, this).ConfigureAwait(false);
				}
				else
				{
					await persister.Insert(id, state, entity, this).ConfigureAwait(false);
				}
				persister.SetIdentifier(entity, id, EntityMode.Poco);
				return id;
			}
		}

		/// <summary> Update a entity.</summary>
		/// <param name="entity">a detached entity instance </param>
		public void Update(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				Update(null, entity);
			}
		}

		/// <summary>Update a entity.</summary>
		/// <param name="entityName">The entityName for the entity to be updated </param>
		/// <param name="entity">a detached entity instance </param>
		public void Update(string entityName, object entity)
		{
			UpdateAsync(entityName, entity).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>Update a entity.</summary>
		/// <param name="entityName">The entityName for the entity to be updated </param>
		/// <param name="entity">a detached entity instance </param>
		public async Task UpdateAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = persister.GetIdentifier(entity, EntityMode.Poco);
				object[] state = persister.GetPropertyValues(entity, EntityMode.Poco);
				object oldVersion;
				if (persister.IsVersioned)
				{
					oldVersion = persister.GetVersion(entity, EntityMode.Poco);
					object newVersion = Versioning.Increment(oldVersion, persister.VersionType, this);
					Versioning.SetVersion(state, newVersion, persister);
					persister.SetPropertyValues(entity, state, EntityMode.Poco);
				}
				else
				{
					oldVersion = null;
				}
				await persister.Update(id, state, null, false, null, oldVersion, entity, null, this).ConfigureAwait(false);
			}
		}

		/// <summary> Delete a entity. </summary>
		/// <param name="entity">a detached entity instance </param>
		public void Delete(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				Delete(null, entity);
			}
		}

		/// <summary> Delete a entity. </summary>
		/// <param name="entity">a detached entity instance </param>
		public async Task DeleteAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await DeleteAsync(null, entity).ConfigureAwait(false);
			}
		}

		/// <summary> Delete a entity. </summary>
		/// <param name="entityName">The entityName for the entity to be deleted </param>
		/// <param name="entity">a detached entity instance </param>
		public void Delete(string entityName, object entity)
		{
			DeleteAsync(entityName, entity).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary> Delete a entity. </summary>
		/// <param name="entityName">The entityName for the entity to be deleted </param>
		/// <param name="entity">a detached entity instance </param>
		public async Task DeleteAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = persister.GetIdentifier(entity, EntityMode.Poco);
				object version = persister.GetVersion(entity, EntityMode.Poco);
				await persister.Delete(id, version, entity, this).ConfigureAwait(false);
			}
		}

		/// <summary> Retrieve a entity. </summary>
		/// <returns> a detached entity instance </returns>
		public object Get(string entityName, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Get(entityName, id, LockMode.None);
			}
		}

		/// <summary> Retrieve a entity. </summary>
		/// <returns> a detached entity instance </returns>
		public async Task<object> GetAsync(string entityName, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await GetAsync(entityName, id, LockMode.None).ConfigureAwait(false);
			}
		}

		/// <summary> Retrieve a entity.
		///
		/// </summary>
		/// <returns> a detached entity instance
		/// </returns>
		public T Get<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)Get(typeof(T), id);
			}
		}

		/// <summary> Retrieve a entity.
		///
		/// </summary>
		/// <returns> a detached entity instance
		/// </returns>
		public async Task<T> GetAsync<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await GetAsync(typeof(T), id).ConfigureAwait(false);
			}
		}

		private object Get(System.Type persistentClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return Get(persistentClass.FullName, id);
			}
		}

		private async Task<object> GetAsync(System.Type persistentClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await GetAsync(persistentClass.FullName, id).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Retrieve a entity, obtaining the specified lock mode.
		/// </summary>
		/// <returns> a detached entity instance </returns>
		public object Get(string entityName, object id, LockMode lockMode)
		{
			return GetAsync(entityName, id, lockMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Retrieve a entity, obtaining the specified lock mode.
		/// </summary>
		/// <returns> a detached entity instance </returns>
		public async Task<object> GetAsync(string entityName, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				object result = await Factory.GetEntityPersister(entityName).Load(id, null, lockMode, this).ConfigureAwait(false);

				if (temporaryPersistenceContext.IsLoadFinished)
				{
					temporaryPersistenceContext.Clear();
				}
				return result;
			}
		}

		/// <summary>
		/// Retrieve a entity, obtaining the specified lock mode.
		/// </summary>
		/// <returns> a detached entity instance </returns>
		public T Get<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)Get(typeof(T).FullName, id, lockMode);
			}
		}

		/// <summary>
		/// Retrieve a entity, obtaining the specified lock mode.
		/// </summary>
		/// <returns> a detached entity instance </returns>
		public async Task<T> GetAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await GetAsync(typeof(T).FullName, id, lockMode).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entity">The entity to be refreshed. </param>
		public void Refresh(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				Refresh(BestGuessEntityName(entity), entity, LockMode.None);
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entity">The entity to be refreshed. </param>
		public async Task RefreshAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await RefreshAsync(BestGuessEntityName(entity), entity, LockMode.None).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entityName">The entityName for the entity to be refreshed. </param>
		/// <param name="entity">The entity to be refreshed.</param>
		public void Refresh(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				Refresh(entityName, entity, LockMode.None);
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entityName">The entityName for the entity to be refreshed. </param>
		/// <param name="entity">The entity to be refreshed.</param>
		public async Task RefreshAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await RefreshAsync(entityName, entity, LockMode.None).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entity">The entity to be refreshed. </param>
		/// <param name="lockMode">The LockMode to be applied.</param>
		public void Refresh(object entity, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				Refresh(BestGuessEntityName(entity), entity, lockMode);
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entity">The entity to be refreshed. </param>
		/// <param name="lockMode">The LockMode to be applied.</param>
		public async Task RefreshAsync(object entity, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await RefreshAsync(BestGuessEntityName(entity), entity, lockMode).ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entityName">The entityName for the entity to be refreshed. </param>
		/// <param name="entity">The entity to be refreshed. </param>
		/// <param name="lockMode">The LockMode to be applied. </param>
		public void Refresh(string entityName, object entity, LockMode lockMode)
		{
			RefreshAsync(entityName, entity, lockMode).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name="entityName">The entityName for the entity to be refreshed. </param>
		/// <param name="entity">The entity to be refreshed. </param>
		/// <param name="lockMode">The LockMode to be applied. </param>
		public async Task RefreshAsync(string entityName, object entity, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = persister.GetIdentifier(entity, EntityMode);
				if (log.IsDebugEnabled)
				{
					log.Debug("refreshing transient " + MessageHelper.InfoString(persister, id, Factory));
				}
				//from H3.2 TODO : can this ever happen???
				//		EntityKey key = new EntityKey( id, persister, source.getEntityMode() );
				//		if ( source.getPersistenceContext().getEntry( key ) != null ) {
				//			throw new PersistentObjectException(
				//					"attempted to refresh transient instance when persistent " +
				//					"instance was already associated with the Session: " +
				//					MessageHelper.infoString( persister, id, source.getFactory() )
				//			);
				//		}

				if (persister.HasCache)
				{
					CacheKey ck = GenerateCacheKey(id, persister.IdentifierType, persister.RootEntityName);
					persister.Cache.Remove(ck);
				}

				string previousFetchProfile = FetchProfile;
				object result;
				try
				{
					FetchProfile = "refresh";
					result = await persister.Load(id, entity, lockMode, this).ConfigureAwait(false);
				}
				finally
				{
					FetchProfile = previousFetchProfile;
				}
				UnresolvableObjectException.ThrowIfNull(result, id, persister.EntityName);
			}
		}

		/// <summary>
		/// Create a new <see cref="ICriteria"/> instance, for the given entity class,
		/// or a superclass of an entity class.
		/// </summary>
		/// <typeparam name="T">A class, which is persistent, or has persistent subclasses</typeparam>
		/// <returns> The <see cref="ICriteria"/>. </returns>
		/// <remarks>Entities returned by the query are detached.</remarks>
		public ICriteria CreateCriteria<T>() where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return CreateCriteria(typeof(T));
			}
		}

		/// <summary>
		/// Create a new <see cref="ICriteria"/> instance, for the given entity class,
		/// or a superclass of an entity class, with the given alias.
		/// </summary>
		/// <typeparam name="T">A class, which is persistent, or has persistent subclasses</typeparam>
		/// <param name="alias">The alias of the entity</param>
		/// <returns> The <see cref="ICriteria"/>. </returns>
		/// <remarks>Entities returned by the query are detached.</remarks>
		public ICriteria CreateCriteria<T>(string alias) where T : class
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return CreateCriteria(typeof(T), alias);
			}
		}

		public ICriteria CreateCriteria(System.Type entityType)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new CriteriaImpl(entityType, this);
			}
		}

		public ICriteria CreateCriteria(System.Type entityType, string alias)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new CriteriaImpl(entityType, alias, this);
			}
		}

		/// <summary>
		/// Create a new <see cref="ICriteria"/> instance, for the given entity name.
		/// </summary>
		/// <param name="entityName">The entity name. </param>
		/// <returns> The <see cref="ICriteria"/>. </returns>
		/// <remarks>Entities returned by the query are detached.</remarks>
		public ICriteria CreateCriteria(string entityName)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new CriteriaImpl(entityName, this);
			}
		}

		/// <summary>
		/// Create a new <see cref="ICriteria"/> instance, for the given entity name,
		/// with the given alias.
		/// </summary>
		/// <param name="entityName">The entity name. </param>
		/// <param name="alias">The alias of the entity</param>
		/// <returns> The <see cref="ICriteria"/>. </returns>
		/// <remarks>Entities returned by the query are detached.</remarks>
		public ICriteria CreateCriteria(string entityName, string alias)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return new CriteriaImpl(entityName, alias, this);
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

		/// <summary>
		/// Begin a NHibernate transaction
		/// </summary>
		/// <returns>A NHibernate transaction</returns>
		public ITransaction BeginTransaction()
		{
			return BeginTransaction(IsolationLevel.Unspecified);
		}

		/// <summary>
		/// Begin a NHibernate transaction
		/// </summary>
		/// <returns>A NHibernate transaction</returns>
		public Task<ITransaction> BeginTransactionAsync()
		{
			return BeginTransactionAsync(IsolationLevel.Unspecified);
		}

		/// <summary>
		/// Begin a NHibernate transaction with the specified isolation level
		/// </summary>
		/// <param name="isolationLevel">The isolation level</param>
		/// <returns>A NHibernate transaction</returns>
		public ITransaction BeginTransaction(IsolationLevel isolationLevel)
		{
			return BeginTransactionAsync(isolationLevel).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Begin a NHibernate transaction with the specified isolation level
		/// </summary>
		/// <param name="isolationLevel">The isolation level</param>
		/// <returns>A NHibernate transaction</returns>
		public async Task<ITransaction> BeginTransactionAsync(IsolationLevel isolationLevel)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return await connectionManager.BeginTransaction(isolationLevel).ConfigureAwait(false);
			}
		}

		#endregion

		#region IDisposable Members
		private bool _isAlreadyDisposed;

		/// <summary>
		/// Finalizer that ensures the object is correctly disposed of.
		/// </summary>
		~StatelessSessionImpl()
		{
			Dispose(false);
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				log.Debug("running IStatelessSession.Dispose()");
				if (TransactionContext != null)
				{
					TransactionContext.ShouldCloseSessionOnDistributedTransactionCompleted = true;
					return;
				}
				Dispose(true);
			}
		}

		protected void Dispose(bool isDisposing)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (_isAlreadyDisposed)
				{
					// don't dispose of multiple times.
					return;
				}

				// free managed resources that are being managed by the session if we
				// know this call came through Dispose()
				if (isDisposing && !IsClosed)
				{
					Close();
				}

				// free unmanaged resources here

				_isAlreadyDisposed = true;
				// nothing for Finalizer to do - so tell the GC to ignore it
				GC.SuppressFinalize(this);
			}
		}

		#endregion

		public override async Task<int> ExecuteNativeUpdateAsync(NativeSQLQuerySpecification nativeSQLQuerySpecification, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				queryParameters.ValidateParameters();
				NativeSQLQueryPlan plan = GetNativeSQLQueryPlan(nativeSQLQuerySpecification);

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
				temporaryPersistenceContext.Clear();
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
				temporaryPersistenceContext.Clear();
				return result;
			}
		}

		public override FutureCriteriaBatch FutureCriteriaBatch
		{
			get { throw new System.NotSupportedException("future queries are not supported for stateless session"); }
			protected internal set { throw new System.NotSupportedException("future queries are not supported for stateless session"); }
		}

		public override FutureQueryBatch FutureQueryBatch
		{
			get { throw new System.NotSupportedException("future queries are not supported for stateless session"); }
			protected internal set { throw new System.NotSupportedException("future queries are not supported for stateless session"); }
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
					return Factory.GetEntityPersister(entityName).GetSubclassEntityPersister(obj, Factory, EntityMode.Poco);
				}
			}
		}
	}
}
