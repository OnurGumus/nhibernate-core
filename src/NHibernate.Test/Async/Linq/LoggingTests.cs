#if NET_4_5
using System.Linq;
using NHibernate.Cfg;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LoggingTests : LinqTestCase
	{
		[Test]
		public async Task CanLogLinqExpressionWithoutInitializingContainedProxyAsync()
		{
			var productId = db.Products.Select(p => p.ProductId).First();
			using (var logspy = new LogSpy("NHibernate.Linq"))
			{
				var productProxy = await (session.LoadAsync<Product>(productId));
				Assert.That(NHibernateUtil.IsInitialized(productProxy), Is.False);
				var result =
					from product in db.Products
					where product == productProxy
					select product;
				Assert.That(result.Count(), Is.EqualTo(1));
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
