#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2977
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			return new HbmMapping();
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Dialect.MsSql2000Dialect;
		}

		[Test]
		public async Task CanGetUniqueStoredProcedureResultAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.CreateSQLQuery("EXEC sp_stored_procedures ?").SetString(0, "sp_help").UniqueResultAsync());
					Assert.That(result, Is.Not.Null);
				}
		}

		[Test]
		public async Task CanLimitStoredProcedureResultsAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.CreateSQLQuery("EXEC sp_stored_procedures").SetMaxResults(5).ListAsync());
					Assert.That(result, Has.Count.EqualTo(5));
				}
		}
	}
}
#endif
