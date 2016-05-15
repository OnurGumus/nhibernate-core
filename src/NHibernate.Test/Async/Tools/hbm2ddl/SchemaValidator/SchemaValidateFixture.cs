#if NET_4_5
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Tools.hbm2ddl.SchemaValidator
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SchemaValidateFixture
	{
		[Test]
		public async Task ShouldVerifySameTableAsync()
		{
			const string resource = "NHibernate.Test.Tools.hbm2ddl.SchemaValidator.1_Version.hbm.xml";
			var cfg = BuildConfiguration(resource);
			await (new SchemaExport(cfg).ExecuteAsync(true, true, false));
			var validator = new Tool.hbm2ddl.SchemaValidator((cfg));
			await (validator.ValidateAsync());
		}

		[Test, SetCulture("tr-TR"), SetUICulture("tr-TR")]
		public async Task ShouldVerifySameTableTurkishAsync()
		{
			//NH-3063
			// Turkish have unusual casing rules for the letter 'i'. This test verifies that
			// code paths executed by the SchemaValidator correctly handles case insensitive
			// comparisons for this.
			// Just make sure that we have an int property in the mapped class. This is
			// the 'i' we rely on for the test.
			var v = new Version();
			Assert.That(v.Id, Is.TypeOf<int>());
			const string resource = "NHibernate.Test.Tools.hbm2ddl.SchemaValidator.1_Version.hbm.xml";
			var cfg = BuildConfiguration(resource);
			await (new SchemaExport(cfg).ExecuteAsync(true, true, false));
			var validator = new Tool.hbm2ddl.SchemaValidator(cfg);
			await (validator.ValidateAsync());
		}

		[Test]
		public async Task ShouldNotVerifyModifiedTableAsync()
		{
			const string resource1 = "NHibernate.Test.Tools.hbm2ddl.SchemaValidator.1_Version.hbm.xml";
			var cfgV1 = BuildConfiguration(resource1);
			const string resource2 = "NHibernate.Test.Tools.hbm2ddl.SchemaValidator.2_Version.hbm.xml";
			var cfgV2 = BuildConfiguration(resource2);
			await (new SchemaExport(cfgV1).ExecuteAsync(true, true, false));
			var validatorV2 = new Tool.hbm2ddl.SchemaValidator(cfgV2);
			try
			{
				await (validatorV2.ValidateAsync());
			}
			catch (HibernateException e)
			{
				Assert.That(e.Message, Is.StringStarting("Missing column: Name"));
			}
		}
	}
}
#endif
