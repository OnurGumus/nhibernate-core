#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH873
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture
	{
		[Test]
		public async Task CacheDisabledAsync()
		{
			Configuration cfg = new Configuration();
			cfg.SetProperty(Environment.UseSecondLevelCache, "false");
			cfg.SetProperty(Environment.UseQueryCache, "false");
			cfg.SetProperty(Environment.CacheProvider, null);
			await (cfg.BuildSessionFactory().CloseAsync());
		}
	}
}
#endif
