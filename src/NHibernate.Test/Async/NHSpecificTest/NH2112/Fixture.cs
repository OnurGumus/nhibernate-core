#if NET_4_5
using NUnit.Framework;
using NHibernate.Cfg;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2112
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.GenerateStatistics, "true");
			configuration.SetProperty(Environment.BatchSize, "0");
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateSQLQuery("DELETE FROM AMapB").ExecuteUpdateAsync());
					await (s.CreateSQLQuery("DELETE FROM TableA").ExecuteUpdateAsync());
					await (s.CreateSQLQuery("DELETE FROM TableB").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestAsync()
		{
			A a;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					a = new A();
					a.Name = "A";
					B b1 = new B{Name = "B1"};
					await (s.SaveAsync(b1));
					B b2 = new B{Name = "B2"};
					await (s.SaveAsync(b2));
					a.Map.Add(b1, "B1Text");
					a.Map.Add(b2, "B2Text");
					await (s.SaveAsync(a));
					await (s.FlushAsync());
					await (tx.CommitAsync());
				}

			ClearCounts();
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					A aCopy = (A)s.Merge(a);
					await (s.FlushAsync());
					await (tx.CommitAsync());
				}

			AssertUpdateCount(0);
			AssertInsertCount(0);
		}

		protected void ClearCounts()
		{
			sessions.Statistics.Clear();
		}

		protected void AssertInsertCount(long expected)
		{
			Assert.That(sessions.Statistics.EntityInsertCount, Is.EqualTo(expected), "unexpected insert count");
		}

		protected void AssertUpdateCount(int expected)
		{
			Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(expected), "unexpected update count");
		}

		protected void AssertDeleteCount(int expected)
		{
			Assert.That(sessions.Statistics.EntityDeleteCount, Is.EqualTo(expected), "unexpected delete count");
		}
	}
}
#endif
