#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1255
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public Task CanLoadMappingWithNotNullIgnoreAsync()
		{
			try
			{
				var cfg = new Configuration();
				if (TestConfigurationHelper.hibernateConfigFile != null)
					cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
				Assert.DoesNotThrow(() => cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1255.Mappings.hbm.xml", typeof (Customer).Assembly));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
