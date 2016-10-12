#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Event;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2322
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PostUpdateEventListener : IPostUpdateEventListener
	{
		Task IPostUpdateEventListener.OnPostUpdateAsync(PostUpdateEvent @event)
		{
			try
			{
				((IPostUpdateEventListener)this).OnPostUpdate(@event);
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
