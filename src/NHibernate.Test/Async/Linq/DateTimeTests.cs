#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task CanQueryByYearAsync()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value.Year == 1998
				select o).ToListAsync());
			Assert.AreEqual(270, x.Count());
		}

		[Test]
		public async Task CanQueryByDateAsync()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value.Date == new DateTime(1998, 02, 26)select o).ToListAsync());
			Assert.AreEqual(6, x.Count());
		}

		[Test]
		public async Task CanQueryByDateTimeAsync()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26)select o).ToListAsync());
			Assert.AreEqual(5, x.Count());
		}

		[Test]
		public async Task CanQueryByDateTime2Async()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26, 0, 1, 0)select o).ToListAsync());
			Assert.AreEqual(1, x.Count());
		}

		[Test]
		public async Task CanSelectYearAsync()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value.Year == 1998
				select o.OrderDate.Value.Year).ToListAsync());
			Assert.That(x, Has.All.EqualTo(1998));
			Assert.AreEqual(270, x.Count());
		}

		[Test]
		public async Task CanSelectDateAsync()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value.Date == new DateTime(1998, 02, 26)select o.OrderDate.Value.Date).ToListAsync());
			Assert.That(x, Has.All.EqualTo(new DateTime(1998, 02, 26)));
			Assert.AreEqual(6, x.Count());
		}

		[Test]
		public async Task CanSelectDateTimeAsync()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26)select o.OrderDate.Value).ToListAsync());
			Assert.That(x, Has.All.EqualTo(new DateTime(1998, 02, 26)));
			Assert.AreEqual(5, x.Count());
		}

		[Test]
		public async Task CanSelectDateTime2Async()
		{
			var x = await ((
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26, 0, 1, 0)select o.OrderDate.Value).ToListAsync());
			Assert.That(x, Has.All.EqualTo(new DateTime(1998, 02, 26, 0, 1, 0)));
			Assert.AreEqual(1, x.Count());
		}
	}
}
#endif
