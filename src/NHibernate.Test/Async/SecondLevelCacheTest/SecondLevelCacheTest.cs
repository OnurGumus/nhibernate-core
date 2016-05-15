#if NET_4_5
using System.Data.Common;
using System.Collections;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SecondLevelCacheTests
{
	using Criterion;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SecondLevelCacheTest : TestCase
	{
		[Test]
		public async Task CachedQueriesHandlesEntitiesParametersCorrectlyAsync()
		{
			using (ISession session = OpenSession())
			{
				Item one = (Item)await (session.LoadAsync(typeof (Item), 1));
				IList results = await ((await (session.CreateQuery("from Item item where item.Parent = :parent").SetEntityAsync("parent", one))).SetCacheable(true).ListAsync());
				Assert.AreEqual(4, results.Count);
				foreach (Item item in results)
				{
					Assert.AreEqual(1, item.Parent.Id);
				}
			}

			using (ISession session = OpenSession())
			{
				Item two = (Item)await (session.LoadAsync(typeof (Item), 2));
				IList results = await ((await (session.CreateQuery("from Item item where item.Parent = :parent").SetEntityAsync("parent", two))).SetCacheable(true).ListAsync());
				Assert.AreEqual(0, results.Count);
			}
		}

		[Test]
		public async Task DeleteItemFromCollectionThatIsInTheSecondLevelCacheAsync()
		{
			using (ISession session = OpenSession())
			{
				Item item = (Item)await (session.LoadAsync(typeof (Item), 1));
				Assert.IsTrue(item.Children.Count == 4); // just force it into the second level cache here
			}

			int childId = -1;
			using (ISession session = OpenSession())
			{
				Item item = (Item)await (session.LoadAsync(typeof (Item), 1));
				Item child = (Item)item.Children[0];
				childId = child.Id;
				await (session.DeleteAsync(child));
				item.Children.Remove(child);
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				Item item = (Item)await (session.LoadAsync(typeof (Item), 1));
				Assert.AreEqual(3, item.Children.Count);
				foreach (Item child in item.Children)
				{
					await (NHibernateUtil.InitializeAsync(child));
					Assert.IsFalse(child.Id == childId);
				}
			}
		}

		[Test]
		public async Task InsertItemToCollectionOnTheSecondLevelCacheAsync()
		{
			using (ISession session = OpenSession())
			{
				Item item = (Item)await (session.LoadAsync(typeof (Item), 1));
				Item child = new Item();
				child.Id = 6;
				item.Children.Add(child);
				await (session.SaveAsync(child));
				await (session.FlushAsync());
			}

			using (ISession session = OpenSession())
			{
				Item item = (Item)await (session.LoadAsync(typeof (Item), 1));
				int count = item.Children.Count;
				Assert.AreEqual(5, count);
			}
		}

		[Test]
		public async Task SecondLevelCacheWithCriteriaQueriesAsync()
		{
			using (ISession session = OpenSession())
			{
				IList list = await (session.CreateCriteria(typeof (AnotherItem)).Add(Expression.Gt("Id", 2)).SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
				using (DbCommand cmd = session.Connection.CreateCommand())
				{
					cmd.CommandText = "DELETE FROM AnotherItem";
					await (cmd.ExecuteNonQueryAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				//should bring from cache
				IList list = await (session.CreateCriteria(typeof (AnotherItem)).Add(Expression.Gt("Id", 2)).SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
			}
		}

		[Test]
		public async Task SecondLevelCacheWithCriteriaQueriesForItemWithCollectionsAsync()
		{
			using (ISession session = OpenSession())
			{
				IList list = await (session.CreateCriteria(typeof (Item)).Add(Expression.Gt("Id", 2)).SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
				using (DbCommand cmd = session.Connection.CreateCommand())
				{
					cmd.CommandText = "DELETE FROM Item";
					await (cmd.ExecuteNonQueryAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				//should bring from cache
				IList list = await (session.CreateCriteria(typeof (Item)).Add(Expression.Gt("Id", 2)).SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
			}
		}

		[Test]
		public async Task SecondLevelCacheWithHqlQueriesForItemWithCollectionsAsync()
		{
			using (ISession session = OpenSession())
			{
				IList list = await (session.CreateQuery("from Item i where i.Id > 2").SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
				using (DbCommand cmd = session.Connection.CreateCommand())
				{
					cmd.CommandText = "DELETE FROM Item";
					await (cmd.ExecuteNonQueryAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				//should bring from cache
				IList list = await (session.CreateQuery("from Item i where i.Id > 2").SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
			}
		}

		[Test]
		public async Task SecondLevelCacheWithHqlQueriesAsync()
		{
			using (ISession session = OpenSession())
			{
				IList list = await (session.CreateQuery("from AnotherItem i where i.Id > 2").SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
				using (DbCommand cmd = session.Connection.CreateCommand())
				{
					cmd.CommandText = "DELETE FROM AnotherItem";
					await (cmd.ExecuteNonQueryAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				//should bring from cache
				IList list = await (session.CreateQuery("from AnotherItem i where i.Id > 2").SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
			}
		}
	}
}
#endif
