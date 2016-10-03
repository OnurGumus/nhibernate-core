#if NET_4_5
using log4net;
using NHibernate.Cfg;
using NUnit.Framework;
using log4net.Core;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1093
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.CacheProvider, string.Empty);
			configuration.SetProperty(Environment.UseQueryCache, "true");
			configuration.SetProperty(Environment.UseSecondLevelCache, "true");
		}

		protected override string CacheConcurrencyStrategy
		{
			get
			{
				return null;
			}
		}

		private async Task CleanupAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				using (s.BeginTransaction())
				{
					await (s.CreateQuery("delete from SimpleCached").ExecuteUpdateAsync());
					await (s.Transaction.CommitAsync());
				}
			}
		}

		private async Task FillDbAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new SimpleCached{Description = "Simple 1"}));
					await (s.SaveAsync(new SimpleCached{Description = "Simple 2"}));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		[Description("Without configured cache, shouldn't throw exception")]
		public async Task NoExceptionAsync()
		{
			await (FillDbAsync());
			await (NormalListAsync());
			await (CriteriaQueryCacheAsync());
			await (HqlQueryCacheAsync());
			await (CleanupAsync());
		}

		private async Task HqlQueryCacheAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.CreateQuery("from SimpleCached").SetCacheable(true).ListAsync<SimpleCached>());
			}
		}

		private async Task CriteriaQueryCacheAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.CreateCriteria<SimpleCached>().SetCacheable(true).ListAsync<SimpleCached>());
			}
		}

		private async Task NormalListAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.CreateCriteria<SimpleCached>().ListAsync<SimpleCached>());
			}
		}

		protected override void BuildSessionFactory()
		{
			// Without configured cache, should log warn.
			using (var ls = new LogSpy(LogManager.GetLogger("NHibernate"), Level.Warn))
			{
				base.BuildSessionFactory();
				Assert.That(ls.GetWholeLog(), Is.StringContaining("Fake cache used"));
			}
		}
	}
}
#endif
