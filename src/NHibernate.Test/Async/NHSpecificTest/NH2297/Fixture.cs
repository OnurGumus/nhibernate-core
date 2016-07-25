#if NET_4_5
using System;
using System.Collections;
using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2297
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[TestCase(".MappingsNames.hbm.xml")]
		public void InvalidCustomCompositeUserTypeThrowsMeaningfulException(string mappingFile)
		{
			var ex = Assert.Throws<InvalidOperationException>(() =>
			{
				var cfg = new Configuration();
				if (TestConfigurationHelper.hibernateConfigFile != null)
					cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
				const string MappingsAssembly = "NHibernate.Test";
				Assembly assembly = Assembly.Load(MappingsAssembly);
				string ns = GetType().Namespace;
				string bugNumber = ns.Substring(ns.LastIndexOf('.') + 1);
				cfg.AddResource(MappingsAssembly + "." + "NHSpecificTest." + bugNumber + mappingFile, assembly);
				// build session factory creates the invalid custom type mapper, and throws the exception
				cfg.BuildSessionFactory();
			});
			Assert.That(ex.Message, Is.EqualTo("ICompositeUserType NHibernate.Test.NHSpecificTest.NH2297.InvalidNamesCustomCompositeUserType returned a null value for 'PropertyNames'."));
		}
	}
}
#endif
