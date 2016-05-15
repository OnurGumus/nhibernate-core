#if NET_4_5
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH952
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH952Fixture
	{
		[Test]
		public async Task OrderingAddResourcesAsync()
		{
			Configuration cfg = new Configuration();
			foreach (string res in Resources)
			{
				cfg.AddResource(res, MyAssembly);
			}

			await (cfg.BuildSessionFactory().CloseAsync());
		}
	}
}
#endif
