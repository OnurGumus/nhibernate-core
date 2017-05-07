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
	public partial class DefaultDirtyCheckEventListener : AbstractFlushingEventListener, IDirtyCheckEventListener
	{

		public virtual async Task OnDirtyCheckAsync(DirtyCheckEvent @event)
		{
			int oldSize = @event.Session.ActionQueue.CollectionRemovalsCount;

			try
			{
				await (FlushEverythingToExecutionsAsync(@event)).ConfigureAwait(false);
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
