#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1925
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private const string NAME_JOE = "Joe";
		private const string NAME_ALLEN = "Allen";
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var joe = new Customer{Name = NAME_JOE};
					await (session.SaveAsync(joe));
					var allen = new Customer{Name = NAME_ALLEN};
					await (session.SaveAsync(allen));
					var joeInvoice0 = new Invoice{Customer = joe, Number = 0};
					await (session.SaveAsync(joeInvoice0));
					var allenInvoice1 = new Invoice{Customer = allen, Number = 1};
					await (session.SaveAsync(allenInvoice1));
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

		private async Task FindJoesLatestInvoiceAsync(string hql)
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					IList list = await (session.CreateQuery(hql).SetString("name", NAME_JOE).ListAsync());
					Assert.That(list, Is.Not.Empty);
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task Query1Async()
		{
			await (FindJoesLatestInvoiceAsync(@"
                    select invoice
                    from Invoice invoice
                        join invoice.Customer customer
                    where
                        invoice.Number = (
                            select max(invoice2.Number) 
                            from 
                                invoice.Customer d2
                                    join d2.Invoices invoice2
                            where
                                d2 = customer
                        )
                        and customer.Name = :name
                "));
		}

		[Test]
		public async Task Query2Async()
		{
			await (FindJoesLatestInvoiceAsync(@"
                    select invoice
                    from Invoice invoice
                        join invoice.Customer customer
                    where
                        invoice.Number = (select max(invoice2.Number) from customer.Invoices invoice2)
                        and customer.Name = :name
                "));
		}
	}
}
#endif
