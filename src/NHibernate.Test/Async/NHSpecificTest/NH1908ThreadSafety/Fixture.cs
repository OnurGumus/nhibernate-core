#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1908ThreadSafety
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task UsingFiltersIsThreadSafeAsync()
		{
			try
			{
				UsingFiltersIsThreadSafe();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		private async Task ScenarioRunningWithMultiThreadingAsync()
		{
			using (var session = sessions.OpenSession())
			{
				session.EnableFilter("CurrentOnly").SetParameter("date", DateTime.Now);
				await (session.CreateQuery(@"
				select u
				from Order u
					left join fetch u.ActiveOrderLines
				where
					u.Email = :email
				").SetString("email", "stupid@bugs.com").UniqueResultAsync<Order>());
			}
		}
	}
}
#endif
