#if NET_4_5
using System;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultSaveOrUpdateEventListener : AbstractSaveEventListener, ISaveOrUpdateEventListener
	{
		public virtual async Task OnSaveOrUpdateAsync(SaveOrUpdateEvent @event)
		{
			ISessionImplementor source = @event.Session;
			object obj = @event.Entity;
			object requestedId = @event.RequestedId;
			if (requestedId != null)
			{
				//assign the requested id to the proxy, *before* 
				//reassociating the proxy
				if (obj.IsProxy())
				{
					((INHibernateProxy)obj).HibernateLazyInitializer.Identifier = requestedId;
				}
			}

			if (ReassociateIfUninitializedProxy(obj, source))
			{
				log.Debug("reassociated uninitialized proxy");
			// an uninitialized proxy, noop, don't even need to 
			// return an id, since it is never a save()
			}
			else
			{
				//initialize properties of the event:
				object entity = await (source.PersistenceContext.UnproxyAndReassociateAsync(obj));
				@event.Entity = entity;
				@event.Entry = source.PersistenceContext.GetEntry(entity);
				//return the id in the event object
				@event.ResultId = await (PerformSaveOrUpdateAsync(@event));
			}
		}

		protected virtual async Task<object> PerformSaveOrUpdateAsync(SaveOrUpdateEvent @event)
		{
			EntityState entityState = await (GetEntityStateAsync(@event.Entity, @event.EntityName, @event.Entry, @event.Session));
			switch (entityState)
			{
				case EntityState.Detached:
					await (EntityIsDetachedAsync(@event));
					return null;
				case EntityState.Persistent:
					return await (EntityIsPersistentAsync(@event));
				default: //TRANSIENT or DELETED
					return await (EntityIsTransientAsync(@event));
			}
		}

		protected virtual async Task<object> EntityIsPersistentAsync(SaveOrUpdateEvent @event)
		{
			log.Debug("ignoring persistent instance");
			EntityEntry entityEntry = @event.Entry;
			if (entityEntry == null)
			{
				throw new AssertionFailure("entity was transient or detached");
			}
			else
			{
				if (entityEntry.Status == Status.Deleted)
				{
					throw new AssertionFailure("entity was deleted");
				}

				ISessionFactoryImplementor factory = @event.Session.Factory;
				object requestedId = @event.RequestedId;
				object savedId;
				if (requestedId == null)
				{
					savedId = entityEntry.Id;
				}
				else
				{
					if (!await (entityEntry.Persister.IdentifierType.IsEqualAsync(requestedId, entityEntry.Id, EntityMode.Poco)))
					{
						throw new PersistentObjectException("object passed to save() was already persistent: " + MessageHelper.InfoString(entityEntry.Persister, requestedId, factory));
					}

					savedId = requestedId;
				}

				if (log.IsDebugEnabled)
				{
					log.Debug("object already associated with session: " + MessageHelper.InfoString(entityEntry.Persister, savedId, factory));
				}

				return savedId;
			}
		}

		/// <summary> 
		/// The given save-update event named a transient entity.
		/// Here, we will perform the save processing. 
		/// </summary>
		/// <param name = "event">The save event to be handled. </param>
		/// <returns> The entity's identifier after saving. </returns>
		protected virtual async Task<object> EntityIsTransientAsync(SaveOrUpdateEvent @event)
		{
			log.Debug("saving transient instance");
			IEventSource source = @event.Session;
			EntityEntry entityEntry = @event.Entry;
			if (entityEntry != null)
			{
				if (entityEntry.Status == Status.Deleted)
				{
					await (source.ForceFlushAsync(entityEntry));
				}
				else
				{
					throw new AssertionFailure("entity was persistent");
				}
			}

			object id = await (SaveWithGeneratedOrRequestedIdAsync(@event));
			source.PersistenceContext.ReassociateProxy(@event.Entity, id);
			return id;
		}

		/// <summary> 
		/// Save the transient instance, assigning the right identifier 
		/// </summary>
		/// <param name = "event">The initiating event. </param>
		/// <returns> The entity's identifier value after saving.</returns>
		protected virtual async Task<object> SaveWithGeneratedOrRequestedIdAsync(SaveOrUpdateEvent @event)
		{
			if (@event.RequestedId == null)
			{
				return await (SaveWithGeneratedIdAsync(@event.Entity, @event.EntityName, null, @event.Session, true));
			}
			else
			{
				return await (SaveWithRequestedIdAsync(@event.Entity, @event.RequestedId, @event.EntityName, null, @event.Session));
			}
		}

		/// <summary> 
		/// The given save-update event named a detached entity.
		/// Here, we will perform the update processing. 
		/// </summary>
		/// <param name = "event">The update event to be handled. </param>
		protected virtual async Task EntityIsDetachedAsync(SaveOrUpdateEvent @event)
		{
			log.Debug("updating detached instance");
			if (@event.Session.PersistenceContext.IsEntryFor(@event.Entity))
			{
				//TODO: assertion only, could be optimized away
				throw new AssertionFailure("entity was persistent");
			}

			object entity = @event.Entity;
			IEntityPersister persister = @event.Session.GetEntityPersister(@event.EntityName, entity);
			@event.RequestedId = await (GetUpdateIdAsync(entity, persister, @event.RequestedId, @event.Session.EntityMode));
			await (PerformUpdateAsync(@event, entity, persister));
		}

		/// <summary> Determine the id to use for updating. </summary>
		/// <param name = "entity">The entity. </param>
		/// <param name = "persister">The entity persister </param>
		/// <param name = "requestedId">The requested identifier </param>
		/// <param name = "entityMode">The entity mode. </param>
		/// <returns> The id. </returns>
		protected virtual async Task<object> GetUpdateIdAsync(object entity, IEntityPersister persister, object requestedId, EntityMode entityMode)
		{
			// use the id assigned to the instance
			object id = await (persister.GetIdentifierAsync(entity, entityMode));
			if (id == null)
			{
				// assume this is a newly instantiated transient object
				// which should be saved rather than updated
				throw new TransientObjectException("The given object has a null identifier: " + persister.EntityName);
			}
			else
			{
				return id;
			}
		}

		protected virtual async Task PerformUpdateAsync(SaveOrUpdateEvent @event, object entity, IEntityPersister persister)
		{
			if (!persister.IsMutable)
			{
				log.Debug("immutable instance passed to PerformUpdate(), locking");
			}

			if (log.IsDebugEnabled)
			{
				log.Debug("updating " + MessageHelper.InfoString(persister, @event.RequestedId, @event.Session.Factory));
			}

			IEventSource source = @event.Session;
			EntityKey key = source.GenerateEntityKey(@event.RequestedId, persister);
			source.PersistenceContext.CheckUniqueness(key, entity);
			if (InvokeUpdateLifecycle(entity, persister, source))
			{
				await (ReassociateAsync(@event, @event.Entity, @event.RequestedId, persister));
				return;
			}

			// this is a transient object with existing persistent state not loaded by the session
			await (new OnUpdateVisitor(source, @event.RequestedId, entity).ProcessAsync(entity, persister));
			//TODO: put this stuff back in to read snapshot from
			//      the second-level cache (needs some extra work)
			/*Object[] cachedState = null;
			
			if ( persister.hasCache() ) {
			CacheEntry entry = (CacheEntry) persister.getCache()
			.get( event.getRequestedId(), source.getTimestamp() );
			cachedState = entry==null ? 
			null : 
			entry.getState(); //TODO: half-assemble this stuff
			}*/
			source.PersistenceContext.AddEntity(entity, persister.IsMutable ? Status.Loaded : Status.ReadOnly, null, key, persister.GetVersion(entity, source.EntityMode), LockMode.None, true, persister, false, true);
			//persister.AfterReassociate(entity, source); TODO H3.2 not ported
			if (log.IsDebugEnabled)
			{
				log.Debug("updating " + MessageHelper.InfoString(persister, @event.RequestedId, source.Factory));
			}

			await (CascadeOnUpdateAsync(@event, persister, entity));
		}

		/// <summary> 
		/// Handles the calls needed to perform cascades as part of an update request
		/// for the given entity. 
		/// </summary>
		/// <param name = "event">The event currently being processed. </param>
		/// <param name = "persister">The defined persister for the entity being updated. </param>
		/// <param name = "entity">The entity being updated. </param>
		private async Task CascadeOnUpdateAsync(SaveOrUpdateEvent @event, IEntityPersister persister, object entity)
		{
			IEventSource source = @event.Session;
			source.PersistenceContext.IncrementCascadeLevel();
			try
			{
				await (new Cascade(CascadingAction.SaveUpdate, CascadePoint.AfterUpdate, source).CascadeOnAsync(persister, entity));
			}
			finally
			{
				source.PersistenceContext.DecrementCascadeLevel();
			}
		}
	}
}
#endif
