#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1549
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		/// <summary>
		/// Verifies that an entity with a base class containing the id property 
		/// can have the id accessed without loading the entity
		/// </summary>
		[Test]
		public async Task CanLoadForEntitiesWithInheritedIdsAsync()
		{
			//create some related products
			var category = new CategoryWithInheritedId{Name = "Fruit"};
			var product = new ProductWithInheritedId{CategoryWithInheritedId = category};
			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.SaveAsync(category));
					await (session.SaveAsync(product));
					await (trans.CommitAsync());
				}
			}

			ProductWithInheritedId restoredProductWithInheritedId;
			//restore the product from the db in another session so that 
			//the association is a proxy
			using (ISession session = OpenSession())
			{
				restoredProductWithInheritedId = await (session.GetAsync<ProductWithInheritedId>(product.Id));
			}

			//verify that the category is a proxy
			Assert.IsFalse(NHibernateUtil.IsInitialized(restoredProductWithInheritedId.CategoryWithInheritedId));
			//we should be able to access the id of the category outside of the session
			Assert.AreEqual(category.Id, restoredProductWithInheritedId.CategoryWithInheritedId.Id);
		}

		[Test]
		public async Task CanLoadForEntitiesWithTheirOwnIdsAsync()
		{
			//create some related products
			var category = new CategoryWithId{Name = "Fruit"};
			var product = new ProductWithId{CategoryWithId = category};
			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.SaveAsync(category));
					await (session.SaveAsync(product));
					await (trans.CommitAsync());
				}
			}

			ProductWithId restoredProductWithInheritedId;
			//restore the product from the db in another session so that 
			//the association is a proxy
			using (ISession session = OpenSession())
			{
				restoredProductWithInheritedId = await (session.GetAsync<ProductWithId>(product.Id));
			}

			//verify that the category is a proxy
			Assert.IsFalse(NHibernateUtil.IsInitialized(restoredProductWithInheritedId.CategoryWithId));
			//we should be able to access the id of the category outside of the session
			Assert.AreEqual(category.Id, restoredProductWithInheritedId.CategoryWithId.Id);
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.DeleteAsync("from ProductWithId"));
					await (session.DeleteAsync("from CategoryWithId"));
					await (session.DeleteAsync("from ProductWithInheritedId"));
					await (session.DeleteAsync("from CategoryWithInheritedId"));
					await (trans.CommitAsync());
				}
			}
		}
	}
}
#endif
