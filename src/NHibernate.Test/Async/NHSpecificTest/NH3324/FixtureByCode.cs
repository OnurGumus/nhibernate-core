#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3324
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureByCodeAsync : TestCaseMappingByCodeAsync
	{
		[Test]
		public async Task LeftOuterJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					const string hql = "FROM Entity e LEFT OUTER JOIN FETCH e.Children";
					var query = session.CreateQuery(hql);
					var result = await (query.ListAsync()); // does work
					Assert.That(result, Has.Count.GreaterThan(0));
				}
		}

		[Test]
		public async Task LeftOuterJoinSetMaxResultsAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					const string hql = "FROM Entity e LEFT OUTER JOIN FETCH e.Children";
					var query = session.CreateQuery(hql);
					query.SetMaxResults(5);
					var result = await (query.ListAsync()); // does not work
					Assert.That(result, Has.Count.GreaterThan(0));
				}
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m =>
				{
					m.Generator(Generators.Identity);
					m.Length(4);
				}

				);
				rc.Property(x => x.Name);
				rc.Bag(x => x.Children, c =>
				{
					c.Key(k =>
					{
						k.Column("EntityId");
						k.NotNullable(false);
						k.ForeignKey("Id");
					}

					);
					c.Cascade(Mapping.ByCode.Cascade.All);
					c.Table("ChildEntity");
					c.Inverse(true);
				}

				, t => t.OneToMany());
			}

			);
			mapper.Class<ChildEntity>(rc =>
			{
				rc.Id(x => x.Id, m =>
				{
					m.Generator(Generators.Identity);
					m.Length(4);
				}

				);
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
					e1.Children.Add(new ChildEntity{Name = "Bob's Child"});
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally"};
					e2.Children.Add(new ChildEntity{Name = "Sally's Child"});
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
	}
}
#endif
