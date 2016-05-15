#if NET_4_5
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1593
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[Test]
		public async Task SchemaUpdateAddsIndexesThatWerentPresentYetAsync()
		{
			Configuration cfg = TestConfigurationHelper.GetDefaultConfiguration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1593.TestIndex.hbm.xml", GetType().Assembly);
			var su = new SchemaUpdate(cfg);
			var sb = new StringBuilder(500);
			await (su.ExecuteAsync(x => sb.AppendLine(x), false));
			Assert.That(sb.ToString(), Is.StringContaining("create index test_index_name on TestIndex (Name)"));
		}
	}
}
#endif
