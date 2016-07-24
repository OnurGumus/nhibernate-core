#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2923
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ExtraLazyFixtureAsync : TestCaseMappingByCodeAsync
	{
		private object bobId;
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Parent>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.List(x => x.Children, m =>
				{
					m.Lazy(CollectionLazy.Extra);
					m.Cascade(Mapping.ByCode.Cascade.All | Mapping.ByCode.Cascade.DeleteOrphans);
					m.Inverse(true);
				}

				, relation => relation.OneToMany());
			}

			);
			mapper.Class<Child>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.ManyToOne(x => x.Parent);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Parent{Name = "Bob"};
					bobId = await (session.SaveAsync(e1));
					var e2 = new Parent{Name = "Sally"};
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
		public async Task ShouldNotThrowExceptionAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var bob = await (session.GetAsync<Parent>(bobId));
					int ? count = null;
					Assert.DoesNotThrow(() =>
					{
						count = bob.Children.Count;
					}

					);
					Assert.NotNull(count);
					Assert.That(count.Value, Is.EqualTo(0));
				}
		}
	}
}
#endif
