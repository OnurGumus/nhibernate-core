using System;
using System.Collections.Generic;
using NHibernate.Action;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using Status = NHibernate.Engine.Status;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultDeleteEventListener : IDeleteEventListener
	{
		protected virtual async Task DeleteEntityAsync(IEventSource session, object entity, EntityEntry entityEntry, bool isCascadeDeleteEnabled, IEntityPersister persister, ISet<object> transientEntities)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("deleting " + MessageHelper.InfoString(persister, entityEntry.Id, session.Factory));
			}

			IPersistenceContext persistenceContext = session.PersistenceContext;
			IType[] propTypes = persister.PropertyTypes;
			object version = entityEntry.Version;
			object[] currentState;
			if (entityEntry.LoadedState == null)
			{
				//ie. the entity came in from update()
				currentState = persister.GetPropertyValues(entity, session.EntityMode);
			}
			else
			{
				currentState = entityEntry.LoadedState;
			}

			object[] deletedState = await (CreateDeletedStateAsync(persister, currentState, session));
			entityEntry.DeletedState = deletedState;
			session.Interceptor.OnDelete(entity, entityEntry.Id, deletedState, persister.PropertyNames, propTypes);
			// before any callbacks, etc, so subdeletions see that this deletion happened first
			persistenceContext.SetEntryStatus(entityEntry, Status.Deleted);
			EntityKey key = session.GenerateEntityKey(entityEntry.Id, persister);
			await (CascadeBeforeDeleteAsync(session, persister, entity, entityEntry, transientEntities));
			await (new ForeignKeys.Nullifier(entity, true, false, session).NullifyTransientReferencesAsync(entityEntry.DeletedState, propTypes));
			await (new Nullability(session).CheckNullabilityAsync(entityEntry.DeletedState, persister, true));
			persistenceContext.NullifiableEntityKeys.Add(key);
			// Ensures that containing deletions happen before sub-deletions
			session.ActionQueue.AddAction(new EntityDeleteAction(entityEntry.Id, deletedState, version, entity, persister, isCascadeDeleteEnabled, session));
			await (CascadeAfterDeleteAsync(session, persister, entity, transientEntities));
		// the entry will be removed after the flush, and will no longer
		// override the stale snapshot
		// This is now handled by removeEntity() in EntityDeleteAction
		//persistenceContext.removeDatabaseSnapshot(key);
		}

		public virtual async Task OnDeleteAsync(DeleteEvent @event, ISet<object> transientEntities)
		{
			IEventSource source = @event.Session;
			IPersistenceContext persistenceContext = source.PersistenceContext;
			object entity = await (persistenceContext.UnproxyAndReassociateAsync(@event.Entity));
			EntityEntry entityEntry = persistenceContext.GetEntry(entity);
			IEntityPersister persister;
			object id;
			object version;
			if (entityEntry == null)
			{
				log.Debug("entity was not persistent in delete processing");
				persister = source.GetEntityPersister(@event.EntityName, entity);
				if (await (ForeignKeys.IsTransientAsync(persister.EntityName, entity, null, source)))
				{
					await (DeleteTransientEntityAsync(source, entity, @event.CascadeDeleteEnabled, persister, transientEntities));
					// EARLY EXIT!!!
					return;
				}
				else
				{
					PerformDetachedEntityDeletionCheck(@event);
				}

				id = await (persister.GetIdentifierAsync(entity, source.EntityMode));
				if (id == null)
				{
					throw new TransientObjectException("the detached instance passed to delete() had a null identifier");
				}

				EntityKey key = source.GenerateEntityKey(id, persister);
				persistenceContext.CheckUniqueness(key, entity);
				await (new OnUpdateVisitor(source, id, entity).ProcessAsync(entity, persister));
				version = persister.GetVersion(entity, source.EntityMode);
				entityEntry = persistenceContext.AddEntity(entity, persister.IsMutable ? Status.Loaded : Status.ReadOnly, persister.GetPropertyValues(entity, source.EntityMode), key, version, LockMode.None, true, persister, false, false);
			}
			else
			{
				log.Debug("deleting a persistent instance");
				if (entityEntry.Status == Status.Deleted || entityEntry.Status == Status.Gone)
				{
					log.Debug("object was already deleted");
					return;
				}

				persister = entityEntry.Persister;
				id = entityEntry.Id;
				version = entityEntry.Version;
			}

			if (InvokeDeleteLifecycle(source, entity, persister))
				return;
			await (DeleteEntityAsync(source, entity, entityEntry, @event.CascadeDeleteEnabled, persister, transientEntities));
			if (source.Factory.Settings.IsIdentifierRollbackEnabled)
			{
				await (persister.ResetIdentifierAsync(entity, id, version, source.EntityMode));
			}
		}

		public virtual async Task OnDeleteAsync(DeleteEvent @event)
		{
			await (OnDeleteAsync(@event, new IdentitySet()));
		}

		private async Task<object[]> CreateDeletedStateAsync(IEntityPersister persister, object[] currentState, IEventSource session)
		{
			IType[] propTypes = persister.PropertyTypes;
			object[] deletedState = new object[propTypes.Length];
			//		TypeFactory.deepCopy( currentState, propTypes, persister.getPropertyUpdateability(), deletedState, session );
			bool[] copyability = new bool[propTypes.Length];
			ArrayHelper.Fill(copyability, true);
			await (TypeHelper.DeepCopyAsync(currentState, propTypes, copyability, deletedState, session));
			return deletedState;
		}

		protected virtual async Task CascadeAfterDeleteAsync(IEventSource session, IEntityPersister persister, object entity, ISet<object> transientEntities)
		{
			ISessionImplementor si = session;
			CacheMode cacheMode = si.CacheMode;
			si.CacheMode = CacheMode.Get;
			session.PersistenceContext.IncrementCascadeLevel();
			try
			{
				// cascade-delete to many-to-one AFTER the parent was deleted
				await (new Cascade(CascadingAction.Delete, CascadePoint.BeforeInsertAfterDelete, session).CascadeOnAsync(persister, entity, transientEntities));
			}
			finally
			{
				session.PersistenceContext.DecrementCascadeLevel();
				si.CacheMode = cacheMode;
			}
		}

		protected virtual async Task DeleteTransientEntityAsync(IEventSource session, object entity, bool cascadeDeleteEnabled, IEntityPersister persister, ISet<object> transientEntities)
		{
			log.Info("handling transient entity in delete processing");
			// NH different impl : NH-1895
			if (transientEntities == null)
			{
				transientEntities = new HashSet<object>();
			}

			if (!transientEntities.Add(entity))
			{
				log.Debug("already handled transient entity; skipping");
				return;
			}

			await (CascadeBeforeDeleteAsync(session, persister, entity, null, transientEntities));
			await (CascadeAfterDeleteAsync(session, persister, entity, transientEntities));
		}

		protected virtual async Task CascadeBeforeDeleteAsync(IEventSource session, IEntityPersister persister, object entity, EntityEntry entityEntry, ISet<object> transientEntities)
		{
			ISessionImplementor si = session;
			CacheMode cacheMode = si.CacheMode;
			si.CacheMode = CacheMode.Get;
			session.PersistenceContext.IncrementCascadeLevel();
			try
			{
				// cascade-delete to collections BEFORE the collection owner is deleted
				await (new Cascade(CascadingAction.Delete, CascadePoint.AfterInsertBeforeDelete, session).CascadeOnAsync(persister, entity, transientEntities));
			}
			finally
			{
				session.PersistenceContext.DecrementCascadeLevel();
				si.CacheMode = cacheMode;
			}
		}
	}
}