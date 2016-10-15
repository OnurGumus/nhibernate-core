#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3414
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public class ByCodeFixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			// SQL Server CE does not appear to support subqueries in the ORDER BY clause.
			return !(dialect is MsSqlCeDialect);
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.Property(x => x.SomeValue);
				rc.ManyToOne(x => x.Parent, map => map.Column("ParentId"));
				rc.Bag(x => x.Children, map =>
				{
					map.Access(Accessor.NoSetter);
					map.Key(km => km.Column("ParentId"));
					map.Cascade(Mapping.ByCode.Cascade.All.Include(Mapping.ByCode.Cascade.DeleteOrphans));
				}

				, rel => rel.OneToMany());
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "A", SomeValue = 1, };
					e1.AddChild(new Entity{Name = "X", SomeValue = 3});
					e1.AddChild(new Entity{Name = "Z", SomeValue = 10});
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "B", SomeValue = 2, };
					e2.AddChild(new Entity{Name = "Y", SomeValue = 10});
					e2.AddChild(new Entity{Name = "Z", SomeValue = 2});
					await (session.SaveAsync(e2));
					var e3 = new Entity{Name = "A", SomeValue = 3, };
					e3.AddChild(new Entity{Name = "X", SomeValue = 9});
					e3.AddChild(new Entity{Name = "Y", SomeValue = 1});
					await (session.SaveAsync(e3));
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
	}
}
#endif
