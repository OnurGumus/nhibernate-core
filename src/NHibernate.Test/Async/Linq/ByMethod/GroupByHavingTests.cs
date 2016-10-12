#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GroupByHavingTestsAsync : LinqTestCaseAsync
	{
		protected override void Configure(Cfg.Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Cfg.Environment.ShowSql, "true");
		}

		[Test]
		public async Task HavingCountSelectCountAsync()
		{
			var list = await ((
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Count()).ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single(), Is.EqualTo(5));
		}

		[Test]
		public async Task HavingCountSelectCountWithInnerWhereAsync()
		{
			var list = await ((
				from o in db.Orders
				where o.RequiredDate < DateTime.Now
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Count()).ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single(), Is.EqualTo(5));
		}

		[Test]
		public async Task HavingCountSelectKeyAsync()
		{
			var list = await ((
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Key).ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task HavingCountSelectKeyWithInnerWhereAsync()
		{
			var list = await ((
				from o in db.Orders
				where o.RequiredDate < DateTime.Now
				group o by o.OrderDate into g
					where g.Count() > 4
					select g.Key).ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
		}

		[Test]
		public async Task HavingCountSelectTupleKeyCountAsync()
		{
			var list = await ((
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select new
					{
					g.Key, Count = g.Count()}

			).ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single().Count, Is.EqualTo(5));
		}

		[Test]
		public async Task HavingCountSelectTupleKeyCountOfOrdersAsync()
		{
			var list = await ((
				from o in db.Orders
				group o by o.OrderDate into g
					where g.Count() > 4
					select new
					{
					g.Key, Count = g.Select(x => x.OrderId).Count()}

			).ToListAsync());
			Assert.That(list.Count, Is.EqualTo(1));
			Assert.That(list.Single().Count, Is.EqualTo(5));
		}

		[Test, Description("I suspect that this case isn't executed correctly - the sql doesn't mention the orderlines. /Oskar 2012-01-22")]
		public async Task HavingCountSelectTupleKeyCountOfOrderLinesAsync()
		{
			Assert.ThrowsAsync<AssertionException>(async () =>
			{
				var list = await ((
					from o in db.Orders
					group o by o.OrderDate into g
						where g.Count() > 4
						select new
						{
						g.Key, Count = g.SelectMany(x => x.OrderLines).Count()}

				).ToListAsync());
				Assert.That(list.Count, Is.EqualTo(1));
				Assert.That(list.Single().Count, Is.EqualTo(14));
			}

			, KnownBug.Issue("NH-????"));
		}

		[Test]
		public async Task ComplexQueryAsync()
		{
			var list = await ((
				from o in db.Orders
				where o.RequiredDate < DateTime.Now
				group o by o.OrderDate into g
					where g.Count() > 4 && g.Key < DateTime.Now
					select g.Count()).ToListAsync());
			Assert.AreEqual(1, list.Count);
		}

		[Test]
		public async Task SingleKeyGroupAndCountWithHavingClauseAsync()
		{
			//NH-2833
			var orderCounts = await (db.Orders.GroupBy(o => o.Customer.CompanyName).Where(g => g.Count() > 10).Select(g => new
			{
			CompanyName = g.Key, OrderCount = g.Count()}

			).ToListAsync());
			Assert.That(orderCounts, Has.Count.EqualTo(28));
			var hornRow = orderCounts.Single(row => row.CompanyName == "Around the Horn");
			Assert.That(hornRow.OrderCount, Is.EqualTo(13));
		}
	}
}
#endif
