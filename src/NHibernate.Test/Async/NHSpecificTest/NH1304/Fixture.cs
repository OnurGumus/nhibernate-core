#if NET_4_5
using System.Text;
using log4net.Core;
using NHibernate.Cfg;
using NHibernate.Tuple.Entity;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1304
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void WhenNoCustomAccessorIsDefinedThenSholdFindOnlyNoCustom()
		{
			var cfg = new Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				cfg.Configure(TestConfigurationHelper.hibernateConfigFile);
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1304.Mappings.hbm.xml", GetType().Assembly);
			using (LogSpy ls = new LogSpy(typeof (AbstractEntityTuplizer)))
			{
				cfg.BuildSessionFactory();
				StringBuilder wholeMessage = new StringBuilder();
				foreach (LoggingEvent loggingEvent in ls.Appender.GetEvents())
				{
					string singleMessage = loggingEvent.RenderedMessage;
					if (singleMessage.IndexOf("AbstractEntityTuplizer") > 0)
						Assert.Greater(singleMessage.IndexOf("No custom"), -1);
					wholeMessage.Append(singleMessage);
				}

				string logs = wholeMessage.ToString();
				Assert.AreEqual(-1, logs.IndexOf("Custom"));
			}
		}
	}
}
#endif
