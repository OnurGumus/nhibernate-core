#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1343
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProductFixture : BugTestCase
	{
		[Test]
		public async Task ProductQueryPassesParsingButFailsAsync()
		{
			Product product1 = new Product("product1");
			OrderLine orderLine = new OrderLine("1", product1);
			using (ISession session = OpenSession())
			{
				await (session.SaveAsync(product1));
				await (session.SaveAsync(orderLine));
				await (session.FlushAsync());
				IQuery query = session.GetNamedQuery("GetLinesForProduct");
				query.SetParameter("product", product1);
				IList<OrderLine> list = query.List<OrderLine>();
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task ProductQueryPassesAndExecutesRightIfPuttingAliasAsync()
		{
			Product product1 = new Product("product1");
			OrderLine orderLine = new OrderLine("1", product1);
			using (ISession session = OpenSession())
			{
				await (session.SaveAsync(product1));
				await (session.SaveAsync(orderLine));
				await (session.FlushAsync());
				IQuery query = session.GetNamedQuery("GetLinesForProductWithAlias");
				query.SetParameter("product", product1);
				IList<OrderLine> list = query.List<OrderLine>();
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
