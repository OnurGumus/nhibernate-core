#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH364
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task IdBagIdentityAsync()
		{
			using (ISession s = OpenSession())
			{
				Category cat1 = new Category();
				cat1.Name = "Cat 1";
				Link link1 = new Link();
				link1.Name = "Link 1";
				link1.Categories.Add(cat1);
				await (s.SaveAsync(cat1));
				await (s.SaveAsync(link1));
				await (s.FlushAsync());
				await (s.DeleteAsync("from Link"));
				await (s.DeleteAsync("from Category"));
				await (s.FlushAsync());
			}
		}

		private async Task IdBagWithCompositeElementThatContainsAManyToOne_SetupAsync()
		{
			using (ISession s = OpenSession())
			{
				product1 = new Product("Star Wars DVD");
				product2 = new Product("100TB Hard Drive");
				product3 = new Product("Something else");
				await (s.SaveAsync(product1));
				await (s.SaveAsync(product2));
				await (s.SaveAsync(product3));
				inv = new Invoice();
				inv.Number = "123";
				inv.Items.Add(new InvoiceItem(product1, 1));
				inv.Items.Add(new InvoiceItem(product2, 1));
				await (s.SaveAsync(inv));
				await (s.FlushAsync());
			}
		}

		private async Task IdBagWithCompositeElementThatContainsAManyToOne_CleanUpAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Invoice"));
				await (s.DeleteAsync("from Product"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task IdBagWithCompositeElementThatContainsAManyToOne_InsertAsync()
		{
			await (IdBagWithCompositeElementThatContainsAManyToOne_SetupAsync());
			using (ISession s = OpenSession())
			{
				Invoice invLoaded = await (s.GetAsync<Invoice>(inv.Id));
				Assert.AreEqual(2, invLoaded.Items.Count, "Expected 2 things in the invoice");
				s.Clear();
			}

			await (IdBagWithCompositeElementThatContainsAManyToOne_CleanUpAsync());
		}

		[Test]
		public async Task IdBagWithCompositeElementThatContainsAManyToOne_UpdateAsync()
		{
			InvoiceItem itemToUpdate = null;
			await (IdBagWithCompositeElementThatContainsAManyToOne_SetupAsync());
			using (ISession s = OpenSession())
			{
				Invoice invToUpdate = await (s.GetAsync<Invoice>(inv.Id));
				itemToUpdate = ((InvoiceItem)invToUpdate.Items[0]); // update information of an element
				itemToUpdate.Quantity = 10m;
				invToUpdate.Items.Add(new InvoiceItem(product3, 1)); // update the idbag collection
				await (s.FlushAsync());
				s.Clear();
			}

			using (ISession s = OpenSession())
			{
				Invoice invLoaded = await (s.GetAsync<Invoice>(inv.Id));
				Assert.AreEqual(3, invLoaded.Items.Count, "The collection should have a new item");
				Assert.IsTrue(invLoaded.Items.Contains(itemToUpdate));
				s.Clear();
			}

			await (IdBagWithCompositeElementThatContainsAManyToOne_CleanUpAsync());
		}

		[Test]
		public async Task IdBagWithCompositeElementThatContainsAManyToOne_DeleteAsync()
		{
			await (IdBagWithCompositeElementThatContainsAManyToOne_SetupAsync());
			using (ISession s = OpenSession())
			{
				Invoice invToUpdate = await (s.GetAsync<Invoice>(inv.Id));
				invToUpdate.Items.RemoveAt(0);
				await (s.FlushAsync());
				s.Clear();
			}

			using (ISession s = OpenSession())
			{
				Invoice invLoaded = await (s.GetAsync<Invoice>(inv.Id));
				Assert.AreEqual(1, invLoaded.Items.Count, "The collection should only have one item");
				s.Clear();
			}

			await (IdBagWithCompositeElementThatContainsAManyToOne_CleanUpAsync());
		}
	}
}
#endif
