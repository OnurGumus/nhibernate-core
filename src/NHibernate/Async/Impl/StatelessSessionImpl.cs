#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessSessionImpl : AbstractSessionImpl, IStatelessSession
	{
		public override async Task InitializeCollectionAsync(IPersistentCollection collection, bool writing)
		{
			if (temporaryPersistenceContext.IsLoadFinished)
			{
				throw new SessionException("Collections cannot be fetched by a stateless session. You can eager load it through specific query.");
			}

			CollectionEntry ce = temporaryPersistenceContext.GetCollectionEntry(collection);
			if (!collection.WasInitialized)
			{
				await (ce.LoadedPersister.InitializeAsync(ce.LoadedKey, this));
			}
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
				return await (GetAsync(entityName, id));
			}
		}

		public override Task<object> ImmediateLoadAsync(string entityName, object id)
		{
			try
			{
				return Task.FromResult<object>(ImmediateLoad(entityName, id));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
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
					AfterOperation(success);
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
					loaders[i] = new CriteriaLoader(GetOuterJoinLoadable(implementors[i]), Factory, criteria, implementors[i], EnabledFilters);
				}

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
					AfterOperation(success);
				}

				temporaryPersistenceContext.Clear();
			}
		}

		public override Task<IEnumerable> EnumerableAsync(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			try
			{
				return Task.FromResult<IEnumerable>(Enumerable(queryExpression, queryParameters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable>(ex);
			}
		}

		public override Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			try
			{
				return Task.FromResult<IEnumerable<T>>(Enumerable<T>(queryExpression, queryParameters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable<T>>(ex);
			}
		}

		public override Task<IList> ListFilterAsync(object collection, string filter, QueryParameters parameters)
		{
			try
			{
				return Task.FromResult<IList>(ListFilter(collection, filter, parameters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IList>(ex);
			}
		}

		public override Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters parameters)
		{
			try
			{
				return Task.FromResult<IList<T>>(ListFilter<T>(collection, filter, parameters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IList<T>>(ex);
			}
		}

		public override Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters parameters)
		{
			try
			{
				return Task.FromResult<IEnumerable>(EnumerableFilter(collection, filter, parameters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable>(ex);
			}
		}

		public override Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters parameters)
		{
			try
			{
				return Task.FromResult<IEnumerable<T>>(EnumerableFilter<T>(collection, filter, parameters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable<T>>(ex);
			}
		}

		public override async Task<object> InstantiateAsync(string clazz, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return await (Factory.GetEntityPersister(clazz).InstantiateAsync(id, EntityMode.Poco));
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
					ArrayHelper.AddAll(results, await (loader.ListAsync(this, queryParameters)));
					success = true;
				}
				finally
				{
					AfterOperation(success);
				}

				temporaryPersistenceContext.Clear();
			}
		}

		public override Task<IQueryTranslator[]> GetQueriesAsync(IQueryExpression query, bool scalar)
		{
			try
			{
				return Task.FromResult<IQueryTranslator[]>(GetQueries(query, scalar));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IQueryTranslator[]>(ex);
			}
		}

		public override Task<object> GetEntityUsingInterceptorAsync(EntityKey key)
		{
			try
			{
				return Task.FromResult<object>(GetEntityUsingInterceptor(key));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task<string> BestGuessEntityNameAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				if (entity.IsProxy())
				{
					var proxy = entity as INHibernateProxy;
					entity = await (proxy.HibernateLazyInitializer.GetImplementationAsync());
				}

				return GuessEntityName(entity);
			}
		}

		public override Task FlushAsync()
		{
			try
			{
				Flush();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <summary> Insert a entity.</summary>
		/// <param name = "entity">A new transient instance </param>
		/// <returns> the identifier of the instance </returns>
		public async Task<object> InsertAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return await (InsertAsync(null, entity));
			}
		}

		/// <summary> Insert a row. </summary>
		/// <param name = "entityName">The entityName for the entity to be inserted </param>
		/// <param name = "entity">a new transient instance </param>
		/// <returns> the identifier of the instance </returns>
		public async Task<object> InsertAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = await (persister.IdentifierGenerator.GenerateAsync(this, entity));
				object[] state = persister.GetPropertyValues(entity, EntityMode.Poco);
				if (persister.IsVersioned)
				{
					object versionValue = state[persister.VersionProperty];
					bool substitute = await (Versioning.SeedVersionAsync(state, persister.VersionProperty, persister.VersionType, persister.IsUnsavedVersion(versionValue), this));
					if (substitute)
					{
						persister.SetPropertyValues(entity, state, EntityMode.Poco);
					}
				}

				if (id == IdentifierGeneratorFactory.PostInsertIndicator)
				{
					id = await (persister.InsertAsync(state, entity, this));
				}
				else
				{
					await (persister.InsertAsync(id, state, entity, this));
				}

				await (persister.SetIdentifierAsync(entity, id, EntityMode.Poco));
				return id;
			}
		}

		/// <summary> Update a entity.</summary>
		/// <param name = "entity">a detached entity instance </param>
		public async Task UpdateAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await (UpdateAsync(null, entity));
			}
		}

		/// <summary>Update a entity.</summary>
		/// <param name = "entityName">The entityName for the entity to be updated </param>
		/// <param name = "entity">a detached entity instance </param>
		public async Task UpdateAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = await (persister.GetIdentifierAsync(entity, EntityMode.Poco));
				object[] state = persister.GetPropertyValues(entity, EntityMode.Poco);
				object oldVersion;
				if (persister.IsVersioned)
				{
					oldVersion = persister.GetVersion(entity, EntityMode.Poco);
					object newVersion = await (Versioning.IncrementAsync(oldVersion, persister.VersionType, this));
					Versioning.SetVersion(state, newVersion, persister);
					persister.SetPropertyValues(entity, state, EntityMode.Poco);
				}
				else
				{
					oldVersion = null;
				}

				await (persister.UpdateAsync(id, state, null, false, null, oldVersion, entity, null, this));
			}
		}

		/// <summary> Delete a entity. </summary>
		/// <param name = "entity">a detached entity instance </param>
		public async Task DeleteAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await (DeleteAsync(null, entity));
			}
		}

		/// <summary> Delete a entity. </summary>
		/// <param name = "entityName">The entityName for the entity to be deleted </param>
		/// <param name = "entity">a detached entity instance </param>
		public async Task DeleteAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = await (persister.GetIdentifierAsync(entity, EntityMode.Poco));
				object version = persister.GetVersion(entity, EntityMode.Poco);
				await (persister.DeleteAsync(id, version, entity, this));
			}
		}

		/// <summary> Retrieve a entity. </summary>
		/// <returns> a detached entity instance </returns>
		public async Task<object> GetAsync(string entityName, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (GetAsync(entityName, id, LockMode.None));
			}
		}

		/// <summary> Retrieve a entity.
		///
		/// </summary>
		/// <returns> a detached entity instance
		/// </returns>
		public Task<T> GetAsync<T>(object id)
		{
			try
			{
				return Task.FromResult<T>(Get<T>(id));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<T>(ex);
			}
		}

		private async Task<object> GetAsync(System.Type persistentClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (GetAsync(persistentClass.FullName, id));
			}
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
				object result = await (Factory.GetEntityPersister(entityName).LoadAsync(id, null, lockMode, this));
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
		public async Task<T> GetAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (GetAsync(typeof (T).FullName, id, lockMode));
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entity">The entity to be refreshed. </param>
		public async Task RefreshAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (RefreshAsync(await (BestGuessEntityNameAsync(entity)), entity, LockMode.None));
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entityName">The entityName for the entity to be refreshed. </param>
		/// <param name = "entity">The entity to be refreshed.</param>
		public async Task RefreshAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (RefreshAsync(entityName, entity, LockMode.None));
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entity">The entity to be refreshed. </param>
		/// <param name = "lockMode">The LockMode to be applied.</param>
		public async Task RefreshAsync(object entity, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (RefreshAsync(await (BestGuessEntityNameAsync(entity)), entity, lockMode));
			}
		}

		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entityName">The entityName for the entity to be refreshed. </param>
		/// <param name = "entity">The entity to be refreshed. </param>
		/// <param name = "lockMode">The LockMode to be applied. </param>
		public async Task RefreshAsync(string entityName, object entity, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = await (persister.GetIdentifierAsync(entity, EntityMode));
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
					result = await (persister.LoadAsync(id, entity, lockMode, this));
				}
				finally
				{
					FetchProfile = previousFetchProfile;
				}

				UnresolvableObjectException.ThrowIfNull(result, id, persister.EntityName);
			}
		}

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
					result = await (plan.PerformExecuteUpdateAsync(queryParameters, this));
					success = true;
				}
				finally
				{
					AfterOperation(success);
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
					result = await (plan.PerformExecuteUpdateAsync(queryParameters, this));
					success = true;
				}
				finally
				{
					AfterOperation(success);
				}

				temporaryPersistenceContext.Clear();
				return result;
			}
		}
	}
}
#endif
