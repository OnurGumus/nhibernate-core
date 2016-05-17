#if NET_4_5
using System.Data.Common;
using NUnit.Framework;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1989
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SecondLevelCacheWithSingleCacheableFutureAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					User user = new User()
					{Name = "test"};
					await (s.SaveAsync(user));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				// Query results should be cached
				User user = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValue<User>().Value;
				Assert.That(user, Is.Not.Null);
				DeleteObjectsOutsideCache(s);
			}

			using (ISession s = OpenSession())
			{
				User user = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValue<User>().Value;
				Assert.That(user, Is.Not.Null, "entity not retrieved from cache");
			}
		}

		[Test]
		public async Task SecondLevelCacheWithDifferentRegionsFutureAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					User user = new User()
					{Name = "test"};
					await (s.SaveAsync(user));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				// Query results should be cached
				User user = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region1").FutureValue<User>().Value;
				Assert.That(user, Is.Not.Null);
				DeleteObjectsOutsideCache(s);
			}

			using (ISession s = OpenSession())
			{
				User user = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region2").FutureValue<User>().Value;
				Assert.That(user, Is.Null, "entity from different region should not be retrieved");
			}
		}

		[Test]
		public async Task SecondLevelCacheWithMixedCacheableAndNonCacheableFutureAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					User user = new User()
					{Name = "test"};
					await (s.SaveAsync(user));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				// cacheable Future, not evaluated yet
				IFutureValue<User> userFuture = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValue<User>();
				// non cacheable Future causes batch to be non-cacheable
				int count = s.CreateCriteria<User>().SetProjection(Projections.RowCount()).FutureValue<int>().Value;
				Assert.That(userFuture.Value, Is.Not.Null);
				Assert.That(count, Is.EqualTo(1));
				DeleteObjectsOutsideCache(s);
			}

			using (ISession s = OpenSession())
			{
				IFutureValue<User> userFuture = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValue<User>();
				int count = s.CreateCriteria<User>().SetProjection(Projections.RowCount()).FutureValue<int>().Value;
				Assert.That(userFuture.Value, Is.Null, "query results should not come from cache");
			}
		}

		[Test]
		public async Task SecondLevelCacheWithMixedCacheRegionsFutureAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					User user = new User()
					{Name = "test"};
					await (s.SaveAsync(user));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				// cacheable Future, not evaluated yet
				IFutureValue<User> userFuture = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region1").FutureValue<User>();
				// different cache-region causes batch to be non-cacheable
				int count = s.CreateCriteria<User>().SetProjection(Projections.RowCount()).SetCacheable(true).SetCacheRegion("region2").FutureValue<int>().Value;
				Assert.That(userFuture.Value, Is.Not.Null);
				Assert.That(count, Is.EqualTo(1));
				DeleteObjectsOutsideCache(s);
			}

			using (ISession s = OpenSession())
			{
				IFutureValue<User> userFuture = s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region1").FutureValue<User>();
				int count = s.CreateCriteria<User>().SetProjection(Projections.RowCount()).SetCacheable(true).SetCacheRegion("region2").FutureValue<int>().Value;
				Assert.That(userFuture.Value, Is.Null, "query results should not come from cache");
			}
		}

		[Test]
		public async Task SecondLevelCacheWithSingleCacheableQueryFutureAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					User user = new User()
					{Name = "test"};
					await (s.SaveAsync(user));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				// Query results should be cached
				User user = s.CreateQuery("from User u where u.Name='test'").SetCacheable(true).FutureValue<User>().Value;
				Assert.That(user, Is.Not.Null);
				DeleteObjectsOutsideCache(s);
			}

			using (ISession s = OpenSession())
			{
				User user = s.CreateQuery("from User u where u.Name='test'").SetCacheable(true).FutureValue<User>().Value;
				Assert.That(user, Is.Not.Null, "entity not retrieved from cache");
			}
		}
	}
}
#endif
