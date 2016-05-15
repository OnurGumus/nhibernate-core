#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1925
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
