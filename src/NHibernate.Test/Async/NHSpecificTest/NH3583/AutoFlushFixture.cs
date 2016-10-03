#if NET_4_5
using System.Linq;
using System.Transactions;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3583
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AutoFlushFixtureAsync : TestCaseMappingByCodeAsync
	{
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
		public async Task ShouldAutoFlushWhenInExplicitTransactionAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var result = (
						from e in session.Query<Entity>()where e.Name == "Bob"
						select e).ToList();
					Assert.That(result.Count, Is.EqualTo(1));
				}
		}

		[Test]
		public async Task ShouldAutoFlushWhenInDistributedTransactionAsync()
		{
			using (new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				using (var session = OpenSession())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var result = (
						from e in session.Query<Entity>()where e.Name == "Bob"
						select e).ToList();
					Assert.That(result.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
