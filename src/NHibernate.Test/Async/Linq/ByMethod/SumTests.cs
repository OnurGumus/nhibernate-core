#if NET_4_5
using System;
using System.Linq;
using NHibernate.Exceptions;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SumTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task EmptySumDecimalAsync()
		{
			Assert.ThrowsAsync<GenericADOException>(async () =>
			{
				await (db.OrderLines.Where(ol => false).SumAsync(ol => ol.Discount));
			}

			);
		}

		[Test]
		public async Task EmptySumCastNullableDecimalAsync()
		{
			decimal total = await (db.OrderLines.Where(ol => false).SumAsync(ol => (decimal ? )ol.Discount)) ?? 0;
			Assert.AreEqual(0, total);
		}

		[Test]
		public async Task SumDecimalAsync()
		{
			decimal total = await (db.OrderLines.SumAsync(ol => ol.Discount));
			Assert.Greater(total, 0);
		}

		[Test]
		public async Task EmptySumNullableDecimalAsync()
		{
			decimal total = await (db.Orders.Where(ol => false).SumAsync(ol => ol.Freight)) ?? 0;
			Assert.AreEqual(0, total);
		}

		[Test]
		public async Task SumNullableDecimalAsync()
		{
			decimal ? total = await (db.Orders.SumAsync(ol => ol.Freight));
			Assert.Greater(total, 0);
		}

		[Test]
		public async Task SumSingleAsync()
		{
			float total = await (db.Products.SumAsync(p => p.ShippingWeight));
			Assert.Greater(total, 0);
		}
	}
}
#endif
