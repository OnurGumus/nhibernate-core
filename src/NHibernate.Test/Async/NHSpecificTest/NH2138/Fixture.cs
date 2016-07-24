#if NET_4_5
using NHibernate.Engine.Query.Sql;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2138
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void AfterAddAppingShouldHaveAResultsetWithEntityName()
		{
			var cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH2138.Mappings.hbm.xml", GetType().Assembly);
			Assert.That(() => cfg.BuildMappings(), Throws.Nothing);
			var sqlQuery = cfg.NamedSQLQueries["AllCoders"];
			var rootReturn = (NativeSQLQueryRootReturn)sqlQuery.QueryReturns[0];
			Assert.That(rootReturn.ReturnEntityName, Is.EqualTo("Coder"));
		}
	}
}
#endif
