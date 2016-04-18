using System;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary> An event handler for update() events</summary>
	[Serializable]
	public class DefaultUpdateEventListener : DefaultSaveOrUpdateEventListener
	{
		protected override Task<object> PerformSaveOrUpdate(SaveOrUpdateEvent @event, bool async)
		{
			// this implementation is supposed to tolerate incorrect unsaved-value
			// mappings, for the purpose of backward-compatibility
			EntityEntry entry = @event.Session.PersistenceContext.GetEntry(@event.Entity);
			if (entry != null)
			{
				if (entry.Status == Status.Deleted)
				{
					throw new ObjectDeletedException("deleted instance passed to update()", null, @event.EntityName);
				}
				else
				{
					return Task.FromResult(EntityIsPersistent(@event));
				}
			}
			else
			{
				EntityIsDetached(@event);
				return Task.FromResult<object>(null);
			}
		}

		protected override Task<object> SaveWithGeneratedOrRequestedId(SaveOrUpdateEvent @event, bool async)
		{
			return SaveWithGeneratedId(@event.Entity, @event.EntityName, null, @event.Session, true, async);
		}

		/// <summary> 
		/// If the user specified an id, assign it to the instance and use that, 
		/// otherwise use the id already assigned to the instance
		/// </summary>
		protected override object GetUpdateId(object entity, IEntityPersister persister, object requestedId, EntityMode entityMode)
		{
			if (requestedId == null)
			{
				return base.GetUpdateId(entity, persister, requestedId, entityMode);
			}
			else
			{
				persister.SetIdentifier(entity, requestedId, entityMode);
				return requestedId;
			}
		}
	}
}
