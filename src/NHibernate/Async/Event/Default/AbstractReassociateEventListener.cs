using System;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using Status = NHibernate.Engine.Status;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AbstractReassociateEventListener
	{
		protected async Task<EntityEntry> ReassociateAsync(AbstractEvent @event, object entity, object id, IEntityPersister persister)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("Reassociating transient instance: " + MessageHelper.InfoString(persister, id, @event.Session.Factory));
			}

			IEventSource source = @event.Session;
			EntityKey key = source.GenerateEntityKey(id, persister);
			source.PersistenceContext.CheckUniqueness(key, entity);
			//get a snapshot
			object[] values = persister.GetPropertyValues(entity, source.EntityMode);
			await (TypeHelper.DeepCopyAsync(values, persister.PropertyTypes, persister.PropertyUpdateability, values, source));
			object version = Versioning.GetVersion(values, persister);
			EntityEntry newEntry = source.PersistenceContext.AddEntity(entity, persister.IsMutable ? Status.Loaded : Status.ReadOnly, values, key, version, LockMode.None, true, persister, false, true);
			await (new OnLockVisitor(source, id, entity).ProcessAsync(entity, persister));
			persister.AfterReassociate(entity, source);
			return newEntry;
		}
	}
}