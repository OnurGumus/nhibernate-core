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
		private static async Task DeleteObjectsOutsideCacheAsync(ISession s)
		{
			using (DbCommand cmd = s.Connection.CreateCommand())
			{
				cmd.CommandText = "DELETE FROM UserTable";
				await (cmd.ExecuteNonQueryAsync());
			}
		}

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
				User user = (await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValueAsync<User>())).Value;
				Assert.That(user, Is.Not.Null);
				await (DeleteObjectsOutsideCacheAsync(s));
			}

			using (ISession s = OpenSession())
			{
				User user = (await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValueAsync<User>())).Value;
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
				User user = (await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region1").FutureValueAsync<User>())).Value;
				Assert.That(user, Is.Not.Null);
				await (DeleteObjectsOutsideCacheAsync(s));
			}

			using (ISession s = OpenSession())
			{
				User user = (await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region2").FutureValueAsync<User>())).Value;
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
				IFutureValue<User> userFuture = await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValueAsync<User>());
				// non cacheable Future causes batch to be non-cacheable
				int count = (await (s.CreateCriteria<User>().SetProjection(Projections.RowCount()).FutureValueAsync<int>())).Value;
				Assert.That(userFuture.Value, Is.Not.Null);
				Assert.That(count, Is.EqualTo(1));
				await (DeleteObjectsOutsideCacheAsync(s));
			}

			using (ISession s = OpenSession())
			{
				IFutureValue<User> userFuture = await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).FutureValueAsync<User>());
				int count = (await (s.CreateCriteria<User>().SetProjection(Projections.RowCount()).FutureValueAsync<int>())).Value;
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
				IFutureValue<User> userFuture = await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region1").FutureValueAsync<User>());
				// different cache-region causes batch to be non-cacheable
				int count = (await (s.CreateCriteria<User>().SetProjection(Projections.RowCount()).SetCacheable(true).SetCacheRegion("region2").FutureValueAsync<int>())).Value;
				Assert.That(userFuture.Value, Is.Not.Null);
				Assert.That(count, Is.EqualTo(1));
				await (DeleteObjectsOutsideCacheAsync(s));
			}

			using (ISession s = OpenSession())
			{
				IFutureValue<User> userFuture = await (s.CreateCriteria<User>().Add(Restrictions.NaturalId().Set("Name", "test")).SetCacheable(true).SetCacheRegion("region1").FutureValueAsync<User>());
				int count = (await (s.CreateCriteria<User>().SetProjection(Projections.RowCount()).SetCacheable(true).SetCacheRegion("region2").FutureValueAsync<int>())).Value;
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
				User user = (await (s.CreateQuery("from User u where u.Name='test'").SetCacheable(true).FutureValueAsync<User>())).Value;
				Assert.That(user, Is.Not.Null);
				await (DeleteObjectsOutsideCacheAsync(s));
			}

			using (ISession s = OpenSession())
			{
				User user = (await (s.CreateQuery("from User u where u.Name='test'").SetCacheable(true).FutureValueAsync<User>())).Value;
				Assert.That(user, Is.Not.Null, "entity not retrieved from cache");
			}
		}
	}
}
#endif
