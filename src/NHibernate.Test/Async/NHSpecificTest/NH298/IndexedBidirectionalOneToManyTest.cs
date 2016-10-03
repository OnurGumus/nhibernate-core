#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH298
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IndexedBidirectionalOneToManyTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = this.OpenSession())
			{
				Category root = new Category(1, "Root", null);
				root.SubCategories.Add(new Category(2, "First", root));
				root.SubCategories.Add(new Category(3, "Second", root));
				root.SubCategories.Add(new Category(4, "Third", root));
				await (session.SaveAsync(root));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = this.OpenSession())
			{
				await (session.DeleteAsync("from System.Object"));
				//session.CreateSQLQuery( "delete from Category" ).List();
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task SubItemMovesCorrectlyAsync()
		{
			Category root1 = null, itemToMove = null;
			using (ISession session = this.OpenSession())
			{
				root1 = await (session.GetAsync<Category>(1));
				itemToMove = root1.SubCategories[1]; //get the middle item
				root1.SubCategories.Remove(itemToMove); //remove the middle item
				root1.SubCategories.Add(itemToMove); //re-add it to the end
				await (session.UpdateAsync(root1));
				await (session.FlushAsync());
			}

			using (ISession session = this.OpenSession())
			{
				Category root2 = await (session.GetAsync<Category>(1));
				Assert.AreEqual(root1.SubCategories.Count, root2.SubCategories.Count);
				Assert.AreEqual(root1.SubCategories[1].Id, root2.SubCategories[1].Id);
				Assert.AreEqual(root1.SubCategories[2].Id, root2.SubCategories[2].Id);
				Assert.AreEqual(itemToMove.Id, root1.SubCategories[2].Id);
			}
		}

		[Test]
		public async Task RemoveAtWorksCorrectlyAsync()
		{
			Category root1 = null;
			using (ISession session = this.OpenSession())
			{
				root1 = await (session.GetAsync<Category>(1));
				root1.SubCategories.RemoveAt(1);
				await (session.UpdateAsync(root1));
				await (session.FlushAsync());
			}

			using (ISession session = this.OpenSession())
			{
				Category root2 = await (session.GetAsync<Category>(1));
				Assert.AreEqual(root1.SubCategories.Count, root2.SubCategories.Count);
			}
		}
	}
}
#endif
