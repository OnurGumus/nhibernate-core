#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2037
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
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
					session.SaveOrUpdate(city);
					await (tx.CommitAsync());
				}

			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					Assert.IsNotNull(await (session.GetAsync<City>(city.Id)));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
