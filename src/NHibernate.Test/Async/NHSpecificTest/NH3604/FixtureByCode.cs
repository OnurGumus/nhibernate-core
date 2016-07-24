#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3604
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(Entity.PropertyAccessExpressions.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.OneToOne(x => x.Detail, m => m.Cascade(Mapping.ByCode.Cascade.All));
			}

			);
			mapper.Class<EntityDetail>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(new ForeignGeneratorDef(ReflectionHelper.GetProperty(EntityDetail.PropertyAccessExpressions.Entity))));
				rc.OneToOne(EntityDetail.PropertyAccessExpressions.Entity, m => m.Constrained(true));
				rc.Property(x => x.ExtraInfo);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally"};
					var ed2 = new EntityDetail(e2)
					{ExtraInfo = "Jo"};
					e2.Detail = ed2;
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public void CanPerformQueryOnMappedClassWithProtectedProperty()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result =
						from e in session.Query<Entity>()where e.Name == "Sally"
						select e;
					var entities = result.ToList();
					Assert.AreEqual(1, entities.Count);
					Assert.AreEqual("Jo", entities[0].Detail.ExtraInfo);
				}
		}

		[Test]
		public void CanWriteMappingsReferencingProtectedProperty()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(Entity.PropertyAccessExpressions.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
			}

			);
			mapper.CompileMappingForEachExplicitlyAddedEntity().WriteAllXmlMapping();
		}
	}
}
#endif
