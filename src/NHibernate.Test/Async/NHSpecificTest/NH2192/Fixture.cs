#if NET_4_5
using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2192
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task HqlIsThreadsafe_UsingThreadsAsync()
		{
			try
			{
				HqlIsThreadsafe_UsingThreads();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task HqlIsThreadsafe_UsingPoolAsync()
		{
			try
			{
				HqlIsThreadsafe_UsingPool();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		private async Task<int> FetchRowResultsAsync()
		{
			using (var s = Sfi.OpenSession())
			{
				var count = (await (s.CreateQuery("select ci from ContentItem ci where ci.Name = :v1").SetParameter("v1", "Test").ListAsync<ContentItem>())).Count;
				return count;
			}
		}
	}
}
#endif
