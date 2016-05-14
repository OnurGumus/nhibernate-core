#if NET_4_5
using System;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultDirtyCheckEventListener : AbstractFlushingEventListener, IDirtyCheckEventListener
	{
		public virtual async Task OnDirtyCheckAsync(DirtyCheckEvent @event)
		{
			int oldSize = @event.Session.ActionQueue.CollectionRemovalsCount;
			try
			{
				await (FlushEverythingToExecutionsAsync(@event));
				bool wasNeeded = @event.Session.ActionQueue.HasAnyQueuedActions;
				log.Debug(wasNeeded ? "session dirty" : "session not dirty");
				@event.Dirty = wasNeeded;
			}
			finally
			{
				@event.Session.ActionQueue.ClearFromFlushNeededCheck(oldSize);
			}
		}
	}
}
#endif
