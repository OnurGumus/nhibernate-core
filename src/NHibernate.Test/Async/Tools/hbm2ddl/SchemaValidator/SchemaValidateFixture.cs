﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Test.Tools.hbm2ddl.SchemaValidator
{
	using System.Threading.Tasks;
	[TestFixture]
	public class SchemaValidateFixtureAsync
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
				Assert.That(e.Message, Does.StartWith("Missing column: Name"));
			}
		}

		private static Configuration BuildConfiguration(string resource)
		{
			var cfg = TestConfigurationHelper.GetDefaultConfiguration();
			using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
				cfg.AddInputStream(stream);
			return cfg;
		}
	}
}
