#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH369
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[Test]
		public async Task KeyManyToOneAndNormalizedPersisterAsync()
		{
			Configuration cfg = new Configuration();
			await (cfg.AddClass(typeof (BaseClass)).AddClass(typeof (KeyManyToOneClass)).BuildSessionFactory().CloseAsync());
		}
	}
}
#endif
