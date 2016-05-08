using System;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultReplicateEventListener : AbstractSaveEventListener, IReplicateEventListener
	{
		public virtual async Task OnReplicateAsync(ReplicateEvent @event)
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
				oldVersion = await (persister.GetCurrentVersionAsync(id, source));
			}

			if (oldVersion != null)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("found existing row for " + MessageHelper.InfoString(persister, id, source.Factory));
				}

				// HHH-2378
				object realOldVersion = persister.IsVersioned ? oldVersion : null;
				bool canReplicate = replicationMode.ShouldOverwriteCurrentVersion(entity, realOldVersion, persister.GetVersion(entity, source.EntityMode), persister.VersionType);
				if (canReplicate)
				{
					//will result in a SQL UPDATE:
					PerformReplication(entity, id, realOldVersion, persister, replicationMode, source);
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
				PerformSaveOrReplicate(entity, key, persister, regenerate, replicationMode, source, true);
			}
		}
	}
}