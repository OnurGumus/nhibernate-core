#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Event;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2322
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PostUpdateEventListener : IPostUpdateEventListener
	{
		async Task IPostUpdateEventListener.OnPostUpdateAsync(PostUpdateEvent @event)
		{
			if (@event.Entity is Person)
			{
				await (@event.Session.CreateSQLQuery("update Person set Name = :newName").SetString("newName", "new updated name").ExecuteUpdateAsync());
			}
		}
	}
}
#endif
