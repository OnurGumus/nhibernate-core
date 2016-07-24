#if NET_4_5
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2033
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var john = new Customer{AssignedId = 1, Name = "John"};
					var other = new Customer{AssignedId = 2, Name = "Other"};
					var johnBusiness = new CustomerAddress{Customer = john, Type = "Business", Address = "123 E West Ave.", City = "New York", OtherCustomer = other};
					await (session.SaveAsync(john));
					await (session.SaveAsync(other));
					await (session.SaveAsync(johnBusiness));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from CustomerAddress").ExecuteUpdateAsync());
					await (session.CreateQuery("delete from Customer").ExecuteUpdateAsync());
					await (transaction.CommitAsync());
				}
		}

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
		public void LinqJoinOnKeyManyToOneShouldGenerateInnerJoin()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query = session.Query<CustomerAddress>().Where(x => x.City == "New York").Where(x => x.Customer.Name == "John");
					var results = query.ToList();
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0].Address, Is.EqualTo("123 E West Ave."));
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
