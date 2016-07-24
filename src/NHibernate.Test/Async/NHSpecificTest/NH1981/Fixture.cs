#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1981
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			// Firebird doesn't support this feature
			return !(dialect is Dialect.FirebirdDialect);
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new Article{Longitude = 90}));
					await (s.SaveAsync(new Article{Longitude = 90}));
					await (s.SaveAsync(new Article{Longitude = 120}));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Article"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanGroupWithParameterAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					const string queryString = @"select (Longitude / :divisor)
					  from Article
					  group by (Longitude / :divisor)
					  order by 1";
					var quotients = await (s.CreateQuery(queryString).SetDouble("divisor", 30).ListAsync<double>());
					Assert.That(quotients.Count, Is.EqualTo(2));
					Assert.That(quotients[0], Is.EqualTo(3));
					Assert.That(quotients[1], Is.EqualTo(4));
				}
		}
	}
}
#endif
