#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3316
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(e =>
			{
				e.Id(x => x.Id, id => id.Generator(Generators.GuidComb));
				e.Set(x => x.Children, c =>
				{
					c.Key(key => key.Column("EntityId"));
					c.Cascade(NHibernate.Mapping.ByCode.Cascade.All | NHibernate.Mapping.ByCode.Cascade.DeleteOrphans);
				}

				, r =>
				{
					r.Component(c =>
					{
						c.Parent(x => x.Parent);
						c.Property(x => x.Value, pm => pm.Column("`Value`"));
					}

					);
				}

				);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
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
		public async Task Test_That_Parent_Property_Can_Be_Persisted_And_RetrievedAsync()
		{
			Guid id = Guid.Empty;
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					Entity e = new Entity();
					e.AddChild(1);
					e.AddChild(2);
					await (session.SaveAsync(e));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
					id = e.Id;
				}

			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					Entity e = await (session.GetAsync<Entity>(id));
					Assert.AreEqual(2, e.Children.Count());
					foreach (ChildComponent c in e.Children)
						Assert.AreEqual(e, c.Parent);
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
