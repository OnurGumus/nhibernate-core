#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GroupByHavingTestsAsync : LinqTestCaseAsync
	{
		protected override async Task ConfigureAsync(Cfg.Configuration configuration)
		{
			await (base.ConfigureAsync(configuration));
			configuration.SetProperty(Cfg.Environment.ShowSql, "true");
		}

		[Test]
		public void HavingCountSelectCount()
		{
			var list = (
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Count()).ToList();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single(), Is.EqualTo(5));
		}

		[Test]
		public void HavingCountSelectCountWithInnerWhere()
		{
			var list = (
				from o in db.Orders
				where o.RequiredDate < DateTime.Now
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Count()).ToList();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single(), Is.EqualTo(5));
		}

		[Test]
		public void HavingCountSelectKey()
		{
			var list = (
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Key).ToList();
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public void HavingCountSelectKeyWithInnerWhere()
		{
			var list = (
				from o in db.Orders
				where o.RequiredDate < DateTime.Now
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Key).ToList();
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public void HavingCountSelectTupleKeyCount()
		{
			var list = (
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select new
					{
					g.Key, Count = g.Count()}

			).ToList();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single().Count, Is.EqualTo(5));
		}

		[Test]
		public void HavingCountSelectTupleKeyCountOfOrders()
		{
			var list = (
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select new
					{
					g.Key, Count = g.Select(x => x.OrderId).Count()}

			).ToList();
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single().Count, Is.EqualTo(5));
		}

		[Test, Description("I suspect that this case isn't executed correctly - the sql doesn't mention the orderlines. /Oskar 2012-01-22")]
		public Task HavingCountSelectTupleKeyCountOfOrderLinesAsync()
		{
			try
			{
				Assert.Throws<AssertionException>(() =>
				{
					var list = (
						from o in db.Orders
						group o by o.OrderDate into g
							where g.Count() > 4
							select new
							{
							g.Key, Count = g.SelectMany(x => x.OrderLines).Count()}

					).ToList();
					Assert.That(list.Count, Is.EqualTo(1));
					Assert.That(list.Single().Count, Is.EqualTo(14));
				}

				, KnownBug.Issue("NH-????"));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void ComplexQuery()
		{
			var list = (
				from o in db.Orders
				where o.RequiredDate < DateTime.Now
				group o by o.OrderDate into g
					where g.Count() > 4 && g.Key < DateTime.Now
					select g.Count()).ToList();
			Assert.AreEqual(1, list.Count);
		}

		[Test]
		public void SingleKeyGroupAndCountWithHavingClause()
		{
			//NH-2833
			var orderCounts = db.Orders.GroupBy(o => o.Customer.CompanyName).Where(g => g.Count() > 10).Select(g => new
			{
			CompanyName = g.Key, OrderCount = g.Count()}

			).ToList();
			Assert.That(orderCounts, Has.Count.EqualTo(28));
			var hornRow = orderCounts.Single(row => row.CompanyName == "Around the Horn");
			Assert.That(hornRow.OrderCount, Is.EqualTo(13));
		}
	}
}
#endif
