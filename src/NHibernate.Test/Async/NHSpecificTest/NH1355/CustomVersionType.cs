#if NET_4_5
using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1355
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomVersionTypeAsync
	{
		[Test]
		public void Bug()
		{
			Configuration cfg = new Configuration();
			Assembly domain = typeof (Category).Assembly;
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1355.Category.hbm.xml", domain);
			try
			{
				cfg.BuildSessionFactory();
			}
			catch (MappingException)
			{
				Assert.Fail("Should not throw exception");
			}
		}

		[Test]
		public void BugSubTask()
		{
			Configuration cfg = new Configuration();
			Assembly domain = typeof (Category).Assembly;
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1355.CategoryTD.hbm.xml", domain);
			try
			{
				cfg.BuildSessionFactory();
			}
			catch (MappingException)
			{
				Assert.Fail("Should not throw exception");
			}
		}
	}
}
#endif
