#if NET_4_5
using System.IO;
using System.Reflection;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Tools.hbm2ddl.SchemaExportTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class WithColumnTagFixture
	{
		[Test]
		public async Task ShouldCreateSchemaWithDefaultClauseAsync()
		{
			var script = new StringBuilder();
			const string mapping = "NHibernate.Test.Tools.hbm2ddl.SchemaExportTests.WithColumnTag.hbm.xml";
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(mapping))
				cfg.AddInputStream(stream);
			await (new SchemaExport(cfg).ExecuteAsync(s => script.AppendLine(s), false, false));
			string wholeScript = script.ToString();
			Assert.That(wholeScript, Is.StringContaining("default SYSTEM_USER"));
			Assert.That(wholeScript, Is.StringContaining("default 77"));
		}
	}
}
#endif
