#if NET_4_5
using System;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultSaveEventListener : DefaultSaveOrUpdateEventListener
	{
		protected override async Task<object> PerformSaveOrUpdateAsync(SaveOrUpdateEvent @event)
		{
			// this implementation is supposed to tolerate incorrect unsaved-value
			// mappings, for the purpose of backward-compatibility
			EntityEntry entry = @event.Session.PersistenceContext.GetEntry(@event.Entity);
			if (entry != null && entry.Status != Status.Deleted)
			{
				return EntityIsPersistent(@event);
			}
			else
			{
				return await (EntityIsTransientAsync(@event));
			}
		}
	}
}
#endif
