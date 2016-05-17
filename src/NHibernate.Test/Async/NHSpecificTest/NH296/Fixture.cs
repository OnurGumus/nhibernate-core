#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH296
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CRUDAsync()
		{
			Stock stock = new Stock();
			stock.ProductPK = new ProductPK();
			stock.ProductPK.Number = 1;
			stock.ProductPK.Type = 1;
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(stock));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				stock = (Stock)s.Get(typeof (Stock), stock.ProductPK);
				Assert.IsNotNull(stock);
			}

			using (ISession s = OpenSession())
			{
				stock = (Stock)s.Get(typeof (Product), stock.ProductPK);
				Assert.IsNotNull(stock);
				stock.Property = 10;
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(stock));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
