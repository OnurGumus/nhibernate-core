#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2167
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is FirebirdDialect);
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally"};
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task GroupPropertyWithSqlFunctionDoesNotThrowAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var projection = session.CreateCriteria<Entity>().SetProjection(Projections.GroupProperty(Projections.SqlFunction("substring", NHibernateUtil.String, Projections.Property("Name"), Projections.Constant(0), Projections.Constant(1))));
					await (projection.ListAsync());
				}
		}
	}
}
#endif
