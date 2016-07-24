#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public void CanQueryByYear()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value.Year == 1998
				select o).ToList();
			Assert.AreEqual(270, x.Count());
		}

		[Test]
		public void CanQueryByDate()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value.Date == new DateTime(1998, 02, 26)select o).ToList();
			Assert.AreEqual(6, x.Count());
		}

		[Test]
		public void CanQueryByDateTime()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26)select o).ToList();
			Assert.AreEqual(5, x.Count());
		}

		[Test]
		public void CanQueryByDateTime2()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26, 0, 1, 0)select o).ToList();
			Assert.AreEqual(1, x.Count());
		}

		[Test]
		public void CanSelectYear()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value.Year == 1998
				select o.OrderDate.Value.Year).ToList();
			Assert.That(x, Has.All.EqualTo(1998));
			Assert.AreEqual(270, x.Count());
		}

		[Test]
		public void CanSelectDate()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value.Date == new DateTime(1998, 02, 26)select o.OrderDate.Value.Date).ToList();
			Assert.That(x, Has.All.EqualTo(new DateTime(1998, 02, 26)));
			Assert.AreEqual(6, x.Count());
		}

		[Test]
		public void CanSelectDateTime()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26)select o.OrderDate.Value).ToList();
			Assert.That(x, Has.All.EqualTo(new DateTime(1998, 02, 26)));
			Assert.AreEqual(5, x.Count());
		}

		[Test]
		public void CanSelectDateTime2()
		{
			var x = (
				from o in db.Orders
				where o.OrderDate.Value == new DateTime(1998, 02, 26, 0, 1, 0)select o.OrderDate.Value).ToList();
			Assert.That(x, Has.All.EqualTo(new DateTime(1998, 02, 26, 0, 1, 0)));
			Assert.AreEqual(1, x.Count());
		}
	}
}
#endif
