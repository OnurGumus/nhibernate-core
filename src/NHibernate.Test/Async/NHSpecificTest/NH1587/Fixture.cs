#if NET_4_5
using log4net.Config;
using log4net.Core;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1587
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void Bug()
		{
			XmlConfigurator.Configure();
			var cfg = new Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1587.Mappings.hbm.xml", GetType().Assembly);
			cfg.Configure();
			bool useOptimizer = false;
			using (var ls = new LogSpy("NHibernate.Tuple.Entity.PocoEntityTuplizer"))
			{
				cfg.BuildSessionFactory();
				foreach (LoggingEvent loggingEvent in ls.Appender.GetEvents())
				{
					if (((string)(loggingEvent.MessageObject)).StartsWith("Create Instantiator using optimizer"))
					{
						useOptimizer = true;
						break;
					}
				}
			}

			Assert.That(useOptimizer);
		}
	}
}
#endif
