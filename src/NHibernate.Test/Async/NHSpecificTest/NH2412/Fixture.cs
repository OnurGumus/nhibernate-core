#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2412
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.DeleteAsync("from Order"));
				await (s.DeleteAsync("from Customer"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task OrderByUsesLeftJoinAsync()
		{
			ISession s = OpenSession();
			try
			{
				Customer c1 = new Customer{Name = "Allen"};
				await (s.SaveAsync(c1));
				Customer c2 = new Customer{Name = "Bob"};
				await (s.SaveAsync(c2));
				Customer c3 = new Customer{Name = "Charlie"};
				await (s.SaveAsync(c3));
				await (s.SaveAsync(new Order{Customer = c1}));
				await (s.SaveAsync(new Order{Customer = c3}));
				await (s.SaveAsync(new Order{Customer = c2}));
				await (s.SaveAsync(new Order()));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				var orders = s.Query<Order>().OrderBy(o => o.Customer.Name).ToList();
				Assert.AreEqual(4, orders.Count);
				if (orders[0].Customer == null)
				{
					CollectionAssert.AreEqual(new[]{"Allen", "Bob", "Charlie"}, orders.Skip(1).Select(o => o.Customer.Name).ToArray());
				}
				else
				{
					CollectionAssert.AreEqual(new[]{"Allen", "Bob", "Charlie"}, orders.Take(3).Select(o => o.Customer.Name).ToArray());
				}
			}
			finally
			{
				s.Close();
			}
		}
	}
}
#endif
