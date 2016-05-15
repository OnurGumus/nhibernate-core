#if NET_4_5
using System.Linq;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2296
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
