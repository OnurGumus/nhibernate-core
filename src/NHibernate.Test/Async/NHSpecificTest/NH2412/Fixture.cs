﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2412
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override void OnTearDown()
		{
			using (ISession s = sessions.OpenSession())
			{
				s.Delete("from Order");
				s.Delete("from Customer");
				s.Flush();
			}
		}

		[Test]
		public async Task OrderByUsesLeftJoinAsync()
		{
			ISession s = OpenSession();
			try
			{
				Customer c1 = new Customer {Name = "Allen"};
				await (s.SaveAsync(c1, CancellationToken.None));
				Customer c2 = new Customer {Name = "Bob"};
				await (s.SaveAsync(c2, CancellationToken.None));
				Customer c3 = new Customer {Name = "Charlie"};
				await (s.SaveAsync(c3, CancellationToken.None));

				await (s.SaveAsync(new Order {Customer = c1}, CancellationToken.None));
				await (s.SaveAsync(new Order {Customer = c3}, CancellationToken.None));
				await (s.SaveAsync(new Order {Customer = c2}, CancellationToken.None));
				await (s.SaveAsync(new Order(), CancellationToken.None));

				await (s.FlushAsync(CancellationToken.None));
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				var orders = await (s.Query<Order>().OrderBy(o => o.Customer.Name).ToListAsync(CancellationToken.None));
				Assert.AreEqual(4, orders.Count);
				if (orders[0].Customer == null)
				{
					CollectionAssert.AreEqual(new[] {"Allen", "Bob", "Charlie"}, orders.Skip(1).Select(o => o.Customer.Name).ToArray());
				}
				else
				{
					CollectionAssert.AreEqual(new[] { "Allen", "Bob", "Charlie" }, orders.Take(3).Select(o => o.Customer.Name).ToArray());
				}
			}
			finally
			{
				s.Close();
			}
		}
	}
}
