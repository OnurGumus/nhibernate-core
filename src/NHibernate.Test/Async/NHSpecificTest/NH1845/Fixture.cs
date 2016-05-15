#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1845
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task LazyLoad_Initialize_AndEvictAsync()
		{
			Category category = new Category("parent");
			category.AddSubcategory(new Category("child"));
			await (SaveCategoryAsync(category));
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					Category loaded = await (session.LoadAsync<Category>(category.Id));
					await (NHibernateUtil.InitializeAsync(loaded.Subcategories[0]));
					await (session.EvictAsync(loaded));
					await (transaction.CommitAsync());
					Assert.AreEqual("child", loaded.Subcategories[0].Name, "cannot access child");
				}

			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					// first delete children
					await (session.CreateQuery("delete from Category where Parent != null").ExecuteUpdateAsync());
					// then the rest
					await (session.CreateQuery("delete from Category").ExecuteUpdateAsync());
					await (transaction.CommitAsync());
				}
		}

		private async Task SaveCategoryAsync(Category category)
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.SaveOrUpdateAsync(category));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
