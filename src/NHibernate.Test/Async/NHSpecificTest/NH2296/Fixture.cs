#if NET_4_5
using System.Linq;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2296
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return !(factory.ConnectionProvider.Driver is OracleManagedDataClientDriver);
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var o = new Order()
					{AccountName = "Acct1"};
					o.Products.Add(new Product()
					{StatusReason = "Success", Order = o});
					o.Products.Add(new Product()
					{StatusReason = "Failure", Order = o});
					await (s.SaveAsync(o));
					o = new Order()
					{AccountName = "Acct2"};
					await (s.SaveAsync(o));
					o = new Order()
					{AccountName = "Acct3"};
					o.Products.Add(new Product()
					{StatusReason = "Success", Order = o});
					o.Products.Add(new Product()
					{StatusReason = "Failure", Order = o});
					await (s.SaveAsync(o));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Product"));
					await (s.DeleteAsync("from Order"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task TestAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var orders = await (s.CreateQuery("select o from Order o").SetMaxResults(2).ListAsync<Order>());
					// trigger lazy-loading of products, using subselect fetch. 
					string sr = orders[0].Products[0].StatusReason;
					// count of entities we want:
					int ourEntities = orders.Count + orders.Sum(o => o.Products.Count);
					Assert.That(s.Statistics.EntityCount, Is.EqualTo(ourEntities));
				}
		}
	}
}
#endif
