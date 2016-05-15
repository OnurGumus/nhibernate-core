#if NET_4_5
using System;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultFlushEventListener : AbstractFlushingEventListener, IFlushEventListener
	{
		public virtual async Task OnFlushAsync(FlushEvent @event)
		{
			IEventSource source = @event.Session;
			if ((source.PersistenceContext.EntityEntries.Count > 0) || (source.PersistenceContext.CollectionEntries.Count > 0))
			{
				await (FlushEverythingToExecutionsAsync(@event));
				await (PerformExecutionsAsync(source));
				await (PostFlushAsync(source));
				if (source.Factory.Statistics.IsStatisticsEnabled)
				{
					source.Factory.StatisticsImplementor.Flush();
				}
			}
		}
	}
}
#endif
