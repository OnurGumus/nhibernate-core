#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1904
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ExecuteQueryAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					Invoice invoice = new Invoice();
					invoice.Issued = DateTime.Now;
					await (session.SaveAsync(invoice));
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
			{
				IList<Invoice> invoices = await (session.CreateCriteria<Invoice>().ListAsync<Invoice>());
			}
		}
	}
}
#endif
