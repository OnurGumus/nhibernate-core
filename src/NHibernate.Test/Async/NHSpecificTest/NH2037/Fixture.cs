#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2037
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			var country = new Country{Name = "Argentina"};
			var city = new City{CityCode = 5, Country = country, Name = "Cordoba"};
			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.SaveAsync(city.Country));
					await (session.SaveAsync(city));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					//THROW
					await (session.SaveOrUpdateAsync(city));
					await (tx.CommitAsync());
				}

			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					Assert.IsNotNull(await (session.GetAsync<City>(city.Id)));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from City"));
					await (session.DeleteAsync("from Country"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
