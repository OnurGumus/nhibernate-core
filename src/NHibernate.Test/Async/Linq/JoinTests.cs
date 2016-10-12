#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task OrderLinesWith2ImpliedJoinShouldProduce2JoinsInSqlAsync()
		{
			//NH-3003
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.Customer.CompanyName == "Vins et alcools Chevalier"
					select l).ToListAsync());
				Assert.AreEqual(10, lines.Count);
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(2));
			}
		}

		[Test]
		public async Task OrderLinesWith2ImpliedJoinByIdShouldNotContainImpliedJoinAsync()
		{
			//NH-2946 + NH-3003 = NH-2451
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.Customer.CustomerId == "VINET"
					where l.Order.Customer.CompanyName == "Vins et alcools Chevalier"
					select l).ToListAsync());
				Assert.AreEqual(10, lines.Count);
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(2));
				Assert.That(Count(spy, "Orders"), Is.EqualTo(1));
			}
		}

		[Test]
		public async Task OrderLinesFilterByCustomerIdSelectLineShouldNotContainJoinWithCustomerAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.Customer.CustomerId == "VINET"
					select l).ToListAsync());
				Assert.AreEqual(10, lines.Count);
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
				Assert.That(Count(spy, "Customers"), Is.EqualTo(0));
			}
		}

		[Test]
		public async Task OrderLinesFilterByCustomerIdSelectCustomerIdShouldNotContainJoinWithCustomerAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.Customer.CustomerId == "VINET"
					select l.Order.Customer.CustomerId).ToListAsync());
				Assert.AreEqual(10, lines.Count);
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
				Assert.That(Count(spy, "Customers"), Is.EqualTo(0));
			}
		}

		[Test]
		public async Task OrderLinesFilterByCustomerIdSelectCustomerShouldContainJoinWithCustomerAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.Customer.CustomerId == "VINET"
					select l.Order.Customer).ToListAsync());
				Assert.AreEqual(10, lines.Count);
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(2));
				Assert.That(Count(spy, "Customers"), Is.EqualTo(1));
			}
		}

		[Test]
		public async Task OrderLinesFilterByCustomerCompanyNameAndSelectCustomerIdShouldJoinOrdersOnlyOnceAsync()
		{
			//NH-2946 + NH-3003 = NH-2451
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.Customer.CompanyName == "Vins et alcools Chevalier"
					select l.Order.Customer.CustomerId).ToListAsync());
				Assert.AreEqual(10, lines.Count);
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(2));
				Assert.That(Count(spy, "Orders"), Is.EqualTo(1));
			}
		}

		[Test]
		public async Task OrderLinesFilterByOrderDateAndSelectOrderIdAsync()
		{
			//NH-2451
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.OrderDate < DateTime.Now
					select l.Order.OrderId).ToListAsync());
				Assert.AreEqual(2155, lines.Count);
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task OrderLinesFilterByOrderIdAndSelectOrderDateAsync()
		{
			//NH-2451
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.OrderId == 100
					select l.Order.OrderDate).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
				Assert.That(Count(spy, "Orders"), Is.EqualTo(1));
			}
		}

		[Test]
		public async Task OrderLinesFilterByOrderIdAndSelectOrderAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				var lines = await ((
					from l in db.OrderLines
					where l.Order.OrderId == 100
					select l.Order).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
				Assert.That(Count(spy, "Orders"), Is.EqualTo(1));
			}
		}

		[Test]
		public async Task OrderLinesWithFilterByOrderIdShouldNotProduceJoinsAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				await ((
					from l in db.OrderLines
					where l.Order.OrderId == 1000
					select l).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(0));
			}
		}

		[Test]
		public async Task OrderLinesWithFilterByOrderIdAndDateShouldProduceOneJoinAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				await ((
					from l in db.OrderLines
					where l.Order.OrderId == 1000 && l.Order.OrderDate < DateTime.Now
					select l).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task OrderLinesWithSelectingOrderIdShouldNotProduceJoinsAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				await ((
					from l in db.OrderLines
					select l.Order.OrderId).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(0));
			}
		}

		[Test]
		public async Task OrderLinesWithSelectingOrderIdAndDateShouldProduceOneJoinAsync()
		{
			//NH-2946
			using (var spy = new SqlLogSpy())
			{
				await ((
					from l in db.OrderLines
					select new
					{
					l.Order.OrderId, l.Order.OrderDate
					}

				).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
			}
		}

		[Test(Description = "NH-3801")]
		public async Task OrderLinesWithSelectingCustomerIdInCaseShouldProduceOneJoinAsync()
		{
			using (var spy = new SqlLogSpy())
			{
				await ((
					from l in db.OrderLines
					select new
					{
					CustomerKnown = l.Order.Customer.CustomerId == null ? 0 : 1, l.Order.OrderDate
					}

				).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
			}
		}

		[Test(Description = "NH-3801"), Ignore("This is an ideal case, but not possible without better join detection")]
		public async Task OrderLinesWithSelectingCustomerInCaseShouldProduceOneJoinAsync()
		{
			using (var spy = new SqlLogSpy())
			{
				// Without nominating the conditional to the select clause (and placing it in SQL)
				// [l.Order.Customer] will be selected in its entirety, creating a second join 
				await ((
					from l in db.OrderLines
					select new
					{
					CustomerKnown = l.Order.Customer == null ? 0 : 1, l.Order.OrderDate
					}

				).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(1));
			}
		}

		[Test(Description = "NH-3801")]
		public async Task OrderLinesWithSelectingCustomerNameInCaseShouldProduceTwoJoinsAsync()
		{
			using (var spy = new SqlLogSpy())
			{
				await ((
					from l in db.OrderLines
					select new
					{
					CustomerKnown = l.Order.Customer.CustomerId == null ? "unknown" : l.Order.Customer.CompanyName, l.Order.OrderDate
					}

				).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(2));
			}
		}

		[Test(Description = "NH-3801"), Ignore("This is an ideal case, but not possible without better join detection")]
		public async Task OrderLinesWithSelectingCustomerNameInCaseShouldProduceTwoJoinsAlternateAsync()
		{
			using (var spy = new SqlLogSpy())
			{
				// Without nominating the conditional to the select clause (and placing it in SQL)
				// [l.Order.Customer] will be selected in its entirety, creating a second join 
				await ((
					from l in db.OrderLines
					select new
					{
					CustomerKnown = l.Order.Customer == null ? "unknown" : l.Order.Customer.CompanyName, l.Order.OrderDate
					}

				).ToListAsync());
				var countJoins = CountJoins(spy);
				Assert.That(countJoins, Is.EqualTo(2));
			}
		}

		private static int CountJoins(LogSpy sqlLog)
		{
			return Count(sqlLog, "join");
		}

		private static int Count(LogSpy sqlLog, string s)
		{
			var log = sqlLog.GetWholeLog();
			return log.Split(new[]{s}, StringSplitOptions.None).Length - 1;
		}
	}
}
#endif
