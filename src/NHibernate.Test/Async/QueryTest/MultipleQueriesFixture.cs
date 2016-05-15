#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Test.SecondLevelCacheTests;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.QueryTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultipleQueriesFixture : TestCase
	{
		[Test]
		public async Task NH_1085_WillIgnoreParametersIfDoesNotAppearInQueryAsync()
		{
			using (var s = sessions.OpenSession())
			{
				var multiQuery = s.CreateMultiQuery().Add("from Item i where i.Id in (:ids)").Add("from Item i where i.Id in (:ids2)").SetParameterList("ids", new[]{50}).SetParameterList("ids2", new[]{50});
				await (multiQuery.ListAsync());
			}
		}

		[Test]
		public Task NH_1085_WillGiveReasonableErrorIfBadParameterNameAsync()
		{
			try
			{
				NH_1085_WillGiveReasonableErrorIfBadParameterName();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task CanGetMultiQueryFromSecondLevelCacheAsync()
		{
			await (CreateItemsAsync());
			await (DoMutiQueryAndAssertAsync());
			var cacheHashtable = GetHashTableUsedAsQueryCache(sessions);
			var cachedListEntry = (IList)new ArrayList(cacheHashtable.Values)[0];
			var cachedQuery = (IList)cachedListEntry[1];
			var firstQueryResults = (IList)cachedQuery[0];
			firstQueryResults.Clear();
			firstQueryResults.Add(3);
			firstQueryResults.Add(4);
			var secondQueryResults = (IList)cachedQuery[1];
			secondQueryResults[0] = 2L;
			using (var s = sessions.OpenSession())
			{
				var multiQuery = s.CreateMultiQuery().Add(s.CreateQuery("from Item i where i.Id > ?").SetInt32(0, 50).SetFirstResult(10)).Add(s.CreateQuery("select count(*) from Item i where i.Id > ?").SetInt32(0, 50));
				multiQuery.SetCacheable(true);
				var results = await (multiQuery.ListAsync());
				var items = (IList)results[0];
				Assert.AreEqual(2, items.Count);
				var count = (long)((IList)results[1])[0];
				Assert.AreEqual(2L, count);
			}
		}

		[Test]
		public async Task CanSpecifyParameterOnMultiQueryWhenItIsNotUsedInAllQueriesAsync()
		{
			using (var s = OpenSession())
			{
				await (s.CreateMultiQuery().Add("from Item").Add("from Item i where i.Id = :id").SetParameter("id", 5).ListAsync());
			}
		}

		[Test]
		public async Task CanSpecifyParameterOnMultiQueryWhenItIsNotUsedInAllQueries_MoreThanOneParameterAsync()
		{
			using (var s = OpenSession())
			{
				await (s.CreateMultiQuery().Add("from Item").Add("from Item i where i.Id = :id or i.Id = :id2").Add("from Item i where i.Id = :id2").SetParameter("id", 5).SetInt32("id2", 5).ListAsync());
			}
		}

		[Test]
		public async Task TwoMultiQueriesWithDifferentPagingGetDifferentResultsWhenUsingCachedQueriesAsync()
		{
			await (CreateItemsAsync());
			using (var s = OpenSession())
			{
				var multiQuery = s.CreateMultiQuery().Add(s.CreateQuery("from Item i where i.Id > ?").SetInt32(0, 50).SetFirstResult(10)).Add(s.CreateQuery("select count(*) from Item i where i.Id > ?").SetInt32(0, 50));
				multiQuery.SetCacheable(true);
				var results = await (multiQuery.ListAsync());
				var items = (IList)results[0];
				Assert.AreEqual(89, items.Count);
				var count = (long)((IList)results[1])[0];
				Assert.AreEqual(99L, count);
			}

			using (var s = OpenSession())
			{
				var multiQuery = s.CreateMultiQuery().Add(s.CreateQuery("from Item i where i.Id > ?").SetInt32(0, 50).SetFirstResult(20)).Add(s.CreateQuery("select count(*) from Item i where i.Id > ?").SetInt32(0, 50));
				multiQuery.SetCacheable(true);
				var results = await (multiQuery.ListAsync());
				var items = (IList)results[0];
				Assert.AreEqual(79, items.Count, "Should have gotten different result here, because the paging is different");
				var count = (long)((IList)results[1])[0];
				Assert.AreEqual(99L, count);
			}
		}

		[Test]
		public async Task CanUseSecondLevelCacheWithPositionalParametersAsync()
		{
			var cacheHashtable = GetHashTableUsedAsQueryCache(sessions);
			cacheHashtable.Clear();
			await (CreateItemsAsync());
			await (DoMutiQueryAndAssertAsync());
			Assert.AreEqual(1, cacheHashtable.Count);
		}

		private async Task DoMutiQueryAndAssertAsync()
		{
			using (var s = OpenSession())
			{
				var multiQuery = s.CreateMultiQuery().Add(s.CreateQuery("from Item i where i.Id > ?").SetInt32(0, 50).SetFirstResult(10)).Add(s.CreateQuery("select count(*) from Item i where i.Id > ?").SetInt32(0, 50));
				multiQuery.SetCacheable(true);
				var results = await (multiQuery.ListAsync());
				var items = (IList)results[0];
				Assert.AreEqual(89, items.Count);
				var count = (long)((IList)results[1])[0];
				Assert.AreEqual(99L, count);
			}
		}

		private async Task CreateItemsAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					for (var i = 0; i < 150; i++)
					{
						var item = new Item();
						item.Id = i;
						await (s.SaveAsync(item));
					}

					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseWithParameterizedQueriesAndLimitAsync()
		{
			await (CreateItemsAsync());
			using (var s = OpenSession())
			{
				var getItems = s.CreateQuery("from Item i where i.Id > :id").SetFirstResult(10);
				var countItems = s.CreateQuery("select count(*) from Item i where i.Id > :id");
				var results = await (s.CreateMultiQuery().Add(getItems).Add(countItems).SetInt32("id", 50).ListAsync());
				var items = (IList)results[0];
				Assert.AreEqual(89, items.Count);
				var count = (long)((IList)results[1])[0];
				Assert.AreEqual(99L, count);
			}
		}

		[Test]
		public async Task CanUseSetParameterListAsync()
		{
			using (var s = OpenSession())
			{
				var item = new Item();
				item.Id = 1;
				await (s.SaveAsync(item));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var results = await (s.CreateMultiQuery().Add("from Item i where i.id in (:items)").Add("select count(*) from Item i where i.id in (:items)").SetParameterList("items", new[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16}).ListAsync());
				var items = (IList)results[0];
				var fromDb = (Item)items[0];
				Assert.AreEqual(1, fromDb.Id);
				var counts = (IList)results[1];
				var count = (long)counts[0];
				Assert.AreEqual(1L, count);
			}
		}

		[Test]
		public async Task CanExecuteMultiplyQueriesInSingleRoundTripAsync()
		{
			using (var s = OpenSession())
			{
				var item = new Item();
				item.Id = 1;
				await (s.SaveAsync(item));
				await (s.FlushAsync());
			}

			using (var s = OpenSession())
			{
				var getItems = s.CreateQuery("from Item");
				var countItems = s.CreateQuery("select count(*) from Item");
				var results = await (s.CreateMultiQuery().Add(getItems).Add(countItems).ListAsync());
				var items = (IList)results[0];
				var fromDb = (Item)items[0];
				Assert.AreEqual(1, fromDb.Id);
				var counts = (IList)results[1];
				var count = (long)counts[0];
				Assert.AreEqual(1L, count);
			}
		}

		[Test]
		public async Task CanAddIQueryWithKeyAndRetrieveResultsWithKeyAsync()
		{
			await (CreateItemsAsync());
			using (var session = OpenSession())
			{
				var multiQuery = session.CreateMultiQuery();
				var firstQuery = session.CreateQuery("from Item i where i.Id < :id").SetInt32("id", 50);
				var secondQuery = session.CreateQuery("from Item");
				multiQuery.Add("first", firstQuery).Add("second", secondQuery);
				var secondResult = (IList)await (multiQuery.GetResultAsync("second"));
				var firstResult = (IList)await (multiQuery.GetResultAsync("first"));
				Assert.Greater(secondResult.Count, firstResult.Count);
			}
		}

		[Test]
		public async Task CanNotRetrieveCriteriaResultWithUnknownKeyAsync()
		{
			using (var session = OpenSession())
			{
				var multiQuery = session.CreateMultiQuery();
				multiQuery.Add("firstCriteria", session.CreateQuery("from Item"));
				try
				{
					var firstResult = (IList)await (multiQuery.GetResultAsync("unknownKey"));
					Assert.Fail("This should've thrown an InvalidOperationException");
				}
				catch (InvalidOperationException)
				{
				}
				catch (Exception)
				{
					Assert.Fail("This should've thrown an InvalidOperationException");
				}
			}
		}

		[Test]
		public async Task ExecutingCriteriaThroughMultiQueryTransformsResultsAsync()
		{
			await (CreateItemsAsync());
			using (var session = OpenSession())
			{
				var transformer = new ResultTransformerStub();
				var criteria = session.CreateQuery("from Item").SetResultTransformer(transformer);
				await (session.CreateMultiQuery().Add(criteria).ListAsync());
				Assert.IsTrue(transformer.WasTransformTupleCalled, "Transform Tuple was not called");
				Assert.IsTrue(transformer.WasTransformListCalled, "Transform List was not called");
			}
		}

		[Test]
		public async Task ExecutingCriteriaThroughMultiQueryTransformsResults_When_setting_on_multi_query_directlyAsync()
		{
			await (CreateItemsAsync());
			using (var session = OpenSession())
			{
				var transformer = new ResultTransformerStub();
				var query = session.CreateQuery("from Item");
				await (session.CreateMultiQuery().Add(query).SetResultTransformer(transformer).ListAsync());
				Assert.IsTrue(transformer.WasTransformTupleCalled, "Transform Tuple was not called");
				Assert.IsTrue(transformer.WasTransformListCalled, "Transform List was not called");
			}
		}

		[Test]
		public async Task ExecutingCriteriaThroughMultiCriteriaTransformsResultsAsync()
		{
			await (CreateItemsAsync());
			using (var session = OpenSession())
			{
				var transformer = new ResultTransformerStub();
				var criteria = session.CreateCriteria(typeof (Item)).SetResultTransformer(transformer);
				var multiCriteria = session.CreateMultiCriteria().Add(criteria);
				await (multiCriteria.ListAsync());
				Assert.IsTrue(transformer.WasTransformTupleCalled, "Transform Tuple was not called");
				Assert.IsTrue(transformer.WasTransformListCalled, "Transform List was not called");
			}
		}

		[Test]
		public async Task CanGetResultsInAGenericListAsync()
		{
			using (var s = OpenSession())
			{
				var getItems = s.CreateQuery("from Item");
				var countItems = s.CreateQuery("select count(*) from Item");
				var results = await (s.CreateMultiQuery().Add(getItems).Add<long>(countItems).ListAsync());
				Assert.That(results[0], Is.InstanceOf<List<object>>());
				Assert.That(results[1], Is.InstanceOf<List<long>>());
			}
		}

		[Test]
		public async Task CanGetResultsInAGenericListClassAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var item1 = new Item{Id = 1, Name = "test item"};
					var item2 = new Item{Id = 2, Name = "test child", Parent = item1};
					await (s.SaveAsync(item1));
					await (s.SaveAsync(item2));
					await (tx.CommitAsync());
					s.Clear();
				}

			using (var s = OpenSession())
			{
				var getItems = s.CreateQuery("from Item");
				var parents = s.CreateQuery("select Parent from Item");
				var results = await (s.CreateMultiQuery().Add(getItems).Add<Item>(parents).ListAsync());
				Assert.That(results[0], Is.InstanceOf<List<object>>());
				Assert.That(results[1], Is.InstanceOf<List<Item>>());
			}
		}
	}
}
#endif
