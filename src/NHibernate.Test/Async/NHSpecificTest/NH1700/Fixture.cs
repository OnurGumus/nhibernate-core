#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1700
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[Test]
		public async Task ShouldNotThrowDuplicateMappingAsync()
		{
			var cfg = new Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1700.Mappings.hbm.xml", GetType().Assembly);
			await (new SchemaExport(cfg).CreateAsync(false, true));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}
	}
}
#endif
