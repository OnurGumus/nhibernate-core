#if NET_4_5
using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1605
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void SupportTypedefInReturnScalarElements()
		{
			var cfg = new Configuration();
			Assembly assembly = Assembly.GetExecutingAssembly();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1605.Mappings.hbm.xml", assembly);
			using (cfg.BuildSessionFactory())
			{
			}
		}
	}
}
#endif
