#if NET_4_5
using System.Text;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1710
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class BaseFixture
	{
		[Test]
		public async Task NotIgnorePrecisionScaleInSchemaExportAsync()
		{
			var script = new StringBuilder();
			await (new SchemaExport(cfg).CreateAsync(sl => script.AppendLine(sl), true));
			Assert.That(script.ToString(), Is.StringContaining(expectedExportString));
			await (new SchemaExport(cfg).DropAsync(false, true));
		}
	}
}
#endif
