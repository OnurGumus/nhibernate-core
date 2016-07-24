#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Exceptions;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH3269
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureNonPublicPropertyAsync : TestCaseMappingByCodeAsync
	{
		[Test]
		public async Task ShouldThrowExceptionWhenTryingToSaveInherited1WithDuplicateNameAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Inherited1{Name = "Bob"};
					await (session.SaveAsync(e1));

					Assert.ThrowsAsync<GenericADOException>(
						async () =>
						{
							await (transaction.CommitAsync());
						});
				}
		}

		[Test]
		public async Task ShouldNotThrowExceptionWhenTryingToSaveInherited2WithDuplicateNameAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e2 = new Inherited2{Name = "Sally"};
					await (session.SaveAsync(e2));
					await (transaction.CommitAsync());
				}
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Inherited1>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.Guid));
				rc.Property("Name", m => m.UniqueKey("Inherited1_UX_Name"));
			}

			);
			mapper.Class<Inherited2>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.Guid));
				rc.Property("Name", m => m.Length(200));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Inherited1{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new Inherited2{Name = "Sally"};
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
