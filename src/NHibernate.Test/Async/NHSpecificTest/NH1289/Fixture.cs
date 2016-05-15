#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1289
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ManyToOne_gets_implicit_polymorphism_correctlyAsync()
		{
			using (var ses = OpenSession())
				using (var tran = ses.BeginTransaction())
				{
					var purchaseItem = await (ses.GetAsync<PurchaseItem>(1));
					Assert.That(purchaseItem, Is.AssignableFrom(typeof (Cons_PurchaseItem)));
					Assert.That(purchaseItem.Product, Is.AssignableFrom(typeof (Cons_Product)));
					await (tran.CommitAsync());
				}
		}
	}
}
#endif
