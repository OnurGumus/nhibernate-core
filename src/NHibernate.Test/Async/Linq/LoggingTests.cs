#if NET_4_5
using System.Linq;
using NHibernate.Cfg;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LoggingTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task PageBetweenProjectionsAsync()
		{
			using (var spy = new LogSpy("NHibernate.Linq"))
			{
				var subquery = db.Products.Where(p => p.ProductId > 5);
				var list = await (db.Products.Where(p => subquery.Contains(p)).Skip(5).Take(10).ToListAsync());
				var logtext = spy.GetWholeLog();
				const string expected = "Expression (partially evaluated): value(NHibernate.Linq.NhQueryable`1[NHibernate.DomainModel.Northwind.Entities.Product]).Where(p => value(NHibernate.Linq.NhQueryable`1[NHibernate.DomainModel.Northwind.Entities.Product]).Where(p => (p.ProductId > 5)).Contains(p)).Skip(5).Take(10)";
				Assert.That(logtext, Is.StringContaining(expected));
			}
		}

		[Test]
		public async Task CanLogLinqExpressionWithoutInitializingContainedProxyAsync()
		{
			var productId = await (db.Products.Select(p => p.ProductId).FirstAsync());
			using (var logspy = new LogSpy("NHibernate.Linq"))
			{
				var productProxy = await (session.LoadAsync<Product>(productId));
				Assert.That(NHibernateUtil.IsInitialized(productProxy), Is.False);
				var result =
					from product in db.Products
					where product == productProxy
					select product;
				Assert.That(await (result.CountAsync()), Is.EqualTo(1));
				// Verify that the expected logging did happen.
				var actualLog = logspy.GetWholeLog();
				const string expectedLog = "Expression (partially evaluated): value(NHibernate.Linq.NhQueryable`1[NHibernate.DomainModel.Northwind.Entities.Product])" + ".Where(product => (product == Product#1)).Count()";
				Assert.That(actualLog, Is.StringContaining(expectedLog));
				// And verify that the proxy in the expression wasn't initialized.
				Assert.That(NHibernateUtil.IsInitialized(productProxy), Is.False, "ERROR: We expected the proxy to NOT be initialized.");
			}
		}
	}
}
#endif
