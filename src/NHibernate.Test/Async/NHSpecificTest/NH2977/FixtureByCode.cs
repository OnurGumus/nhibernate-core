#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2977
{
	/// <summary>
	/// Fixture using 'by code' mappings
	/// </summary>
	/// <remarks>
	/// This fixture is identical to <see cref = "Fixture"/> except the <see cref = "Entity"/> mapping is performed 
	/// by code in the GetMappings method, and does not require the <c>Mappings.hbm.xml</c> file. Use this approach
	/// if you prefer.
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixture : TestCaseMappingByCode
	{
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
