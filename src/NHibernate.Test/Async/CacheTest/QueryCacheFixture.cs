#if NET_4_5
using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.DomainModel;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.CacheTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryCacheFixture : TestCase
	{
		[Test]
		public async Task QueryCacheWithNullParametersAsync()
		{
			Simple simple = new Simple();
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(simple, 1L));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				await ((await (s.CreateQuery("from Simple s where s = :s or s.Name = :name or s.Address = :address").SetEntityAsync("s", await (s.LoadAsync(typeof (Simple), 1L))))).SetString("name", null).SetString("address", null).SetCacheable(true).UniqueResultAsync());
				// Run a second time, just to test the query cache
				object result = await ((await (s.CreateQuery("from Simple s where s = :s or s.Name = :name or s.Address = :address").SetEntityAsync("s", await (s.LoadAsync(typeof (Simple), 1L))))).SetString("name", null).SetString("address", null).SetCacheable(true).UniqueResultAsync());
				Assert.IsNotNull(result);
				Assert.AreEqual(1L, (long)s.GetIdentifier(result));
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Simple"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
