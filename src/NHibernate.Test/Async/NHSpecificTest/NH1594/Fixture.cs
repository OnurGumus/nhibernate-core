#if NET_4_5
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1594
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void Bug()
		{
			Configuration cfg = new Configuration();
			Assembly assembly = Assembly.GetExecutingAssembly();
			cfg.AddResource("NHibernate.Test.NHSpecificTest.NH1594.Mappings.hbm.xml", assembly);
			string[] script = cfg.GenerateSchemaCreationScript(new MsSql2000Dialect());
			bool found = string.Compare(script[0], "create table A (id INT IDENTITY NOT NULL, Foo DECIMAL(4, 2) null, primary key (id))", true) == 0;
			Assert.IsTrue(found, "when using decimal(precision,scale) Script should contain the correct create table statement");
		}
	}
}
#endif
