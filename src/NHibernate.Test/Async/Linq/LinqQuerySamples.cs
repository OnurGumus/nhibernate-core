#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LinqQuerySamplesAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task GroupTwoQueriesAndSumAsync()
		{
			//NH-3534
			var queryWithAggregation =
				from o1 in db.Orders
				from o2 in db.Orders
				where o1.Customer.CustomerId == o2.Customer.CustomerId && o1.OrderDate == o2.OrderDate
				group o1 by new
				{
				o1.Customer.CustomerId, o1.OrderDate
				}

					into g
					select new
					{
					CustomerId = g.Key.CustomerId, LastOrderDate = g.Max(x => x.OrderDate)}

			;
			var result = await (queryWithAggregation.ToListAsync());
			Assert.IsNotNull(result);
			Assert.IsNotEmpty(result);
		}

		[Category("SELECT/DISTINCT")]
		[Test(Description = "This sample uses SELECT and anonymous types to return a sequence of just the Customers' contact names and phone numbers.")]
		public async Task DLinq10Async()
		{
			var q =
				from c in db.Customers
				select new
				{
				c.ContactName, c.Address.PhoneNumber
				}

			;
			var items = await (q.ToListAsync());
			Assert.AreEqual(91, items.Count);
			items.Each(x =>
			{
				Assert.IsNotNull(x.ContactName);
				Assert.IsNotNull(x.PhoneNumber);
			}

			);
		}

		[Category("SELECT/DISTINCT")]
		[Test(Description = "This sample uses SELECT and anonymous types to return " + "a sequence of just the Employees' names and phone numbers, " + "with the FirstName and LastName fields combined into a single field, 'Name', " + "and the HomePhone field renamed to Phone in the resulting sequence.")]
		public async Task DLinq11Async()
		{
			var q =
				from e in db.Employees
				select new
				{
				Name = e.FirstName + " " + e.LastName, Phone = e.Address.PhoneNumber
				}

			;
			var items = await (q.ToListAsync());
			Assert.AreEqual(9, items.Count);
			items.Each(x =>
			{
				Assert.IsNotNull(x.Name);
				Assert.IsNotNull(x.Phone);
			}

			);
		}

		[Category("SELECT/DISTINCT")]
		[Test(Description = "This sample uses nested queries to return a sequence of " + "all orders containing their OrderId, a subsequence of the " + "items in the order where there is a discount, and the money " + "saved if shipping is not included.")]
		[Ignore("TODO - nested select")]
		public async Task DLinq17Async()
		{
			using (ISession s = OpenSession())
			{
				/////////////
				///// Flattened Select
				/////////////
				//// In HQL select, get all the data that's needed
				//var dbOrders =
				//    s.CreateQuery("select o.OrderId, od, o.Freight from Order o join o.OrderLines od").List<object[]>();
				//// Now group by the items in the parent select, grouping the items in the child select (note lookups on object[], ala SelectClauseVisitor)
				//// Note the casts to get the types correct.  Need to check if SelectClauseVisitor handles that, but think it does
				//var a = from o in dbOrders
				//        group new { OrderLine = (OrderLine)o[1], Freight = (Decimal?)o[2] } by new { OrderId = (int) o[0] }
				//            into g
				//            select
				//            // Select the parent items,  and the child items in a nested select
				//            new { g.Key.OrderId, DiscountedProducts = from e in g select new { e.OrderLine, FreeShippingDiscount = e.Freight } };
				//a.ToList();
				/////////////
				///// Nested Select
				/////////////
				//var dbOrders2 = s.CreateQuery("select o.OrderId from Order o").List<int>();
				//var q2 = from o in dbOrders2
				//         select new
				//                    {
				//                        OrderId = o,
				//                        DiscountedProducts =
				//                             from subO in db.Orders
				//                                 where subO.OrderId == o
				//                                 from orderLine in subO.OrderLines
				//                                 select new { orderLine, FreeShippingDiscount = subO.Freight }
				//                    };
				//q2.ToList();
				///////////
				///// Batching Select
				///////////
				var dbOrders3 = await (s.CreateQuery("select o.OrderId from Order o").ListAsync<int>());
				//var q3 = dbOrders3.SubQueryBatcher(orderId => orderId,
				//                                   ids => from subO in db.Orders.ToList()  // Note that ToList is just because current group by code is incorrent in our linq provider
				//                                          where ids.Contains(subO.OrderId)
				//                                          from orderLine in subO.OrderLines
				//                                          group new {orderLine, FreeShippingDiscount = subO.Freight}
				//                                             by subO.OrderId
				//                                          into g
				//                                             select g
				//                                   )
				//                                   .Select((input, index) => new
				//                                    {
				//                                         OrderId = input.Item,
				//                                         DiscountedProducts = input.Batcher.GetData(index)
				//                    });
				// This is what we want:
				//var q3 = dbOrders3.SubQueryBatcher(orderId => orderId,
				//                                   ids => db.Orders
				//                                       .Where(o => ids.Contains(o.OrderId))
				//                                       .Select(o => new {o.OrderId, o.OrderLines, o.Freight}).ToList()
				//                                       .GroupBy(k => k.OrderId, e => new { e.OrderLines, FreeShippingDiscount = e.Freight})
				//                                   )
				//                                   .Select((input, index) => new
				//                                    {
				//                                         OrderId = input.Item,
				//                                         DiscountedProducts = input.Batcher.GetData(index)
				//                    });
				// This is what we're using since our provider can't yet handle the in or the group by clauses correctly (note the ToList and the Where clause moving to get us into Linq to Objects world)
				var q3 = dbOrders3.SubQueryBatcher(orderId => orderId, ids => (
					from o in db.Orders
					from ol in o.OrderLines
					select new
					{
					OrderLines = ol, FreeShippingDiscount = o.Freight, o.OrderId
					}

				).ToList().Where(o => ids.Contains(o.OrderId)).GroupBy(k => k.OrderId, e => new
				{
				e.OrderLines, e.FreeShippingDiscount
				}

				)).Select((input, index) => new
				{
				OrderId = input.Item, DiscountedProducts = input.Batcher.GetData(index)}

				);
				foreach (var x in q3)
				{
					Console.WriteLine(x.OrderId);
					foreach (var y in x.DiscountedProducts)
					{
						Console.WriteLine(y.FreeShippingDiscount);
					}
				}

				q3.ToList();
			}

			var q =
				from o in db.Orders
				select new
				{
				o.OrderId, DiscountedProducts =
					from od in o.OrderLines
					//                                    from od in o.OrderLines.Cast<OrderLine>()
					where od.Discount > 0.0m
					select od, FreeShippingDiscount = o.Freight
				}

			;
			ObjectDumper.Write(q, 1);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Count to find the number of Customers in the database.")]
		public async Task DLinq19Async()
		{
			int q = await (db.Customers.CountAsync());
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Count to find the number of Products in the database " + "that are not discontinued.")]
		public async Task DLinq20Async()
		{
			int q = await (db.Products.CountAsync(p => !p.Discontinued));
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Sum to find the total freight over all Orders.")]
		public async Task DLinq21Async()
		{
			decimal ? q = await (db.Orders.Select(o => o.Freight).SumAsync());
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Sum to find the total number of units on order over all Products.")]
		public async Task DLinq22Async()
		{
			int ? q = await (db.Products.SumAsync(p => p.UnitsOnOrder));
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Min to find the lowest unit price of any Product.")]
		public async Task DLinq23Async()
		{
			decimal ? q = await (db.Products.Select(p => p.UnitPrice).MinAsync());
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Min to find the lowest freight of any Order.")]
		public async Task DLinq24Async()
		{
			decimal ? q = await (db.Orders.MinAsync(o => o.Freight));
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Min to find the Products that have the lowest unit price " + "in each category.")]
		[Ignore("TODO nested aggregating group by")]
		public async Task DLinq25Async()
		{
			using (var session = OpenSession())
			{
				var output = (await (session.CreateQuery("select p.Category.CategoryId, p from Product p where p.UnitPrice = (select min(p2.UnitPrice) from Product p2 where p.Category.CategoryId = p2.Category.CategoryId)").ListAsync<object[]>())).GroupBy(input => input[0]).Select(input => new
				{
				CategoryId = (int)input.Key, CheapestProducts =
					from g in input
					select (Product)g[1]
				}

				);
			}

			/*
			 * From g, only using g.Key, min(UnitPrice), g
			 *  - g.Key is fine
			 *  - min(UnitPrice) is fine
			 *  - g is the problem.  Can't just issue a single select since it's non-aggregating
			 *    However, don't want to loose the aggregate; need that processed in the DB
			 * 
			 * To get additional information over and above g.Key and any aggregates, need a where clause against the aggregate:
			 * 
			 * select xxx, yyy, zzz from Product p where p.UnitPrice = (select min(p2.UnitPrice) from Product p2)
			 * 
			 * the outer where comes from the inner where in the queryModel:
			 *
			 * where p2.UnitPrice == g.Min(p3 => p3.UnitPrice)
			 * 
			 * also need additional constraints on the aggregate to fulfil the groupby requirements:
			 * 
			 * where p.Category.CategoryId = p2.Category.CategoryId
			 * 
			 * so join the inner select to the outer select using the group by criteria
			 * 
			 * finally, need to do some client-side processing to get the "shape" correct
			 * 
			 */
			var categories =
				from p in db.Products
				group p by p.Category.CategoryId into g
					select new
					{
					CategoryId = g.Key, CheapestProducts = (IEnumerable<Product>)(
						from p2 in g
						where p2.UnitPrice == g.Min(p3 => p3.UnitPrice)select p2)}

			;
			Console.WriteLine(ObjectDumper.Write(categories, 1));
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Max to find the latest hire date of any Employee.")]
		public async Task DLinq26Async()
		{
			DateTime? q = await (db.Employees.Select(e => e.HireDate).MaxAsync());
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Max to find the most units in stock of any Product.")]
		public async Task DLinq27Async()
		{
			int ? q = await (db.Products.MaxAsync(p => p.UnitsInStock));
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Average to find the average freight of all Orders.")]
		public async Task DLinq29Async()
		{
			decimal ? q = await (db.Orders.Select(o => o.Freight).AverageAsync());
			Console.WriteLine(q);
		}

		[Category("COUNT/SUM/MIN/MAX/AVG")]
		[Test(Description = "This sample uses Average to find the average unit price of all Products.")]
		public async Task DLinq30Async()
		{
			decimal ? q = await (db.Products.AverageAsync(p => p.UnitPrice));
			Console.WriteLine(q);
		}

		[Category("WHERE")]
		[Test(Description = "This sample uses First to select the first Shipper in the table.")]
		public async Task DLinq6Async()
		{
			Shipper shipper = await (db.Shippers.FirstAsync());
			Assert.AreEqual(1, shipper.ShipperId);
		}

		[Category("WHERE")]
		[Test(Description = "This sample uses First to select the single Customer with CustomerId 'BONAP'.")]
		public async Task DLinq7Async()
		{
			Customer cust = await (db.Customers.FirstAsync(c => c.CustomerId == "BONAP"));
			Assert.AreEqual("BONAP", cust.CustomerId);
		}

		[Category("WHERE")]
		[Test(Description = "This sample uses First to select an Order with freight greater than 10.00.")]
		public async Task DLinq8Async()
		{
			Order ord = await (db.Orders.FirstAsync(o => o.Freight > 10.00M));
			Assert.Greater(ord.Freight, 10.00M);
		}

		[Category("SELECT/DISTINCT")]
		[Test(Description = "This sample uses SELECT to return a sequence of just the Customers' contact names.")]
		public async Task DLinq9Async()
		{
			IQueryable<string> q =
				from c in db.Customers
				select c.ContactName;
			IList<string> items = await (q.ToListAsync());
			Assert.AreEqual(91, items.Count);
			items.Each(Assert.IsNotNull);
		}

		[Category("JOIN")]
		[Test(Description = "This sample uses foreign key navigation in the " + "from clause to select all orders for customers.")]
		public async Task DLinqJoin1cAsync()
		{
			IQueryable<Order> q =
				from c in db.Customers
				from o in c.Orders
				//                from o in c.Orders.Cast<Order>()
				select o;
			List<Order> list = await (q.ToListAsync());
			ObjectDumper.Write(q);
		}

		[Category("JOIN")]
		[Test(Description = "This sample uses foreign key navigation in the " + "from clause to select all orders for customers.")]
		public async Task DLinqJoin1dAsync()
		{
			IQueryable<DateTime? > q =
				from c in db.Customers
				//                from o in c.Orders.Cast<Order>()
				from o in c.Orders
				select o.OrderDate;
			List<DateTime? > list = await (q.ToListAsync());
			ObjectDumper.Write(q);
		}

		[Category("JOIN")]
		[Test(Description = "This sample uses foreign key navigation in the " + "from clause to select all orders for customers.")]
		public async Task DLinqJoin1eAsync()
		{
			IQueryable<Customer> q =
				from c in db.Customers
				from o in c.Orders
				//                from o in c.Orders.Cast<Order>()
				select c;
			List<Customer> list = await (q.ToListAsync());
			ObjectDumper.Write(q);
		}

		[Category("JOIN")]
		[Test(Description = "This sample shows a group join with a composite key.")]
		public async Task DLinqJoin9Async()
		{
			var expected = (
				from o in db.Orders.ToList()from p in db.Products.ToList()join d in db.OrderLines.ToList()on new
				{
				o.OrderId, p.ProductId
				}

				equals new
				{
				d.Order.OrderId, d.Product.ProductId
				}

				into details
				from d in details
				select new
				{
				o.OrderId, p.ProductId, d.UnitPrice
				}

			).ToList();
			var actual = await ((
				from o in db.Orders
				from p in db.Products
				join d in db.OrderLines on new
				{
				o.OrderId, p.ProductId
				}

				equals new
				{
				d.Order.OrderId, d.Product.ProductId
				}

				into details
				from d in details
				select new
				{
				o.OrderId, p.ProductId, d.UnitPrice
				}

			).ToListAsync());
			Assert.AreEqual(expected.Count, actual.Count);
		}
	}
}
#endif
