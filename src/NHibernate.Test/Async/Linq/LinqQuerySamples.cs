#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LinqQuerySamples : LinqTestCase
	{
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
	}
}
#endif
