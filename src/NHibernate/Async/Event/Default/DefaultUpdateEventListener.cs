#if NET_4_5
using System;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultUpdateEventListener : DefaultSaveOrUpdateEventListener
	{
		protected override async Task<object> PerformSaveOrUpdateAsync(SaveOrUpdateEvent @event)
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
					return EntityIsPersistent(@event);
				}
			}
			else
			{
				await (EntityIsDetachedAsync(@event));
				return null;
			}
		}

		protected override Task<object> SaveWithGeneratedOrRequestedIdAsync(SaveOrUpdateEvent @event)
		{
			return SaveWithGeneratedIdAsync(@event.Entity, @event.EntityName, null, @event.Session, true);
		}
	}
}
#endif
