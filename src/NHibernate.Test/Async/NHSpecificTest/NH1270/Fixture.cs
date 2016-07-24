#if NET_4_5
using System.Text;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1270
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[Test]
		public async Task WhenMapCustomFkNamesThenUseItAsync()
		{
			var conf = TestConfigurationHelper.GetDefaultConfiguration();
			conf.DataBaseIntegration(i => i.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote);
			conf.AddMapping(GetMappings());
			var sb = new StringBuilder();
			await ((new SchemaExport(conf)).CreateAsync(s => sb.AppendLine(s), true));
			Assert.That(sb.ToString(), Is.StringContaining("FK_RoleInUser").And.StringContaining("FK_UserInRole"));
			await ((new SchemaExport(conf)).DropAsync(false, true));
		}
	}
}
#endif
