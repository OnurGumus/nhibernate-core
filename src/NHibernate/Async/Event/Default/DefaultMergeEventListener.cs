#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultMergeEventListener : AbstractSaveEventListener, IMergeEventListener
	{
		public virtual async Task OnMergeAsync(MergeEvent @event)
		{
			EventCache copyCache = new EventCache();
			await (OnMergeAsync(@event, copyCache));
			// TODO: iteratively get transient entities and retry merge until one of the following conditions:
			//   1) transientCopyCache.size() == 0
			//   2) transientCopyCache.size() is not decreasing and copyCache.size() is not increasing
			// TODO: find out if retrying can add entities to copyCache (don't think it can...)
			// For now, just retry once; throw TransientObjectException if there are still any transient entities
			IDictionary transientCopyCache = await (this.GetTransientCopyCacheAsync(@event, copyCache));
			if (transientCopyCache.Count > 0)
			{
				await (RetryMergeTransientEntitiesAsync(@event, transientCopyCache, copyCache));
				// find any entities that are still transient after retry
				transientCopyCache = await (this.GetTransientCopyCacheAsync(@event, copyCache));
				if (transientCopyCache.Count > 0)
				{
					ISet<string> transientEntityNames = new HashSet<string>();
					foreach (object transientEntity in transientCopyCache.Keys)
					{
						string transientEntityName = @event.Session.GuessEntityName(transientEntity);
						transientEntityNames.Add(transientEntityName);
						log.InfoFormat("transient instance could not be processed by merge: {0} [{1}]", transientEntityName, transientEntity.ToString());
					}

					throw new TransientObjectException("one or more objects is an unsaved transient instance - save transient instance(s) before merging: " + transientEntityNames);
				}
			}

			copyCache.Clear();
			copyCache = null;
		}

		public virtual async Task OnMergeAsync(MergeEvent @event, IDictionary copiedAlready)
		{
			EventCache copyCache = (EventCache)copiedAlready;
			IEventSource source = @event.Session;
			object original = @event.Original;
			if (original != null)
			{
				object entity;
				if (original.IsProxy())
				{
					ILazyInitializer li = ((INHibernateProxy)original).HibernateLazyInitializer;
					if (li.IsUninitialized)
					{
						log.Debug("ignoring uninitialized proxy");
						@event.Result = await (source.LoadAsync(li.EntityName, li.Identifier));
						return; //EARLY EXIT!
					}
					else
					{
						entity = await (li.GetImplementationAsync());
					}
				}
				else
				{
					entity = original;
				}

				if (copyCache.Contains(entity) && copyCache.IsOperatedOn(entity))
				{
					log.Debug("already in merge process");
					@event.Result = entity;
				}
				else
				{
					if (copyCache.Contains(entity))
					{
						log.Info("already in copyCache; setting in merge process");
						copyCache.SetOperatedOn(entity, true);
					}

					@event.Entity = entity;
					EntityState entityState = EntityState.Undefined;
					if (ReferenceEquals(null, @event.EntityName))
					{
						@event.EntityName = await (source.BestGuessEntityNameAsync(entity));
					}

					// Check the persistence context for an entry relating to this
					// entity to be merged...
					EntityEntry entry = source.PersistenceContext.GetEntry(entity);
					if (entry == null)
					{
						IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);
						object id = await (persister.GetIdentifierAsync(entity, source.EntityMode));
						if (id != null)
						{
							EntityKey key = source.GenerateEntityKey(id, persister);
							object managedEntity = source.PersistenceContext.GetEntity(key);
							entry = source.PersistenceContext.GetEntry(managedEntity);
							if (entry != null)
							{
								// we have specialized case of a detached entity from the
								// perspective of the merge operation.  Specifically, we
								// have an incoming entity instance which has a corresponding
								// entry in the current persistence context, but registered
								// under a different entity instance
								entityState = EntityState.Detached;
							}
						}
					}

					if (entityState == EntityState.Undefined)
					{
						entityState = await (GetEntityStateAsync(entity, @event.EntityName, entry, source));
					}

					switch (entityState)
					{
						case EntityState.Persistent:
							await (EntityIsPersistentAsync(@event, copyCache));
							break;
						case EntityState.Transient:
							await (EntityIsTransientAsync(@event, copyCache));
							break;
						case EntityState.Detached:
							await (EntityIsDetachedAsync(@event, copyCache));
							break;
						default:
							throw new ObjectDeletedException("deleted instance passed to merge", null, GetLoggableName(@event.EntityName, entity));
					}
				}
			}
		}

		protected virtual async Task EntityIsPersistentAsync(MergeEvent @event, IDictionary copyCache)
		{
			log.Debug("ignoring persistent instance");
			//TODO: check that entry.getIdentifier().equals(requestedId)
			object entity = @event.Entity;
			IEventSource source = @event.Session;
			IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);
			((EventCache)copyCache).Add(entity, entity, true); //before cascade!
			await (CascadeOnMergeAsync(source, persister, entity, copyCache));
			await (CopyValuesAsync(persister, entity, entity, source, copyCache));
			@event.Result = entity;
		}

		protected virtual async Task EntityIsTransientAsync(MergeEvent @event, IDictionary copyCache)
		{
			log.Info("merging transient instance");
			object entity = @event.Entity;
			IEventSource source = @event.Session;
			IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);
			string entityName = persister.EntityName;
			@event.Result = await (this.MergeTransientEntityAsync(entity, entityName, @event.RequestedId, source, copyCache));
		}

		private async Task<object> MergeTransientEntityAsync(object entity, string entityName, object requestedId, IEventSource source, IDictionary copyCache)
		{
			IEntityPersister persister = source.GetEntityPersister(entityName, entity);
			object id = persister.HasIdentifierProperty ? await (persister.GetIdentifierAsync(entity, source.EntityMode)) : null;
			object copy = null;
			if (copyCache.Contains(entity))
			{
				copy = copyCache[entity];
				await (persister.SetIdentifierAsync(copy, id, source.EntityMode));
			}
			else
			{
				copy = await (source.InstantiateAsync(persister, id));
				((EventCache)copyCache).Add(entity, copy, true); // before cascade!
			}

			// cascade first, so that all unsaved objects get their
			// copy created before we actually copy
			//cascadeOnMerge(event, persister, entity, copyCache, Cascades.CASCADE_BEFORE_MERGE);
			await (base.CascadeBeforeSaveAsync(source, persister, entity, copyCache));
			await (CopyValuesAsync(persister, entity, copy, source, copyCache, ForeignKeyDirection.ForeignKeyFromParent));
			try
			{
				// try saving; check for non-nullable properties that are null or transient entities before saving
				await (this.SaveTransientEntityAsync(copy, entityName, requestedId, source, copyCache));
			}
			catch (PropertyValueException ex)
			{
				string propertyName = ex.PropertyName;
				object propertyFromCopy = persister.GetPropertyValue(copy, propertyName, source.EntityMode);
				object propertyFromEntity = persister.GetPropertyValue(entity, propertyName, source.EntityMode);
				IType propertyType = persister.GetPropertyType(propertyName);
				EntityEntry copyEntry = source.PersistenceContext.GetEntry(copy);
				if (propertyFromCopy == null || !propertyType.IsEntityType)
				{
					log.InfoFormat("property '{0}.{1}' is null or not an entity; {1} =[{2}]", copyEntry.EntityName, propertyName, propertyFromCopy);
					throw;
				}

				if (!copyCache.Contains(propertyFromEntity))
				{
					log.InfoFormat("property '{0}.{1}' from original entity is not in copyCache; {1} =[{2}]", copyEntry.EntityName, propertyName, propertyFromEntity);
					throw;
				}

				if (((EventCache)copyCache).IsOperatedOn(propertyFromEntity))
				{
					log.InfoFormat("property '{0}.{1}' from original entity is in copyCache and is in the process of being merged; {1} =[{2}]", copyEntry.EntityName, propertyName, propertyFromEntity);
				}
				else
				{
					log.InfoFormat("property '{0}.{1}' from original entity is in copyCache and is not in the process of being merged; {1} =[{2}]", copyEntry.EntityName, propertyName, propertyFromEntity);
				}
			// continue...; we'll find out if it ends up not getting saved later
			}

			// cascade first, so that all unsaved objects get their
			// copy created before we actually copy
			await (base.CascadeAfterSaveAsync(source, persister, entity, copyCache));
			await (CopyValuesAsync(persister, entity, copy, source, copyCache, ForeignKeyDirection.ForeignKeyToParent));
			return copy;
		}

		private async Task SaveTransientEntityAsync(object entity, string entityName, object requestedId, IEventSource source, IDictionary copyCache)
		{
			// this bit is only *really* absolutely necessary for handling
			// requestedId, but is also good if we merge multiple object
			// graphs, since it helps ensure uniqueness
			if (requestedId == null)
			{
				await (SaveWithGeneratedIdAsync(entity, entityName, copyCache, source, false));
			}
			else
			{
				await (SaveWithRequestedIdAsync(entity, requestedId, entityName, copyCache, source));
			}
		}

		protected virtual async Task EntityIsDetachedAsync(MergeEvent @event, IDictionary copyCache)
		{
			log.Debug("merging detached instance");
			object entity = @event.Entity;
			IEventSource source = @event.Session;
			IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);
			string entityName = persister.EntityName;
			object id = @event.RequestedId;
			if (id == null)
			{
				id = await (persister.GetIdentifierAsync(entity, source.EntityMode));
			}
			else
			{
				// check that entity id = requestedId
				object entityId = await (persister.GetIdentifierAsync(entity, source.EntityMode));
				if (!await (persister.IdentifierType.IsEqualAsync(id, entityId, source.EntityMode, source.Factory)))
				{
					throw new HibernateException("merge requested with id not matching id of passed entity");
				}
			}

			string previousFetchProfile = source.FetchProfile;
			source.FetchProfile = "merge";
			//we must clone embedded composite identifiers, or
			//we will get back the same instance that we pass in
			object clonedIdentifier = await (persister.IdentifierType.DeepCopyAsync(id, source.EntityMode, source.Factory));
			object result = await (source.GetAsync(persister.EntityName, clonedIdentifier));
			source.FetchProfile = previousFetchProfile;
			if (result == null)
			{
				await (EntityIsTransientAsync(@event, copyCache));
			}
			else
			{
				// NH different behavior : NH-1517
				if (InvokeUpdateLifecycle(entity, persister, source))
				{
					return;
				}

				((EventCache)copyCache).Add(entity, result, true); //before cascade!
				object target = await (source.PersistenceContext.UnproxyAsync(result));
				if (target == entity)
				{
					throw new AssertionFailure("entity was not detached");
				}
				else if (!(await (source.GetEntityNameAsync(target))).Equals(entityName))
				{
					throw new WrongClassException("class of the given object did not match class of persistent copy", @event.RequestedId, persister.EntityName);
				}
				else if (await (IsVersionChangedAsync(entity, source, persister, target)))
				{
					if (source.Factory.Statistics.IsStatisticsEnabled)
					{
						source.Factory.StatisticsImplementor.OptimisticFailure(entityName);
					}

					throw new StaleObjectStateException(persister.EntityName, id);
				}

				await (CascadeOnMergeAsync(source, persister, entity, copyCache));
				await (CopyValuesAsync(persister, entity, target, source, copyCache));
				//copyValues works by reflection, so explicitly mark the entity instance dirty
				MarkInterceptorDirty(entity, target);
				@event.Result = result;
			}
		}

		private static async Task<bool> IsVersionChangedAsync(object entity, IEventSource source, IEntityPersister persister, object target)
		{
			if (!persister.IsVersioned)
			{
				return false;
			}

			// for merging of versioned entities, we consider the version having
			// been changed only when:
			// 1) the two version values are different;
			//      *AND*
			// 2) The target actually represents database state!
			//
			// This second condition is a special case which allows
			// an entity to be merged during the same transaction
			// (though during a separate operation) in which it was
			// originally persisted/saved
			bool changed = !await (persister.VersionType.IsSameAsync(persister.GetVersion(target, source.EntityMode), persister.GetVersion(entity, source.EntityMode), source.EntityMode));
			// TODO : perhaps we should additionally require that the incoming entity
			// version be equivalent to the defined unsaved-value?
			return changed && await (ExistsInDatabaseAsync(target, source, persister));
		}

		private static async Task<bool> ExistsInDatabaseAsync(object entity, IEventSource source, IEntityPersister persister)
		{
			EntityEntry entry = source.PersistenceContext.GetEntry(entity);
			if (entry == null)
			{
				object id = await (persister.GetIdentifierAsync(entity, source.EntityMode));
				if (id != null)
				{
					EntityKey key = source.GenerateEntityKey(id, persister);
					object managedEntity = source.PersistenceContext.GetEntity(key);
					entry = source.PersistenceContext.GetEntry(managedEntity);
				}
			}

			if (entry == null)
			{
				// perhaps this should be an exception since it is only ever used
				// in the above method?
				return false;
			}
			else
			{
				return entry.ExistsInDatabase;
			}
		}

		protected virtual async Task CopyValuesAsync(IEntityPersister persister, object entity, object target, ISessionImplementor source, IDictionary copyCache)
		{
			object[] copiedValues = await (TypeHelper.ReplaceAsync(persister.GetPropertyValues(entity, source.EntityMode), persister.GetPropertyValues(target, source.EntityMode), persister.PropertyTypes, source, target, copyCache));
			persister.SetPropertyValues(target, copiedValues, source.EntityMode);
		}

		protected virtual async Task CopyValuesAsync(IEntityPersister persister, object entity, object target, ISessionImplementor source, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
		{
			object[] copiedValues;
			if (foreignKeyDirection.Equals(ForeignKeyDirection.ForeignKeyToParent))
			{
				// this is the second pass through on a merge op, so here we limit the
				// replacement to associations types (value types were already replaced
				// during the first pass)
				copiedValues = await (TypeHelper.ReplaceAssociationsAsync(persister.GetPropertyValues(entity, source.EntityMode), persister.GetPropertyValues(target, source.EntityMode), persister.PropertyTypes, source, target, copyCache, foreignKeyDirection));
			}
			else
			{
				copiedValues = await (TypeHelper.ReplaceAsync(persister.GetPropertyValues(entity, source.EntityMode), persister.GetPropertyValues(target, source.EntityMode), persister.PropertyTypes, source, target, copyCache, foreignKeyDirection));
			}

			persister.SetPropertyValues(target, copiedValues, source.EntityMode);
		}

		/// <summary>
		/// Perform any cascades needed as part of this copy event.
		/// </summary>
		/// <param name = "source">The merge event being processed. </param>
		/// <param name = "persister">The persister of the entity being copied. </param>
		/// <param name = "entity">The entity being copied. </param>
		/// <param name = "copyCache">A cache of already copied instance. </param>
		protected virtual async Task CascadeOnMergeAsync(IEventSource source, IEntityPersister persister, object entity, IDictionary copyCache)
		{
			source.PersistenceContext.IncrementCascadeLevel();
			try
			{
				await (new Cascade(CascadeAction, CascadePoint.BeforeMerge, source).CascadeOnAsync(persister, entity, copyCache));
			}
			finally
			{
				source.PersistenceContext.DecrementCascadeLevel();
			}
		}

		/// <summary>
		/// Determine which merged entities in the copyCache are transient.
		/// </summary>
		/// <param name = "event"></param>
		/// <param name = "copyCache"></param>
		/// <returns></returns>
		/// <remarks>Should this method be on the EventCache class?</remarks>
		protected async Task<EventCache> GetTransientCopyCacheAsync(MergeEvent @event, EventCache copyCache)
		{
			EventCache transientCopyCache = new EventCache();
			foreach (object entity in copyCache.Keys)
			{
				object entityCopy = copyCache[entity];
				if (entityCopy.IsProxy())
					entityCopy = await (((INHibernateProxy)entityCopy).HibernateLazyInitializer.GetImplementationAsync());
				// NH-specific: Disregard entities that implement ILifecycle and manage their own state - they 
				// don't have an EntityEntry, and we can't determine if they are transient or not
				if (entityCopy is ILifecycle)
					continue;
				EntityEntry copyEntry = @event.Session.PersistenceContext.GetEntry(entityCopy);
				if (copyEntry == null)
				{
					// entity name will not be available for non-POJO entities
					// TODO: cache the entity name somewhere so that it is available to this exception
					log.InfoFormat("transient instance could not be processed by merge: {0} [{1}]", @event.Session.GuessEntityName(entityCopy), entity);
					// merge did not cascade to this entity; it's in copyCache because a
					// different entity has a non-nullable reference to it;
					// this entity should not be put in transientCopyCache, because it was
					// not included in the merge;
					throw new TransientObjectException("object is an unsaved transient instance - save the transient instance before merging: " + @event.Session.GuessEntityName(entityCopy));
				}
				else if (copyEntry.Status == Status.Saving)
				{
					transientCopyCache.Add(entity, entityCopy, copyCache.IsOperatedOn(entity));
				}
				else if (copyEntry.Status != Status.Loaded && copyEntry.Status != Status.ReadOnly)
				{
					throw new AssertionFailure(String.Format("Merged entity does not have status set to MANAGED or READ_ONLY; {0} status = {1}", entityCopy, copyEntry.Status));
				}
			}

			return transientCopyCache;
		}

		/// <summary>
		/// Retry merging transient entities
		/// </summary>
		/// <param name = "event"></param>
		/// <param name = "transientCopyCache"></param>
		/// <param name = "copyCache"></param>
		protected async Task RetryMergeTransientEntitiesAsync(MergeEvent @event, IDictionary transientCopyCache, EventCache copyCache)
		{
			// TODO: The order in which entities are saved may matter (e.g., a particular
			// transient entity may need to be saved before other transient entities can
			// be saved).
			// Keep retrying the batch of transient entities until either:
			// 1) there are no transient entities left in transientCopyCache
			// or 2) no transient entities were saved in the last batch.
			// For now, just run through the transient entities and retry the merge
			foreach (object entity in transientCopyCache.Keys)
			{
				object copy = transientCopyCache[entity];
				EntityEntry copyEntry = @event.Session.PersistenceContext.GetEntry(copy);
				if (entity == @event.Entity)
					await (MergeTransientEntityAsync(entity, copyEntry.EntityName, @event.RequestedId, @event.Session, copyCache));
				else
					await (MergeTransientEntityAsync(entity, copyEntry.EntityName, copyEntry.Id, @event.Session, copyCache));
			}
		}
	}
}
#endif
