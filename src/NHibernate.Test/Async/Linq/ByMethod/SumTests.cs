#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Exceptions;
using NHibernate.Util;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SumTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public Task EmptySumDecimalAsync()
		{
			try
			{
				Assert.Throws<GenericADOException>(() =>
				{
					db.OrderLines.Where(ol => false).Sum(ol => ol.Discount);
				}

				);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void EmptySumCastNullableDecimal()
		{
			decimal total = db.OrderLines.Where(ol => false).Sum(ol => (decimal ? )ol.Discount) ?? 0;
			Assert.AreEqual(0, total);
		}

		[Test]
		public void SumDecimal()
		{
			decimal total = db.OrderLines.Sum(ol => ol.Discount);
			Assert.Greater(total, 0);
		}

		[Test]
		public void EmptySumNullableDecimal()
		{
			decimal total = db.Orders.Where(ol => false).Sum(ol => ol.Freight) ?? 0;
			Assert.AreEqual(0, total);
		}

		[Test]
		public void SumNullableDecimal()
		{
			decimal ? total = db.Orders.Sum(ol => ol.Freight);
			Assert.Greater(total, 0);
		}

		[Test]
		public void SumSingle()
		{
			float total = db.Products.Sum(p => p.ShippingWeight);
			Assert.Greater(total, 0);
		}
	}
}
#endif
