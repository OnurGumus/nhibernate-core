#if NET_4_5
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2033
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task QueryOverJoinAliasOnKeyManyToOneShouldGenerateInnerJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					Customer customerAlias = null;
					var query = session.QueryOver<CustomerAddress>().Where(x => x.City == "New York").JoinAlias(x => x.Customer, () => customerAlias).Where(() => customerAlias.Name == "John");
					var results = await (query.ListAsync());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0].Address, Is.EqualTo("123 E West Ave."));
				}
		}

		[Test]
		public async Task QueryOverJoinAliasOnManyToOneShouldGenerateInnerJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					Customer customerAlias = null;
					var query = session.QueryOver<CustomerAddress>().Where(x => x.City == "New York").JoinAlias(x => x.OtherCustomer, () => customerAlias).Where(() => customerAlias.Name == "Other");
					var results = await (query.ListAsync());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0].Address, Is.EqualTo("123 E West Ave."));
					session.Clear();
				}
		}

		[Test]
		public async Task CreateCriteriaOnKeyManyToOneShouldGenerateInnerJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query = session.CreateCriteria<CustomerAddress>().Add(Restrictions.Eq("City", "New York")).CreateCriteria("Customer").Add(Restrictions.Eq("Name", "John"));
					var results = await (query.ListAsync<CustomerAddress>());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0].Address, Is.EqualTo("123 E West Ave."));
				}
		}

		[Test]
		public async Task HqlJoinOnKeyManyToOneShouldGenerateInnerJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query = session.CreateQuery(@"
						select a
						from
							CustomerAddress a
							join a.Customer c
						where
							a.City = :city
							and c.Name = :name").SetString("city", "New York").SetString("name", "John");
					var results = await (query.ListAsync<CustomerAddress>());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0].Address, Is.EqualTo("123 E West Ave."));
				}
		}
	}
}
#endif
