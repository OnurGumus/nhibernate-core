using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Event;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2322
{
	public class PostUpdateEventListener : IPostUpdateEventListener
	{
		Task IPostUpdateEventListener.OnPostUpdate(PostUpdateEvent @event)
		{
			if (@event.Entity is Person)
			{
				@event.Session
					.CreateSQLQuery("update Person set Name = :newName")
					.SetString("newName", "new updated name")
					.ExecuteUpdate();
			}
			return TaskHelper.CompletedTask;
		}
	}
}