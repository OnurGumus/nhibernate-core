#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CountTestsAsync : LinqTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Cfg.Environment.ShowSql, "true");
		}

		[Test]
		public async Task CountDistinctProperty_ReturnsNumberOfDistinctEntriesForThatPropertyAsync()
		{
			//NH-2722
			var result = await (db.Orders.Select(x => x.ShippingDate).Distinct().CountAsync());
			Assert.That(result, Is.EqualTo(387));
		}

		[Test]
		public async Task CountProperty_ReturnsNumberOfNonNullEntriesForThatPropertyAsync()
		{
			//NH-2722
			var result = await (db.Orders.Select(x => x.ShippingDate).CountAsync());
			Assert.That(result, Is.EqualTo(809));
		}

		[Test]
		public async Task Count_ReturnsNumberOfRecordsAsync()
		{
			//NH-2722
			var result = await (db.Orders.CountAsync());
			Assert.That(result, Is.EqualTo(830));
		}

		[Test]
		public async Task LongCountDistinctProperty_ReturnsNumberOfDistinctEntriesForThatPropertyAsync()
		{
			//NH-2722
			var result = await (db.Orders.Select(x => x.ShippingDate).Distinct().LongCountAsync());
			Assert.That(result, Is.EqualTo(387));
		}

		[Test]
		public async Task LongCountProperty_ReturnsNumberOfNonNullEntriesForThatPropertyAsync()
		{
			//NH-2722
			var result = await (db.Orders.Select(x => x.ShippingDate).LongCountAsync());
			Assert.That(result, Is.EqualTo(809));
		}

		[Test]
		public async Task LongCount_ReturnsNumberOfRecordsAsync()
		{
			//NH-2722
			var result = await (db.Orders.LongCountAsync());
			Assert.That(result, Is.EqualTo(830));
		}

		[Test]
		public async Task CountOnJoinedGroupByAsync()
		{
			//NH-3001
			var query =
				from o in db.Orders
				join ol in db.OrderLines on o equals ol.Order
				group ol by ol.Product.ProductId into temp
					select new
					{
					temp.Key, count = temp.Count()}

			;
			var result = await (query.ToListAsync());
			Assert.That(result.Count, Is.EqualTo(77));
		}
	}
}
#endif
