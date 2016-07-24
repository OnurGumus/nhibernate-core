#if NET_4_5
using System.Text;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1521
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private static void CheckDialect(Configuration configuration)
		{
			if (!configuration.Properties[Environment.Dialect].Contains("MsSql"))
				Assert.Ignore("Specific test for MsSQL dialects");
		}

		private static async Task AssertThatCheckOnTableExistenceIsCorrectAsync(Configuration configuration)
		{
			var su = new SchemaExport(configuration);
			var sb = new StringBuilder(500);
			await (su.ExecuteAsync(x => sb.AppendLine(x), false, false));
			string script = sb.ToString();
			Assert.That(script, Is.StringContaining("if exists (select * from dbo.sysobjects where id = object_id(N'nhibernate.dbo.Aclass') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"));
		}

		[Test]
		public async Task TestForClassWithDefaultSchemaAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			CheckDialect(cfg);
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithNothing.hbm.xml", GetType().Assembly);
			cfg.SetProperty(Environment.DefaultCatalog, "nhibernate");
			cfg.SetProperty(Environment.DefaultSchema, "dbo");
			await (AssertThatCheckOnTableExistenceIsCorrectAsync(cfg));
		}

		[Test]
		public async Task WithDefaultValuesInMappingAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			CheckDialect(cfg);
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithDefault.hbm.xml", GetType().Assembly);
			await (AssertThatCheckOnTableExistenceIsCorrectAsync(cfg));
		}

		[Test]
		public async Task WithSpecificValuesInMappingAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			CheckDialect(cfg);
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithSpecific.hbm.xml", GetType().Assembly);
			await (AssertThatCheckOnTableExistenceIsCorrectAsync(cfg));
		}

		[Test]
		public async Task WithDefaultValuesInConfigurationPriorityToMappingAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			CheckDialect(cfg);
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1521.AclassWithDefault.hbm.xml", GetType().Assembly);
			cfg.SetProperty(Environment.DefaultCatalog, "somethingDifferent");
			cfg.SetProperty(Environment.DefaultSchema, "somethingDifferent");
			await (AssertThatCheckOnTableExistenceIsCorrectAsync(cfg));
		}
	}
}
#endif
