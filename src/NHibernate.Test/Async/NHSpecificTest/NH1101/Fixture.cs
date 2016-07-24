#if NET_4_5
using NHibernate.Stat;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1101
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task ConfigureAsync(Cfg.Configuration configuration)
		{
			await (base.ConfigureAsync(configuration));
			cfg.SetProperty(Cfg.Environment.GenerateStatistics, "true");
		}

		[Test]
		public async Task BehaviorAsync()
		{
			object savedId;
			A a = new A();
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					savedId = await (s.SaveAsync(a));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					a = await (s.GetAsync<A>(savedId));
					IStatistics statistics = sessions.Statistics;
					statistics.Clear();
					Assert.IsNotNull(a.B); // an instance of B was created
					await (s.FlushAsync());
					await (t.CommitAsync());
					// since we don't change anyproperties in a.B there are no dirty entity to commit
					Assert.AreEqual(0, statistics.PrepareStatementCount);
				}

			// using proxy
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					a = await (s.LoadAsync<A>(savedId));
					IStatistics statistics = sessions.Statistics;
					statistics.Clear();
					Assert.IsNotNull(a.B); // an instance of B was created
					await (s.FlushAsync());
					await (t.CommitAsync());
					Assert.AreEqual(0, statistics.PrepareStatementCount);
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from A"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
