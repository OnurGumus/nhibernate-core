#if NET_4_5
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1443
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		private static async Task BugAsync(Configuration cfg)
		{
			var su = new SchemaExport(cfg);
			var sb = new StringBuilder(500);
			await (su.ExecuteAsync(x => sb.AppendLine(x), false, false));
			string script = sb.ToString();
			if (Dialect.Dialect.GetDialect(cfg.Properties).SupportsIfExistsBeforeTableName)
				Assert.That(script, Is.StringMatching("drop table if exists nhibernate.dbo.Aclass"));
			else
				Assert.That(script, Is.StringMatching("drop table nhibernate.dbo.Aclass"));
			Assert.That(script, Is.StringMatching("create table nhibernate.dbo.Aclass"));
		}

		[Test]
		public async Task WithDefaultValuesInConfigurationAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1443.AclassWithNothing.hbm.xml", GetType().Assembly);
			cfg.SetProperty(Environment.DefaultCatalog, "nhibernate");
			cfg.SetProperty(Environment.DefaultSchema, "dbo");
			await (BugAsync(cfg));
		}

		[Test]
		public async Task WithDefaultValuesInMappingAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1443.AclassWithDefault.hbm.xml", GetType().Assembly);
			await (BugAsync(cfg));
		}

		[Test]
		public async Task WithSpecificValuesInMappingAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1443.AclassWithSpecific.hbm.xml", GetType().Assembly);
			await (BugAsync(cfg));
		}

		[Test]
		public async Task WithDefaultValuesInConfigurationPriorityToMappingAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1443.AclassWithDefault.hbm.xml", GetType().Assembly);
			cfg.SetProperty(Environment.DefaultCatalog, "somethingDifferent");
			cfg.SetProperty(Environment.DefaultSchema, "somethingDifferent");
			await (BugAsync(cfg));
		}
	}
}
#endif
