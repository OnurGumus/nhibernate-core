#if NET_4_5
using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2192
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new ContentItem()
					{Name = "Test"}));
					await (s.SaveAsync(new ContentItem()
					{Name = "Test"}));
					await (s.SaveAsync(new ContentItem()
					{Name = "Test2"}));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from ContentItem"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		private const int _threadCount = 150;
		private int FetchRowResults()
		{
			using (var s = Sfi.OpenSession())
			{
				var count = s.CreateQuery("select ci from ContentItem ci where ci.Name = :v1").SetParameter("v1", "Test").List<ContentItem>().Count;
				return count;
			}
		}
	}
}
#endif
