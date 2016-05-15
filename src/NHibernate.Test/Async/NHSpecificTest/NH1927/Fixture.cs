#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1927
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
