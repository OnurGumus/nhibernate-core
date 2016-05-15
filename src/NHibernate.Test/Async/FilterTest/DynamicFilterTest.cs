#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using log4net;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.FilterTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicFilterTest : TestCase
	{
		[Test]
		public async Task SecondLevelCachedCollectionsFilteringAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			// Force a collection into the second level cache, with its non-filtered elements
			Salesperson sp = (Salesperson)await (session.LoadAsync(typeof (Salesperson), testData.steveId));
			await (NHibernateUtil.InitializeAsync(sp.Orders));
			ICollectionPersister persister = ((ISessionFactoryImplementor)sessions).GetCollectionPersister(typeof (Salesperson).FullName + ".Orders");
			Assert.IsTrue(persister.HasCache, "No cache for collection");
			CacheKey cacheKey = new CacheKey(testData.steveId, persister.KeyType, persister.Role, EntityMode.Poco, (ISessionFactoryImplementor)sessions);
			CollectionCacheEntry cachedData = (CollectionCacheEntry)persister.Cache.Cache.Get(cacheKey);
			Assert.IsNotNull(cachedData, "collection was not in cache");
			session.Close();
			session = OpenSession();
			session.EnableFilter("fulfilledOrders").SetParameter("asOfDate", testData.lastMonth);
			sp = (Salesperson)await (session.CreateQuery("from Salesperson as s where s.id = :id").SetInt64("id", testData.steveId).UniqueResultAsync());
			Assert.AreEqual(1, sp.Orders.Count, "Filtered-collection not bypassing 2L-cache");
			CollectionCacheEntry cachedData2 = (CollectionCacheEntry)persister.Cache.Cache.Get(cacheKey);
			Assert.IsNotNull(cachedData2, "collection no longer in cache!");
			Assert.AreSame(cachedData, cachedData2, "Different cache values!");
			session.Close();
			session = OpenSession();
			session.EnableFilter("fulfilledOrders").SetParameter("asOfDate", testData.lastMonth);
			sp = (Salesperson)await (session.LoadAsync(typeof (Salesperson), testData.steveId));
			Assert.AreEqual(1, sp.Orders.Count, "Filtered-collection not bypassing 2L-cache");
			session.Close();
			// Finally, make sure that the original cached version did not get over-written
			session = OpenSession();
			sp = (Salesperson)await (session.LoadAsync(typeof (Salesperson), testData.steveId));
			Assert.AreEqual(2, sp.Orders.Count, "Actual cached version got over-written");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task CombinedClassAndCollectionFiltersEnabledAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("regionlist").SetParameterList("regions", new string[]{"LA", "APAC"});
			session.EnableFilter("fulfilledOrders").SetParameter("asOfDate", testData.lastMonth);
			// test retreival through hql with the collection as non-eager
			IList salespersons = await (session.CreateQuery("select s from Salesperson as s").ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			Salesperson sp = (Salesperson)salespersons[0];
			Assert.AreEqual(1, sp.Orders.Count, "Incorrect order count");
			session.Clear();
			// test retreival through hql with the collection join fetched
			salespersons = await (session.CreateQuery("select s from Salesperson as s left join fetch s.Orders").ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			sp = (Salesperson)salespersons[0];
			Assert.AreEqual(sp.Orders.Count, 1, "Incorrect order count");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task FiltersWithQueryCacheAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("regionlist").SetParameterList("regions", new string[]{"LA", "APAC"});
			session.EnableFilter("fulfilledOrders").SetParameter("asOfDate", testData.lastMonth);
			// test retreival through hql with the collection as non-eager
			IList salespersons = await (session.CreateQuery("select s from Salesperson as s").SetCacheable(true).ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			// Try a second time, to make use of query cache
			salespersons = await (session.CreateQuery("select s from Salesperson as s").SetCacheable(true).ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			session.Clear();
			// test retreival through hql with the collection join fetched
			salespersons = await (session.CreateQuery("select s from Salesperson as s left join fetch s.Orders").SetCacheable(true).ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			// A second time, to make use of query cache
			salespersons = await (session.CreateQuery("select s from Salesperson as s left join fetch s.Orders").SetCacheable(true).ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task HqlFiltersAsync()
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// HQL test
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			log.Info("Starting HQL filter tests");
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("region").SetParameter("region", "APAC");
			session.EnableFilter("effectiveDate").SetParameter("asOfDate", testData.lastMonth);
			log.Info("HQL against Salesperson...");
			IList results = await (session.CreateQuery("select s from Salesperson as s left join fetch s.Orders").ListAsync());
			Assert.IsTrue(results.Count == 1, "Incorrect filtered HQL result count [" + results.Count + "]");
			Salesperson result = (Salesperson)results[0];
			Assert.IsTrue(result.Orders.Count == 1, "Incorrect collectionfilter count");
			log.Info("HQL against Product...");
			results = await (session.CreateQuery("from Product as p where p.StockNumber = ?").SetInt32(0, 124).ListAsync());
			Assert.IsTrue(results.Count == 1);
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task CriteriaQueryFiltersAsync()
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Criteria-query test
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			log.Info("Starting Criteria-query filter tests");
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("region").SetParameter("region", "APAC");
			session.EnableFilter("fulfilledOrders").SetParameter("asOfDate", testData.lastMonth);
			session.EnableFilter("effectiveDate").SetParameter("asOfDate", testData.lastMonth);
			log.Info("Criteria query against Salesperson...");
			IList salespersons = await (session.CreateCriteria(typeof (Salesperson)).SetFetchMode("orders", FetchMode.Join).ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			Assert.AreEqual(1, ((Salesperson)salespersons[0]).Orders.Count, "Incorrect order count");
			log.Info("Criteria query against Product...");
			IList products = await (session.CreateCriteria(typeof (Product)).Add(Expression.Eq("StockNumber", 124)).ListAsync());
			Assert.AreEqual(1, products.Count, "Incorrect product count");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task GetFiltersAsync()
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// Get() test
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			log.Info("Starting get() filter tests (eager assoc. fetching).");
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("region").SetParameter("region", "APAC");
			log.Info("Performing get()...");
			Salesperson salesperson = (Salesperson)await (session.GetAsync(typeof (Salesperson), testData.steveId));
			Assert.IsNotNull(salesperson);
			Assert.AreEqual(1, salesperson.Orders.Count, "Incorrect order count");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task OneToManyFiltersAsync()
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// one-to-many loading tests
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			log.Info("Starting one-to-many collection loader filter tests.");
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("seniorSalespersons").SetParameter("asOfDate", testData.lastMonth);
			log.Info("Performing Load of Department...");
			Department department = (Department)await (session.LoadAsync(typeof (Department), testData.deptId));
			ISet<Salesperson> salespersons = department.Salespersons;
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task InStyleFilterParameterAsync()
		{
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			// one-to-many loading tests
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			log.Info("Starting one-to-many collection loader filter tests.");
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("regionlist").SetParameterList("regions", new string[]{"LA", "APAC"});
			log.Debug("Performing query of Salespersons");
			IList salespersons = await (session.CreateQuery("from Salesperson").ListAsync());
			Assert.AreEqual(1, salespersons.Count, "Incorrect salesperson count");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task ManyToManyFilterOnCriteriaAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("effectiveDate").SetParameter("asOfDate", DateTime.Today);
			Product prod = (Product)await (session.CreateCriteria(typeof (Product)).SetResultTransformer(new DistinctRootEntityResultTransformer()).Add(Expression.Eq("id", testData.prod1Id)).UniqueResultAsync());
			Assert.IsNotNull(prod);
			Assert.AreEqual(1, prod.Categories.Count, "Incorrect Product.categories count for filter");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task ManyToManyFilterOnLoadAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("effectiveDate").SetParameter("asOfDate", DateTime.Today);
			Product prod = (Product)await (session.GetAsync(typeof (Product), testData.prod1Id));
			//long initLoadCount = sessions.Statistics.CollectionLoadCount;
			//long initFetchCount = sessions.Statistics.CollectionFetchCount;
			// should already have been initialized...
			Assert.IsTrue(NHibernateUtil.IsInitialized(prod.Categories));
			int size = prod.Categories.Count;
			Assert.AreEqual(1, size, "Incorrect filtered collection count");
			//long currLoadCount = sessions.Statistics.CollectionLoadCount;
			//long currFetchCount = sessions.Statistics.CollectionFetchCount;
			//Assert.IsTrue(
			//    (initLoadCount == currLoadCount) && (initFetchCount == currFetchCount),
			//    "Load with join fetch of many-to-many did not trigger join fetch"
			//    );
			// make sure we did not get back a collection of proxies
			//long initEntityLoadCount = sessions.Statistics.EntityLoadCount;
			foreach (Category cat in prod.Categories)
			{
				Assert.IsTrue(NHibernateUtil.IsInitialized(cat), "Load with join fetch of many-to-many did not trigger *complete* join fetch");
			//Console.WriteLine(" ===> " + cat.Name);
			}

			//long currEntityLoadCount = sessions.Statistics.EntityLoadCount;
			//Assert.IsTrue(
			//    (initEntityLoadCount == currEntityLoadCount),
			//    "Load with join fetch of many-to-many did not trigger *complete* join fetch"
			//    );
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task ManyToManyOnCollectionLoadAfterHQLAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("effectiveDate").SetParameter("asOfDate", DateTime.Today);
			// Force the categories to not get initialized here
			IList result = await (session.CreateQuery("from Product as p where p.id = :id").SetInt64("id", testData.prod1Id).ListAsync());
			Assert.IsTrue(result.Count > 0, "No products returned from HQL");
			Product prod = (Product)result[0];
			Assert.IsNotNull(prod);
			Assert.AreEqual(1, prod.Categories.Count, "Incorrect Product.categories count for filter on collection Load");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task ManyToManyFilterOnQueryAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			session.EnableFilter("effectiveDate").SetParameter("asOfDate", DateTime.Today);
			IList result = await (session.CreateQuery("from Product p inner join fetch p.Categories").ListAsync());
			Assert.IsTrue(result.Count > 0, "No products returned from HQL many-to-many filter case");
			Product prod = (Product)result[0];
			Assert.IsNotNull(prod);
			Assert.AreEqual(1, prod.Categories.Count, "Incorrect Product.categories count for filter with HQL");
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task ManyToManyBaseAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			Product prod = (Product)await (session.GetAsync(typeof (Product), testData.prod1Id));
			// TODO H3: Statistics
			//long initLoadCount = sessions.Statistics.CollectionLoadCount;
			//long initFetchCount = sessions.Statistics.CollectionFetchCount;
			// should already have been initialized...
			Assert.IsTrue(NHibernateUtil.IsInitialized(prod.Categories), "Load with join fetch of many-to-many did not trigger join fetch");
			int size = prod.Categories.Count;
			Assert.AreEqual(2, size, "Incorrect non-filtered collection count");
			//long currLoadCount = sessions.Statistics.CollectionLoadCount;
			//long currFetchCount = sessions.Statistics.CollectionFetchCount;
			//Assert.IsTrue(
			//        ( initLoadCount == currLoadCount ) && ( initFetchCount == currFetchCount ),
			//        "Load with join fetch of many-to-many did not trigger join fetch"
			//);
			// make sure we did not get back a collection of proxies
			// TODO H3: statistics
			//long initEntityLoadCount = sessions.Statistics.EntityLoadCount;
			foreach (Category cat in prod.Categories)
			{
				Assert.IsTrue(NHibernateUtil.IsInitialized(cat), "Load with join fetch of many-to-many did not trigger *complete* join fetch");
			//Console.WriteLine(" ===> " + cat.Name);
			}

			//long currEntityLoadCount = sessions.Statistics.EntityLoadCount;
			//Assert.IsTrue(
			//        ( initEntityLoadCount == currEntityLoadCount ),
			//        "Load with join fetch of many-to-many did not trigger *complete* join fetch"
			//);
			session.Close();
			await (testData.ReleaseAsync());
		}

		[Test]
		public async Task ManyToManyBaseThruCriteriaAsync()
		{
			TestData testData = new TestData(this);
			await (testData.PrepareAsync());
			ISession session = OpenSession();
			IList result = await (session.CreateCriteria(typeof (Product)).Add(Expression.Eq("id", testData.prod1Id)).ListAsync());
			Product prod = (Product)result[0];
			//long initLoadCount = sessions.Statistics.CollectionLoadCount;
			//long initFetchCount = sessions.Statistics.CollectionFetchCount;
			// should already have been initialized...
			Assert.IsTrue(NHibernateUtil.IsInitialized(prod.Categories), "Load with join fetch of many-to-many did not trigger join fetch");
			int size = prod.Categories.Count;
			Assert.AreEqual(2, size, "Incorrect non-filtered collection count");
			//long currLoadCount = sessions.Statistics.CollectionLoadCount;
			//long currFetchCount = sessions.Statistics.CollectionFetchCount;
			//Assert.IsTrue(
			//    (initLoadCount == currLoadCount) && (initFetchCount == currFetchCount),
			//    "Load with join fetch of many-to-many did not trigger join fetch"
			//    );
			// make sure we did not get back a collection of proxies
			//long initEntityLoadCount = sessions.Statistics.EntityLoadCount;
			foreach (Category cat in prod.Categories)
			{
				Assert.IsTrue(NHibernateUtil.IsInitialized(cat), "Load with join fetch of many-to-many did not trigger *complete* join fetch");
			//Console.WriteLine(" ===> " + cat.Name);
			}

			//long currEntityLoadCount = sessions.Statistics.EntityLoadCount;
			//Assert.IsTrue(
			//    (initEntityLoadCount == currEntityLoadCount),
			//    "Load with join fetch of many-to-many did not trigger *complete* join fetch"
			//    );
			session.Close();
			await (testData.ReleaseAsync());
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TestData
		{
			public async Task PrepareAsync()
			{
				ISession session = outer.OpenSession();
				ITransaction transaction = session.BeginTransaction();
				lastMonth = DateTime.Today.AddMonths(-1);
				nextMonth = DateTime.Today.AddMonths(1);
				sixMonthsAgo = DateTime.Today.AddMonths(-6);
				fourMonthsAgo = DateTime.Today.AddMonths(-4);
				Department dept = new Department();
				dept.Name = ("Sales");
				await (session.SaveAsync(dept));
				deptId = dept.Id;
				entitiesToCleanUp.Add(dept);
				Salesperson steve = new Salesperson();
				steve.Name = ("steve");
				steve.Region = ("APAC");
				steve.HireDate = (sixMonthsAgo);
				steve.Department = (dept);
				dept.Salespersons.Add(steve);
				Salesperson max = new Salesperson();
				max.Name = ("max");
				max.Region = ("EMEA");
				max.HireDate = (nextMonth);
				max.Department = (dept);
				dept.Salespersons.Add(max);
				await (session.SaveAsync(steve));
				await (session.SaveAsync(max));
				entitiesToCleanUp.Add(steve);
				entitiesToCleanUp.Add(max);
				steveId = steve.Id;
				Category cat1 = new Category("test cat 1", lastMonth, nextMonth);
				Category cat2 = new Category("test cat 2", sixMonthsAgo, fourMonthsAgo);
				Product product1 = new Product();
				product1.Name = ("Acme Hair Gel");
				product1.StockNumber = (123);
				product1.EffectiveStartDate = (lastMonth);
				product1.EffectiveEndDate = (nextMonth);
				product1.AddCategory(cat1);
				product1.AddCategory(cat2);
				await (session.SaveAsync(product1));
				entitiesToCleanUp.Add(product1);
				prod1Id = product1.Id;
				Order order1 = new Order();
				order1.Buyer = "gavin";
				order1.Region = ("APAC");
				order1.PlacementDate = sixMonthsAgo;
				order1.FulfillmentDate = fourMonthsAgo;
				order1.Salesperson = steve;
				order1.AddLineItem(product1, 500);
				await (session.SaveAsync(order1));
				entitiesToCleanUp.Add(order1);
				Product product2 = new Product();
				product2.Name = ("Acme Super-Duper DTO Factory");
				product2.StockNumber = (124);
				product2.EffectiveStartDate = (sixMonthsAgo);
				product2.EffectiveEndDate = (DateTime.Today);
				Category cat3 = new Category("test cat 2", sixMonthsAgo, DateTime.Today);
				product2.AddCategory(cat3);
				await (session.SaveAsync(product2));
				entitiesToCleanUp.Add(product2);
				// An uncategorized product
				Product product3 = new Product();
				product3.Name = ("Uncategorized product");
				await (session.SaveAsync(product3));
				entitiesToCleanUp.Add(product3);
				Order order2 = new Order();
				order2.Buyer = "christian";
				order2.Region = ("EMEA");
				order2.PlacementDate = lastMonth;
				order2.Salesperson = steve;
				order2.AddLineItem(product2, -1);
				await (session.SaveAsync(order2));
				entitiesToCleanUp.Add(order2);
				await (transaction.CommitAsync());
				session.Close();
			}

			public async Task ReleaseAsync()
			{
				ISession session = outer.OpenSession();
				ITransaction transaction = session.BeginTransaction();
				foreach (object obj in entitiesToCleanUp)
				{
					await (session.DeleteAsync(obj));
				}

				await (transaction.CommitAsync());
				session.Close();
			}
		}
	}
}
#endif
