#if NET_4_5
using System;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultAutoFlushEventListener : AbstractFlushingEventListener, IAutoFlushEventListener
	{
		/// <summary>
		/// Handle the given auto-flush event.
		/// </summary>
		/// <param name = "event">The auto-flush event to be handled.</param>
		public virtual async Task OnAutoFlushAsync(AutoFlushEvent @event)
		{
			IEventSource source = @event.Session;
			if (FlushMightBeNeeded(source))
			{
				int oldSize = source.ActionQueue.CollectionRemovalsCount;
				await (FlushEverythingToExecutionsAsync(@event));
				if (FlushIsReallyNeeded(@event, source))
				{
					if (log.IsDebugEnabled)
						log.Debug("Need to execute flush");
					await (PerformExecutionsAsync(source));
					PostFlush(source);
					// note: performExecutions() clears all collectionXxxxtion
					// collections (the collection actions) in the session
					if (source.Factory.Statistics.IsStatisticsEnabled)
					{
						source.Factory.StatisticsImplementor.Flush();
					}
				}
				else
				{
					if (log.IsDebugEnabled)
						log.Debug("Dont need to execute flush");
					source.ActionQueue.ClearFromFlushNeededCheck(oldSize);
				}

				@event.FlushRequired = FlushIsReallyNeeded(@event, source);
			}
		}
	}
}
#endif
