#if NET_4_5
using System;
using System.Collections;
using System.Threading;
using NHibernate.Stat;
using NUnit.Framework;
using NHibernate.Transform;
using System.Threading.Tasks;

namespace NHibernate.Test.SecondLevelCacheTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ScalarQueryFixture : TestCase
	{
		public async Task FillDbAsync(int startId)
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					for (int i = startId; i < startId + 10; i++)
					{
						await (s.SaveAsync(new AnotherItem{Id = i, Name = (i / 2).ToString()}));
					}

					await (tx.CommitAsync());
				}
		}

		public async Task CleanUpAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from AnotherItem"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task ShouldHitCacheUsingNamedQueryWithProjectionAsync()
		{
			await (FillDbAsync(1));
			sessions.Statistics.Clear();
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.GetNamedQuery("Stat").ListAsync());
					await (tx.CommitAsync());
				}

			Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
			Assert.That(sessions.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(0));
			sessions.Statistics.Clear();
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.GetNamedQuery("Stat").ListAsync());
					await (tx.CommitAsync());
				}

			Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(0));
			Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(1));
			sessions.Statistics.LogSummary();
			await (CleanUpAsync());
		}

		[Test]
		public async Task ShouldHitCacheUsingQueryWithProjectionAsync()
		{
			await (FillDbAsync(1));
			sessions.Statistics.Clear();
			int resultCount;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					resultCount = (await (s.CreateQuery("select ai.Name, count(*) from AnotherItem ai group by ai.Name").SetCacheable(true).SetCacheRegion("Statistics").ListAsync())).Count;
					await (tx.CommitAsync());
				}

			Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
			Assert.That(sessions.Statistics.QueryCachePutCount, Is.EqualTo(1));
			Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(0));
			sessions.Statistics.Clear();
			int secondResultCount;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					secondResultCount = (await (s.CreateQuery("select ai.Name, count(*) from AnotherItem ai group by ai.Name").SetCacheable(true).SetCacheRegion("Statistics").ListAsync())).Count;
					await (tx.CommitAsync());
				}

			Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(0));
			Assert.That(sessions.Statistics.QueryCacheHitCount, Is.EqualTo(1));
			Assert.That(secondResultCount, Is.EqualTo(resultCount));
			sessions.Statistics.LogSummary();
			await (CleanUpAsync());
		}

		[Test]
		public async Task QueryCacheInvalidationAsync()
		{
			sessions.EvictQueries();
			sessions.Statistics.Clear();
			const string queryString = "from Item i where i.Name='widget'";
			object savedId = await (CreateItemAsync(queryString));
			QueryStatistics qs = sessions.Statistics.GetQueryStatistics(queryString);
			EntityStatistics es = sessions.Statistics.GetEntityStatistics(typeof (Item).FullName);
			Thread.Sleep(200);
			IList result;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					result = await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(0));
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					result = await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(1));
			Assert.That(es.FetchCount, Is.EqualTo(0));
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					result = await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					Assert.That(NHibernateUtil.IsInitialized(result[0]));
					var i = (Item)result[0];
					i.Name = "Widget";
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(2));
			Assert.That(qs.CacheMissCount, Is.EqualTo(2));
			Assert.That(es.FetchCount, Is.EqualTo(0));
			Thread.Sleep(200);
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					var i = await (s.GetAsync<Item>(savedId));
					Assert.That(i.Name, Is.EqualTo("Widget"));
					await (s.DeleteAsync(i));
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(2));
			Assert.That(qs.CacheMissCount, Is.EqualTo(3));
			Assert.That(qs.CachePutCount, Is.EqualTo(3));
			Assert.That(qs.ExecutionCount, Is.EqualTo(3));
			Assert.That(es.FetchCount, Is.EqualTo(0)); //check that it was being cached
		}

		private async Task<object> CreateItemAsync(string queryString)
		{
			object savedId;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					var i = new Item{Name = "widget"};
					savedId = await (s.SaveAsync(i));
					await (tx.CommitAsync());
				}

			return savedId;
		}

		[Test]
		public async Task SimpleProjectionsAsync()
		{
			var transformer = new CustomTransformer();
			sessions.EvictQueries();
			sessions.Statistics.Clear();
			const string queryString = "select i.Name, i.Description from AnotherItem i where i.Name='widget'";
			object savedId;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					var i = new AnotherItem{Name = "widget"};
					savedId = await (s.SaveAsync(i));
					await (tx.CommitAsync());
				}

			QueryStatistics qs = sessions.Statistics.GetQueryStatistics(queryString);
			EntityStatistics es = sessions.Statistics.GetEntityStatistics(typeof (AnotherItem).FullName);
			Thread.Sleep(200);
			IList result;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(0));
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(1));
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).SetResultTransformer(transformer).ListAsync());
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(2), "hit count should go up since the cache contains the result before the possible application of a resulttransformer");
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).SetResultTransformer(transformer).ListAsync());
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(3), "hit count should go up since we are using the same resulttransformer");
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					result = await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					Assert.That(result.Count, Is.EqualTo(1));
					var i = await (s.GetAsync<AnotherItem>(savedId));
					i.Name = "Widget";
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(4));
			Assert.That(qs.CacheMissCount, Is.EqualTo(2));
			Thread.Sleep(200);
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery(queryString).SetCacheable(true).ListAsync());
					var i = await (s.GetAsync<AnotherItem>(savedId));
					Assert.That(i.Name, Is.EqualTo("Widget"));
					await (s.DeleteAsync(i));
					await (tx.CommitAsync());
				}

			Assert.That(qs.CacheHitCount, Is.EqualTo(4));
			Assert.That(qs.CacheMissCount, Is.EqualTo(3));
			Assert.That(qs.CachePutCount, Is.EqualTo(3));
			Assert.That(qs.ExecutionCount, Is.EqualTo(3));
			Assert.That(es.FetchCount, Is.EqualTo(0)); //check that it was being cached
		}
	}
}
#endif
