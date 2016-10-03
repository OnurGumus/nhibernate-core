#if NET_4_5
using System.IO;
using System.Reflection;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2243
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[Test]
		public async Task ShouldCreateSchemaWithDefaultClauseAsync()
		{
			var script = new StringBuilder();
			const string mapping = "NHibernate.Test.NHSpecificTest.NH2243.Mappings.hbm.xml";
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(mapping))
				cfg.AddInputStream(stream);
			await (new SchemaExport(cfg).ExecuteAsync(s => script.AppendLine(s), false, false));
			Assert.That(script.ToString(), Is.StringContaining("MyNameForFK"));
		}
	}
}
#endif
