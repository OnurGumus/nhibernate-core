#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1845
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Category>(rc =>
			{
				rc.Id(x => x.Id, map => map.Generator(Generators.Native));
				rc.Property(x => x.Name);
				rc.ManyToOne(x => x.Parent, map => map.Column("ParentId"));
				rc.Bag(x => x.Subcategories, map =>
				{
					map.Access(Accessor.NoSetter);
					map.Key(km => km.Column("ParentId"));
					map.Cascade(Mapping.ByCode.Cascade.All.Include(Mapping.ByCode.Cascade.DeleteOrphans));
				}

				, rel => rel.OneToMany());
			}

			);
			var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
			return mappings;
		}

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
