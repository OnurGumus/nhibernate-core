#if NET_4_5
using System.IO;
using System.Reflection;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2003
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public async Task ShouldCreateNotNullIdColumnAsync()
		{
			StringBuilder script = new StringBuilder();
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			string ns = GetType().Namespace;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ns + ".Mappings.hbm.xml"))
				cfg.AddInputStream(stream);
			await (new SchemaExport(cfg).ExecuteAsync(s => script.AppendLine(s), false, false));
			string wholeScript = script.ToString();
			Assert.That(wholeScript, Is.StringContaining("not null").IgnoreCase);
		}
	}
}
#endif
