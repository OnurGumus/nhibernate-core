#if NET_4_5
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH251
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomAccessFixtureAsync
	{
		[Test]
		public void ConfigurationIsOK()
		{
			Configuration cfg = new Configuration();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH251.CustomAccessDO.hbm.xml", Assembly.GetExecutingAssembly());
			ISessionFactoryImplementor factory = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
			cfg.GenerateSchemaCreationScript(factory.Dialect);
		}
	}
}
#endif
