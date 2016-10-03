#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1927
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private static readonly DateTime MAX_DATE = new DateTime(3000, 1, 1);
		private static readonly DateTime VALID_DATE = new DateTime(2000, 1, 1);
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var joe = new Customer()
					{ValidUntil = MAX_DATE};
					await (session.SaveAsync(joe));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Invoice"));
					await (session.DeleteAsync("from Customer"));
					await (tx.CommitAsync());
				}
			}

			await (base.OnTearDownAsync());
		}

		private delegate Customer QueryFactoryFunc(ISession session);
		private async Task TestQueryAsync(QueryFactoryFunc queryFactoryFunc)
		{
			// test without filter
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					Assert.That(queryFactoryFunc(session), Is.Not.Null, "failed with filter off");
					await (tx.CommitAsync());
				}

			// test with the validity filter
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					session.EnableFilter("validity").SetParameter("date", VALID_DATE);
					Assert.That(queryFactoryFunc(session), Is.Not.Null, "failed with filter on");
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CriteriaWithEagerFetchAsync()
		{
			await (TestQueryAsync(s => s.CreateCriteria(typeof (Customer)).SetFetchMode("Invoices", FetchMode.Eager).UniqueResult<Customer>()));
		}

		[Test]
		public async Task CriteriaWithoutEagerFetchAsync()
		{
			await (TestQueryAsync(s => s.CreateCriteria(typeof (Customer)).UniqueResult<Customer>()));
		}

		[Test]
		public async Task HqlWithEagerFetchAsync()
		{
			await (TestQueryAsync(s => s.CreateQuery(@"
                    select c
                    from Customer c
                        left join fetch c.Invoices").UniqueResult<Customer>()));
		}

		[Test]
		public async Task HqlWithoutEagerFetchAsync()
		{
			await (TestQueryAsync(s => s.CreateQuery(@"
                    select c
                    from Customer c").UniqueResult<Customer>()));
		}
	}
}
#endif
