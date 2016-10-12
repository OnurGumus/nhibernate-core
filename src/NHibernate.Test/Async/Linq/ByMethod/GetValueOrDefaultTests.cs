#if NET_4_5
using System.Linq;
using NHibernate.Dialect;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GetValueOrDefaultTestsAsync : LinqTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			// It seems that SQLite has a nasty bug with coalesce
			// Following query does not work
			//    SELECT order0_.*
			//    FROM   Orders order0_ 
			//    WHERE  coalesce(order0_.Freight, 0) > @p0;
			// And this one works
			//    SELECT order0_.*
			//    FROM   Orders order0_ 
			//    WHERE  cast(coalesce(order0_.Freight, 0) as NUMERIC) > @p0;
			if (dialect is SQLiteDialect)
				return false;
			return base.AppliesTo(dialect);
		}

		[Test]
		public async Task CoalesceInWhereAsync()
		{
			var orders = await (db.Orders.Where(x => (x.Freight ?? 100) > 0).ToListAsync());
			Assert.AreEqual(830, orders.Count);
		}

		[Test]
		public async Task GetValueOrDefaultInWhereAsync()
		{
			var orders = await (db.Orders.Where(x => x.Freight.GetValueOrDefault(100) > 0).ToListAsync());
			Assert.AreEqual(830, orders.Count);
		}

		[Test]
		public async Task GetValueOrDefaultWithSingleArgumentInWhereAsync()
		{
			var orders = await (db.Orders.Where(x => x.Freight.GetValueOrDefault() > 0).ToListAsync());
			Assert.AreEqual(830, orders.Count);
		}
	}
}
#endif
