#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH296
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH296";
			}
		}

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
				stock = (Stock)await (s.GetAsync(typeof (Stock), stock.ProductPK));
				Assert.IsNotNull(stock);
			}

			using (ISession s = OpenSession())
			{
				stock = (Stock)await (s.GetAsync(typeof (Product), stock.ProductPK));
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
