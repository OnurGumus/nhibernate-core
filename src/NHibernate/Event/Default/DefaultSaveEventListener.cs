using System;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary> An event handler for save() events</summary>
	[Serializable]
	public class DefaultSaveEventListener : DefaultSaveOrUpdateEventListener
	{
		protected override Task<object> PerformSaveOrUpdate(SaveOrUpdateEvent @event, bool async)
		{
			// this implementation is supposed to tolerate incorrect unsaved-value
			// mappings, for the purpose of backward-compatibility
			EntityEntry entry = @event.Session.PersistenceContext.GetEntry(@event.Entity);
			if (entry != null && entry.Status != Status.Deleted)
			{
				return Task.FromResult(EntityIsPersistent(@event));
			}
			else
			{
				return EntityIsTransient(@event, async);
			}
		}

		protected override bool ReassociateIfUninitializedProxy(object obj, ISessionImplementor source)
		{
			if (!NHibernateUtil.IsInitialized(obj))
			{
                throw new PersistentObjectException("Uninitialized proxy passed to save(). Object: " + obj.ToString());
			}
			else
			{
				return false;
			}
		}
	}
}
