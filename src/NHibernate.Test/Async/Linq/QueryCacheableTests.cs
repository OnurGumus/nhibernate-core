#if NET_4_5
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryCacheableTestsAsync : LinqTestCaseAsync
	{
		protected override void Configure(Configuration cfg)
		{
			cfg.SetProperty(Environment.UseQueryCache, "true");
			cfg.SetProperty(Environment.GenerateStatistics, "true");
			base.Configure(cfg);
		}

		[Test]
		public async Task QueryIsCacheableAsync()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			var x = await ((
				from c in db.Customers
				select c).Cacheable().ToListAsync());
			var x2 = await ((
				from c in db.Customers
				select c).Cacheable().ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1));
		}

		[Test]
		public async Task QueryIsCacheable2Async()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			var x = await ((
				from c in db.Customers
				select c).Cacheable().ToListAsync());
			var x2 = await ((
				from c in db.Customers
				select c).ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(0));
		}

		[Test]
		public async Task QueryIsCacheable3Async()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			var x = await ((
				from c in db.Customers.Cacheable()select c).ToListAsync());
			var x2 = await ((
				from c in db.Customers
				select c).ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(0));
		}

		[Test]
		public async Task QueryIsCacheableWithRegionAsync()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			var x = await ((
				from c in db.Customers
				select c).Cacheable().CacheRegion("test").ToListAsync());
			var x2 = await ((
				from c in db.Customers
				select c).Cacheable().CacheRegion("test").ToListAsync());
			var x3 = await ((
				from c in db.Customers
				select c).Cacheable().CacheRegion("other").ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1));
		}

		[Test]
		public async Task CacheableBeforeOtherClausesAsync()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			await (db.Customers.Cacheable().Where(c => c.ContactName != c.CompanyName).Take(1).ToListAsync());
			await (db.Customers.Where(c => c.ContactName != c.CompanyName).Take(1).ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(0));
		}

		[Test]
		public async Task CacheableRegionBeforeOtherClausesAsync()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			await (db.Customers.Cacheable().CacheRegion("test").Where(c => c.ContactName != c.CompanyName).Take(1).ToListAsync());
			await (db.Customers.Cacheable().CacheRegion("test").Where(c => c.ContactName != c.CompanyName).Take(1).ToListAsync());
			await (db.Customers.Cacheable().CacheRegion("other").Where(c => c.ContactName != c.CompanyName).Take(1).ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1));
		}

		[Test]
		public async Task GroupByQueryIsCacheableAsync()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			var c = await (db.Customers.GroupBy(x => x.Address.Country).Select(x => x.Key).Cacheable().ToListAsync());
			c = await (db.Customers.GroupBy(x => x.Address.Country).Select(x => x.Key).ToListAsync());
			c = await (db.Customers.GroupBy(x => x.Address.Country).Select(x => x.Key).Cacheable().ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1));
		}

		[Test]
		public async Task GroupByQueryIsCacheable2Async()
		{
			Sfi.Statistics.Clear();
			Sfi.QueryCache.Clear();
			var c = await (db.Customers.Cacheable().GroupBy(x => x.Address.Country).Select(x => x.Key).ToListAsync());
			c = await (db.Customers.GroupBy(x => x.Address.Country).Select(x => x.Key).ToListAsync());
			c = await (db.Customers.Cacheable().GroupBy(x => x.Address.Country).Select(x => x.Key).ToListAsync());
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2));
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1));
		}
	}
}
#endif
