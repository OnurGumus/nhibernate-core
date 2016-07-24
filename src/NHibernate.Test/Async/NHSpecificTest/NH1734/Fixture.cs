#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1734
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					var product = new Product{Amount = 3, Price = 43.2};
					var product2 = new Product{Amount = 3, Price = 43.2};
					await (session.SaveAsync(product));
					await (session.SaveAsync(product2));
					await (tran.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Product"));
					await (tran.CommitAsync());
				}
		}

		[Test]
		public async Task ReturnsApropriateTypeWhenSumUsedWithSomeFormulaAsync()
		{
			using (var session = this.OpenSession())
				using (var tran = session.BeginTransaction())
				{
					double delta = 0.0000000000001;
					var query = session.CreateQuery("select sum(Amount*Price) from Product");
					var result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (double)));
					Assert.AreEqual(43.2 * 3 * 2, (double)result, delta);
					query = session.CreateQuery("select sum(Price*Amount) from Product");
					result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (double)));
					Assert.AreEqual(43.2 * 3 * 2, (double)result, delta);
					query = session.CreateQuery("select sum(Price) from Product");
					result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (double)));
					Assert.AreEqual(43.2 * 2, (double)result, delta);
					query = session.CreateQuery("select sum(Amount) from Product");
					result = await (query.UniqueResultAsync());
					Assert.That(result, Is.InstanceOf(typeof (Int64)));
					Assert.That(result, Is.EqualTo(6));
				}
		}
	}
}
#endif
