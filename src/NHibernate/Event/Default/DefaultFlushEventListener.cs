using System;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Defines the default flush event listeners used by hibernate for 
	/// flushing session state in response to generated flush events. 
	/// </summary>
	[Serializable]
	public class DefaultFlushEventListener : AbstractFlushingEventListener, IFlushEventListener
	{
		public virtual async Task OnFlush(FlushEvent @event, bool async)
		{
			IEventSource source = @event.Session;

			if ((source.PersistenceContext.EntityEntries.Count > 0) || (source.PersistenceContext.CollectionEntries.Count > 0))
			{
				FlushEverythingToExecutions(@event);
				await PerformExecutions(source,async);
				PostFlush(source);

				if (source.Factory.Statistics.IsStatisticsEnabled)
				{
					source.Factory.StatisticsImplementor.Flush();
				}
			}
		}
	}
}
