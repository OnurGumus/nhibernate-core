using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

		public async Task<IQuery> CreateFilterAsync(object collection, string queryString)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				CheckAndUpdateSessionStatus();
				CollectionFilterImpl filter = new CollectionFilterImpl(queryString, collection, this, await (GetFilterQueryPlanAsync(collection, queryString, null, false)).ParameterMetadata);
				//filter.SetComment(queryString);
				return filter;
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

		public async Task<object> LoadAsync(System.Type entityClass, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (LoadAsync(entityClass.FullName, id, lockMode));
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

		public async Task<T> LoadAsync<T>(object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (LoadAsync(typeof (T), id, lockMode));
			}
		}

		public async Task<T> LoadAsync<T>(object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return (T)await (LoadAsync(typeof (T), id));
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

		public async Task<object> MergeAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (FireMergeAsync(new MergeEvent(entityName, obj, this)));
			}
		}

		public async Task MergeAsync(string entityName, object obj, IDictionary copiedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireMergeAsync(copiedAlready, new MergeEvent(entityName, obj, this)));
			}
		}

		public async Task<object> MergeAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (MergeAsync(null, obj));
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

		public async Task UpdateAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(entityName, obj, id, this)));
			}
		}

		public async Task UpdateAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(null, obj, this)));
			}
		}

		public async Task UpdateAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(null, obj, id, this)));
			}
		}

		public async Task UpdateAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireUpdateAsync(new SaveOrUpdateEvent(entityName, obj, this)));
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

		public async Task SaveAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveAsync(new SaveOrUpdateEvent(entityName, obj, id, this)));
			}
		}

		public async Task<object> SaveAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (FireSaveAsync(new SaveOrUpdateEvent(null, obj, this)));
			}
		}

		public async Task SaveAsync(object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveAsync(new SaveOrUpdateEvent(null, obj, id, this)));
			}
		}

		public async Task<object> SaveAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (FireSaveAsync(new SaveOrUpdateEvent(entityName, obj, this)));
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

		public async Task PersistAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistAsync(new PersistEvent(entityName, obj, this)));
			}
		}

		public async Task PersistAsync(string entityName, object obj, IDictionary createdAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistAsync(createdAlready, new PersistEvent(entityName, obj, this)));
			}
		}

		public async Task PersistAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (PersistAsync(null, obj));
			}
		}

		public async Task PersistOnFlushAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (PersistAsync(null, obj));
			}
		}

		public async Task PersistOnFlushAsync(string entityName, object obj, IDictionary copiedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistOnFlushAsync(copiedAlready, new PersistEvent(entityName, obj, this)));
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

		public async Task PersistOnFlushAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FirePersistOnFlushAsync(new PersistEvent(entityName, obj, this)));
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

		public async Task SaveOrUpdateAsync(string entityName, object obj, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireSaveOrUpdateAsync(new SaveOrUpdateEvent(entityName, obj, id, this)));
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

		public async Task EvictAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireEvictAsync(new EvictEvent(obj, this)));
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

		public async Task ReplicateAsync(string entityName, object obj, ReplicationMode replicationMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireReplicateAsync(new ReplicateEvent(entityName, obj, replicationMode, this)));
			}
		}

		public async Task ReplicateAsync(object obj, ReplicationMode replicationMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireReplicateAsync(new ReplicateEvent(obj, replicationMode, this)));
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

		public async Task LockAsync(string entityName, object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireLockAsync(new LockEvent(entityName, obj, lockMode, this)));
			}
		}

		public async Task LockAsync(object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireLockAsync(new LockEvent(obj, lockMode, this)));
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

		async Task<IList> FindAsync(string query, object[] values, IType[] types)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (ListAsync(query.ToQueryExpression(), new QueryParameters(types, values)));
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

		public async Task DeleteAsync(string entityName, object child, bool isCascadeDeleteEnabled, ISet<object> transientEntities)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireDeleteAsync(new DeleteEvent(entityName, child, isCascadeDeleteEnabled, this), transientEntities));
			}
		}

		public async Task DeleteAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireDeleteAsync(new DeleteEvent(obj, this)));
			}
		}

		public async Task DeleteAsync(string entityName, object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireDeleteAsync(new DeleteEvent(entityName, obj, this)));
			}
		}

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
					return await (plan.PerformIterateAsync<T>(queryParameters, this));
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
					return await (plan.PerformIterateAsync(queryParameters, this));
				}
				finally
				{
					dontFlushFromFind--;
				}
			}
		}

		public override async Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await (GetFilterQueryPlanAsync(collection, filter, queryParameters, true));
				return await (plan.PerformIterateAsync<T>(queryParameters, this));
			}
		}

		public override async Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				FilterQueryPlan plan = await (GetFilterQueryPlanAsync(collection, filter, queryParameters, true));
				return await (plan.PerformIterateAsync(queryParameters, this));
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

		public async Task RefreshAsync(object obj)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireRefreshAsync(new RefreshEvent(obj, this)));
			}
		}

		public async Task RefreshAsync(object obj, IDictionary refreshedAlready)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireRefreshAsync(refreshedAlready, new RefreshEvent(obj, this)));
			}
		}

		public async Task RefreshAsync(object obj, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				await (FireRefreshAsync(new RefreshEvent(obj, lockMode, this)));
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

		public override async Task<object> InstantiateAsync(string clazz, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (InstantiateAsync(Factory.GetEntityPersister(clazz), id));
			}
		}

		public async Task<object> InstantiateAsync(IEntityPersister persister, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				ErrorIfClosed();
				object result = interceptor.Instantiate(persister.EntityName, entityMode, id);
				if (result == null)
				{
					result = await (persister.InstantiateAsync(id, entityMode));
				}

				return result;
			}
		}

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

		public async Task<object> GetAsync(System.Type entityClass, object id)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				return await (GetAsync(entityClass.FullName, id));
			}
		}

		public async Task<object> GetAsync(System.Type clazz, object id, LockMode lockMode)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				LoadEvent loadEvent = new LoadEvent(id, clazz.FullName, lockMode, this);
				await (FireLoadAsync(loadEvent, LoadEventListener.Get));
				return loadEvent.Result;
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

		public async Task SetReadOnlyAsync(object entityOrProxy, bool readOnly)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				await (persistenceContext.SetReadOnlyAsync(entityOrProxy, readOnly));
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

		public async Task<LockMode> GetCurrentLockModeAsync(object obj)
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
					obj = await (proxy.HibernateLazyInitializer.GetImplementationAsync(this));
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
	}
}