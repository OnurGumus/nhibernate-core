#if NET_4_5
using System;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH283
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void ForeignKeyNames()
		{
			Configuration cfg = new Configuration();
			Assembly assembly = Assembly.GetExecutingAssembly();
			cfg.AddResource("NHibernate.DomainModel.MasterDetail.hbm.xml", Assembly.GetAssembly(typeof (Master)));
			string script = string.Join("\n", cfg.GenerateSchemaCreationScript(new MsSql2000Dialect()));
			Assert.IsTrue(script.IndexOf("add constraint AA") >= 0);
			Assert.IsTrue(script.IndexOf("add constraint BB") >= 0);
			Assert.IsTrue(script.IndexOf("add constraint CC") >= 0);
		}
	}
}
#endif
