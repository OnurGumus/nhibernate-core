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
		public override async Task FlushAsync()
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				ManagedFlush(); // NH Different behavior since ADOContext.Context is not implemented
			}
		}

		public override async Task<IList> ListFilterAsync(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

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

		public async Task<object> InsertAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				return await (InsertAsync(null, entity));
			}
		}

		public override async Task<object> GetEntityUsingInterceptorAsync(EntityKey key)
		{
			CheckAndUpdateSessionStatus();
			// while a pending Query we should use existing temporary entities so a join fetch does not create multiple instances
			// of the same parent item (NH-3015, NH-3705).
			object obj;
			if (temporaryPersistenceContext.EntitiesByKey.TryGetValue(key, out obj))
				return obj;
			return null;
		}

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

		public override async Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override async Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			throw new NotImplementedException();
		}

		public override async Task<IEnumerable> EnumerableAsync(IQueryExpression queryExpression, QueryParameters queryParameters)
		{
			throw new NotImplementedException();
		}

		public override async Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override async Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters parameters)
		{
			throw new NotSupportedException();
		}

		public override async Task<IQueryTranslator[]> GetQueriesAsync(IQueryExpression query, bool scalar)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				// take the union of the query spaces (ie the queried tables)
				var plan = Factory.QueryPlanCache.GetHQLQueryPlan(query, scalar, EnabledFilters);
				return plan.Translators;
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

		public async Task DeleteAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await (DeleteAsync(null, entity));
			}
		}

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

		public async Task UpdateAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await (UpdateAsync(null, entity));
			}
		}

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

		public async Task RefreshAsync(object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (RefreshAsync(await (BestGuessEntityNameAsync(entity)), entity, LockMode.None));
			}
		}

		public async Task RefreshAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (RefreshAsync(entityName, entity, LockMode.None));
			}
		}

		public async Task RefreshAsync(object entity, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (RefreshAsync(await (BestGuessEntityNameAsync(entity)), entity, lockMode));
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

		public override async Task<object> ImmediateLoadAsync(string entityName, object id)
		{
			throw new SessionException("proxies cannot be fetched by a stateless session");
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
		public async Task<T> GetAsync<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)Get(typeof (T), id);
			}
		}

		public async Task<T> GetAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (GetAsync(typeof (T).FullName, id, lockMode));
			}
		}

		private async Task<object> GetAsync(System.Type persistentClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (GetAsync(persistentClass.FullName, id));
			}
		}
	}
}