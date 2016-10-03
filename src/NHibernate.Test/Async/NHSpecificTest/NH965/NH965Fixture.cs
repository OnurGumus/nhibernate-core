#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH965
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH965FixtureAsync
	{
		[Test]
		public async Task BugAsync()
		{
			Configuration cfg = new Configuration();
			cfg.AddResource(GetType().Namespace + ".Mappings.hbm.xml", GetType().Assembly);
			await (cfg.BuildSessionFactory().CloseAsync());
		}
	}
}
#endif
