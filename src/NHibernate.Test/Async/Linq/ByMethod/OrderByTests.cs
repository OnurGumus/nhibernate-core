#if NET_4_5
using System;
using System.Linq;
using NHibernate.Dialect;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using NHibernate.Linq;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq.ByMethod
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OrderByTestsAsync : LinqTestCaseAsync
	{
		protected override void Configure(Cfg.Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Environment.ShowSql, "true");
		}

		[Test]
		public async Task GroupByThenOrderByAsync()
		{
			var query =
				from c in db.Customers
				group c by c.Address.Country into g
					orderby g.Key
					select new
					{
					Country = g.Key, Count = g.Count()}

			;
			var ids = await (query.ToListAsync());
			Assert.NotNull(ids);
			AssertOrderedBy.Ascending(ids, arg => arg.Country);
		}

		[Test]
		public async Task AscendingOrderByClauseAsync()
		{
			var query =
				from c in db.Customers
				orderby c.CustomerId
				select c.CustomerId;
			var ids = await (query.ToListAsync());
			if (ids.Count > 1)
			{
				Assert.Greater(ids[1], ids[0]);
			}
		}

		[Test]
		public async Task DescendingOrderByClauseAsync()
		{
			var query =
				from c in db.Customers
				orderby c.CustomerId descending
				select c.CustomerId;
			var ids = await (query.ToListAsync());
			if (ids.Count > 1)
			{
				Assert.Greater(ids[0], ids[1]);
			}
		}

		[Test]
		public async Task ComplexAscendingOrderByClauseAsync()
		{
			var query =
				from c in db.Customers
				where c.Address.Country == "Belgium"
				orderby c.Address.Country, c.Address.City
				select c.Address.City;
			var ids = await (query.ToListAsync());
			if (ids.Count > 1)
			{
				Assert.Greater(ids[1], ids[0]);
			}
		}

		[Test]
		public async Task ComplexDescendingOrderByClauseAsync()
		{
			var query =
				from c in db.Customers
				where c.Address.Country == "Belgium"
				orderby c.Address.Country descending, c.Address.City descending
				select c.Address.City;
			var ids = await (query.ToListAsync());
			if (ids.Count > 1)
			{
				Assert.Greater(ids[0], ids[1]);
			}
		}

		[Test]
		public async Task ComplexAscendingDescendingOrderByClauseAsync()
		{
			var query =
				from c in db.Customers
				where c.Address.Country == "Belgium"
				orderby c.Address.Country ascending, c.Address.City descending
				select c.Address.City;
			var ids = await (query.ToListAsync());
			if (ids.Count > 1)
			{
				Assert.Greater(ids[0], ids[1]);
			}
		}

		[Test]
		public async Task OrderByDoesNotFilterResultsOnJoinAsync()
		{
			// Check preconditions.
			var allAnimalsWithNullFather =
				from a in db.Animals
				where a.Father == null
				select a;
			Assert.Greater(await (allAnimalsWithNullFather.CountAsync()), 0);
			// Check join result.
			var allAnimals = db.Animals;
			var orderedAnimals =
				from a in db.Animals
				orderby a.Father.SerialNumber
				select a;
			// ReSharper disable RemoveToList.2
			// We to ToList() first or it skips the generation of the joins.
			Assert.AreEqual((await (allAnimals.ToListAsync())).Count(), orderedAnimals.ToList().Count());
		// ReSharper restore RemoveToList.2
		}

		[Test(Description = "NH-3217")]
		public async Task OrderByNullCompareAndSkipAndTakeAsync()
		{
			await (db.Orders.OrderBy(o => o.Shipper == null ? 0 : o.Shipper.ShipperId).Skip(3).Take(4).ToListAsync());
		}

		[Test(Description = "NH-3445")]
		public async Task OrderByWithSelectDistinctAndTakeAsync()
		{
			Assert.ThrowsAsync<NotSupportedException>(async () =>
			{
				await (db.Orders.Select(o => o.ShippedTo).Distinct().OrderBy(o => o).Take(1000).ToListAsync());
			}

			, KnownBug.Issue("NH-3445"));
		}
	}
}
#endif
