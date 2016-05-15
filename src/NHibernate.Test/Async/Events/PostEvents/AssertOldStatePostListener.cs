#if NET_4_5
using log4net;
using NHibernate.Event;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.Events.PostEvents
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AssertOldStatePostListener : IPostUpdateEventListener
	{
		public Task OnPostUpdateAsync(PostUpdateEvent @event)
		{
			try
			{
				OnPostUpdate(event);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
