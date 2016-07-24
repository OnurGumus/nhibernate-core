#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3124
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Person>(ca =>
			{
				ca.Id(x => x.Id, map => map.Generator(Generators.Assigned));
				ca.Property(x => x.Name, map => map.Length(150));
				ca.Property(x => x.Type, map => map.Length(1));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Id = 1000, Name = "Test", Type = 'A'}));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task LinqStatementGeneratesIncorrectCastToIntegerAsync()
		{
			using (ISession session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					session.Query<Person>().Where(x => x.Type == 'A').ToList();
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
