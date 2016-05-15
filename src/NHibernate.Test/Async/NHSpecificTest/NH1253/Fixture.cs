#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1253
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		// The test only check that there are no lost parameter set (no exception)
		[Test]
		public async Task TestParametersWithTrailingNumbersSingleInListAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var q = s.CreateQuery("from Car c where c.Make in (:param1) or c.Model in (:param11)");
					q.SetParameterList("param11", new string[]{"Model1", "Model2"});
					q.SetParameterList("param1", new string[]{"Make1", "Make2"});
					var cars = await (q.ListAsync<Car>());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestParametersWithTrailingNumbersSingleInListReverseAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var q = s.CreateQuery("from Car c where c.Make in (:param1) or c.Model in (:param11)");
					q.SetParameterList("param1", new string[]{"Model1", "Model2"});
					q.SetParameterList("param11", new string[]{"Make1", "Make2"});
					var cars = await (q.ListAsync<Car>());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestSamePartialNameAsync()
		{
			// Demonstration of NH-1422
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var q = s.CreateQuery("from Car c where c.Id in (:foo) or c.Id = :foobar");
					q.SetParameterList("foo", new long[]{1, 2});
					q.SetInt64("foobar", 3);
					var cars = await (q.ListAsync<Car>());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestParametersWithTrailingNumbersMultipleInListAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var q = s.CreateQuery("from Car c where c.Make in (:param11) or c.Model in (:param1)");
					q.SetParameterList("param11", new string[]{"One", "Two"});
					q.SetParameterList("param1", new string[]{"One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve"});
					var cars = await (q.ListAsync());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task MultiQuerySingleInListAsync()
		{
			var driver = sessions.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var results = await (s.CreateMultiQuery().Add("from Car c where c.Make in (:param1) or c.Model in (:param11)").Add("from Car c where c.Make in (:param1) or c.Model in (:param11)").SetParameterList("param11", new string[]{"Model1", "Model2"}).SetParameterList("param1", new string[]{"Make1", "Make2"}).ListAsync());
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
