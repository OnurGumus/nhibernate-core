#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH555
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			using (ISession s = OpenSession())
			{
				Customer c = new Customer();
				c.Name = "TestCustomer";
				await (s.SaveAsync(c));
				Article art = new Article();
				art.Name = "TheArticle1";
				art.Price = 10.5M;
				await (s.SaveAsync(art));
				Order o = c.CreateNewOrder();
				OrderLine ol = o.CreateNewOrderLine();
				ol.SetArticle(art);
				ol.NumberOfItems = 5;
				o.AddOrderLine(ol);
				await (s.SaveAsync(o));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				string hql = "select sum (ol.ArticlePrice * ol.NumberOfItems) " + "from Order o, OrderLine ol, Customer c " + "where c.Id = :custId and o.OrderDate >= :orderDate";
				IQuery q = s.CreateQuery(hql);
				q.SetInt32("custId", 1);
				q.SetDateTime("orderDate", DateTime.Now.AddMonths(-3));
				Assert.AreEqual(52.5m, (decimal)await (q.UniqueResultAsync()));
			}

			using (ISession s = OpenSession())
			{
				Order o = (Order)await (s.CreateQuery("from Order").UniqueResultAsync());
				OrderLine ol = (OrderLine)o.OrderLines[0];
				await (s.DeleteAsync(ol));
				o.OrderLines.RemoveAt(0);
				await (s.DeleteAsync(o));
				await (s.DeleteAsync(o.OwningCustomer));
				await (s.DeleteAsync("from Article"));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
