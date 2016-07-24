#if NET_4_5
using System.IO;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Tools.hbm2ddl.SchemaExportTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ExportToFileFixture
	{
		[Test]
		public async Task ExportToFileUsingSetOutputFileAndCreateAsync()
		{
			var configuration = TestConfigurationHelper.GetDefaultConfiguration();
			configuration.AddResource("NHibernate.Test.Tools.hbm2ddl.SchemaMetadataUpdaterTest.HeavyEntity.hbm.xml", GetType().Assembly);
			var outputFileName = Path.GetTempFileName();
			var export = new SchemaExport(configuration);
			export.SetOutputFile(outputFileName);
			await (export.CreateAsync(false, false));
			Assert.IsTrue(File.Exists(outputFileName));
			Assert.IsTrue(new FileInfo(outputFileName).Length > 0);
		}

		[Test]
		public async Task ExportToFileUsingExecuteAsync()
		{
			var configuration = TestConfigurationHelper.GetDefaultConfiguration();
			configuration.AddResource("NHibernate.Test.Tools.hbm2ddl.SchemaMetadataUpdaterTest.HeavyEntity.hbm.xml", GetType().Assembly);
			var outputFileName = Path.GetTempFileName();
			var export = new SchemaExport(configuration);
			await (export.ExecuteAsync(null, false, false, new StreamWriter(outputFileName)));
			Assert.IsTrue(File.Exists(outputFileName));
			Assert.IsTrue(new FileInfo(outputFileName).Length > 0);
		}
	}
}
#endif
