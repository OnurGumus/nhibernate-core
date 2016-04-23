using System;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Defines the default replicate event listener used by Hibernate to replicate
	/// entities in response to generated replicate events. 
	/// </summary>
	[Serializable]
	public class DefaultReplicateEventListener : AbstractSaveEventListener, IReplicateEventListener
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(DefaultReplicateEventListener));

		public virtual async Task OnReplicate(ReplicateEvent @event)
		{
			IEventSource source = @event.Session;
			if (source.PersistenceContext.ReassociateIfUninitializedProxy(@event.Entity))
			{
				log.Debug("uninitialized proxy passed to replicate()");
				return;
			}

			object entity = source.PersistenceContext.UnproxyAndReassociate(@event.Entity);

			if (source.PersistenceContext.IsEntryFor(entity))
			{
				log.Debug("ignoring persistent instance passed to replicate()");
				//hum ... should we cascade anyway? throw an exception? fine like it is?
				return;
			}

			IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);

			// get the id from the object
			/*if ( persister.isUnsaved(entity, source) ) {
			throw new TransientObjectException("transient instance passed to replicate()");
			}*/
			object id = persister.GetIdentifier(entity, source.EntityMode);
			if (id == null)
			{
				throw new TransientObjectException("instance with null id passed to replicate()");
			}

			ReplicationMode replicationMode = @event.ReplicationMode;
			object oldVersion;
			if (replicationMode == ReplicationMode.Exception)
			{
				//always do an INSERT, and let it fail by constraint violation
				oldVersion = null;
			}
			else
			{
				//what is the version on the database?
				oldVersion = await persister.GetCurrentVersion(id, source).ConfigureAwait(false);
			}

			if (oldVersion != null)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("found existing row for " + MessageHelper.InfoString(persister, id, source.Factory));
				}

				// HHH-2378
				object realOldVersion = persister.IsVersioned ? oldVersion : null;

				bool canReplicate =
					replicationMode.ShouldOverwriteCurrentVersion(entity, realOldVersion,
																  persister.GetVersion(entity, source.EntityMode),
																  persister.VersionType);

				if (canReplicate)
				{
					//will result in a SQL UPDATE:
					await PerformReplication(entity, id, realOldVersion, persister, replicationMode, source).ConfigureAwait(false);
				}
				else
				{
					//else do nothing (don't even reassociate object!)
					log.Debug("no need to replicate");
				}

				//TODO: would it be better to do a refresh from db?
			}
			else
			{
				// no existing row - do an insert
				if (log.IsDebugEnabled)
				{
					log.Debug("no existing row, replicating new instance " + MessageHelper.InfoString(persister, id, source.Factory));
				}

				bool regenerate = persister.IsIdentifierAssignedByInsert; // prefer re-generation of identity!
				EntityKey key = regenerate ? null : source.GenerateEntityKey(id, persister);

				await PerformSaveOrReplicate(entity, key, persister, regenerate, replicationMode, source, true).ConfigureAwait(false);
			}
		}

		private async Task PerformReplication(object entity, object id, object version, IEntityPersister persister, ReplicationMode replicationMode, IEventSource source)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("replicating changes to " + MessageHelper.InfoString(persister, id, source.Factory));
			}

			await new OnReplicateVisitor(source, id, entity, true).Process(entity, persister).ConfigureAwait(false);

			source.PersistenceContext.AddEntity(
				entity,
				persister.IsMutable ? Status.Loaded : Status.ReadOnly,
				null,
				source.GenerateEntityKey(id, persister),
				version,
				LockMode.None,
				true,
				persister,
				true,
				false);

			await CascadeAfterReplicate(entity, persister, replicationMode, source).ConfigureAwait(false);
		}

		private async Task CascadeAfterReplicate(object entity, IEntityPersister persister, ReplicationMode replicationMode, IEventSource source)
		{
			source.PersistenceContext.IncrementCascadeLevel();
			try
			{
				await new Cascade(CascadingAction.Replicate, CascadePoint.AfterUpdate, source).CascadeOn(persister, entity, replicationMode).ConfigureAwait(false);
			}
			finally
			{
				source.PersistenceContext.DecrementCascadeLevel();
			}
		}

		protected override bool VersionIncrementDisabled
		{
			get { return true; }
		}

		protected override CascadingAction CascadeAction
		{
			get { return CascadingAction.Replicate; }
		}

		protected override bool SubstituteValuesIfNecessary(object entity, object id, object[] values, IEntityPersister persister, ISessionImplementor source)
		{
			return false;
		}

		protected override async Task<bool> VisitCollectionsBeforeSave(object entity, object id, object[] values, Type.IType[] types, IEventSource source)
		{
			//TODO: we use two visitors here, inefficient!
			OnReplicateVisitor visitor = new OnReplicateVisitor(source, id, entity, false);
			await visitor.ProcessEntityPropertyValues(values, types).ConfigureAwait(false);
			return await base.VisitCollectionsBeforeSave(entity, id, values, types, source).ConfigureAwait(false);
		}
	}
}
