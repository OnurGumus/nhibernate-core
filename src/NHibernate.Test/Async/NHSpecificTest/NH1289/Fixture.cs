#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1289
{
	[TestFixture, Ignore("")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var ses = OpenSession())
				using (var tran = ses.BeginTransaction())
				{
					var purchaseOrder = new Cons_PurchaseOrder{PurchaseItems = new HashSet<PurchaseItem>(), };
					var product = new Cons_Product{ProductName = "abc", Units = 5, Price = "123", Description = "desc", ImageName = "abc"};
					var purchaseItem = new Cons_PurchaseItem{Product = product, PurchaseOrder = purchaseOrder};
					purchaseOrder.PurchaseItems.Add(purchaseItem);
					await (ses.SaveAsync(product));
					await (ses.SaveAsync(purchaseOrder));
					await (ses.SaveAsync(purchaseItem));
					await (tran.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var ses = OpenSession())
				using (var tran = ses.BeginTransaction())
				{
					await (ses.DeleteAsync("from Cons_PurchaseOrder"));
					await (ses.DeleteAsync("from Cons_PurchaseItem"));
					await (ses.DeleteAsync("from Cons_Product"));
					await (tran.CommitAsync());
				}
		}

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
