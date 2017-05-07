﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;

namespace NHibernate.Event.Default
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class DefaultFlushEventListener : AbstractFlushingEventListener, IFlushEventListener
	{
		public virtual async Task OnFlushAsync(FlushEvent @event)
		{
			IEventSource source = @event.Session;

			if ((source.PersistenceContext.EntityEntries.Count > 0) || (source.PersistenceContext.CollectionEntries.Count > 0))
			{
				await (FlushEverythingToExecutionsAsync(@event)).ConfigureAwait(false);
				await (PerformExecutionsAsync(source)).ConfigureAwait(false);
				PostFlush(source);

				if (source.Factory.Statistics.IsStatisticsEnabled)
				{
					source.Factory.StatisticsImplementor.Flush();
				}
			}
		}
	}
}
