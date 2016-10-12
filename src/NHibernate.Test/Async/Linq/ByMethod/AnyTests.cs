#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AnyTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task AnySublistAsync()
		{
			var orders = await (db.Orders.Where(o => o.OrderLines.Any(ol => ol.Quantity == 5)).ToListAsync());
			Assert.That(orders.Count, Is.EqualTo(61));
			orders = await (db.Orders.Where(o => o.OrderLines.Any(ol => ol.Order == null)).ToListAsync());
			Assert.That(orders.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task NestedAnyAsync()
		{
			var test = await ((
				from c in db.Customers
				where c.ContactName == "Bob" && (c.CompanyName == "NormalooCorp" || c.Orders.Any(o => o.OrderLines.Any(ol => ol.Discount < 20 && ol.Discount >= 10)))select c).ToListAsync());
			Assert.That(test.Count, Is.EqualTo(0));
		}

		[Test]
		public async Task ManyToManyAnyAsync()
		{
			var test = db.Orders.Where(o => o.Employee.FirstName == "test");
			var result = await (test.Where(o => o.Employee.Territories.Any(t => t.Description == "test")).ToListAsync());
			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test(Description = "NH-2654")]
		public async Task AnyWithCountAsync()
		{
			var result = await (db.Orders.AnyAsync(p => p.OrderLines.Count == 0));
			Assert.That(result, Is.False);
		}

		[Test]
		public async Task AnyWithFetchAsync()
		{
			//NH-3241
			var result = await (db.Orders.Fetch(x => x.Customer).FetchMany(x => x.OrderLines).AnyAsync());
		}
	}
}
#endif
