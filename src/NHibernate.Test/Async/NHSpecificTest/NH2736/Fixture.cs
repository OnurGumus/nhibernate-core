#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2736
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestHqlParametersWithTakeAsync()
		{
			using (var session = Sfi.OpenSession())
				using (session.BeginTransaction())
				{
					var query = session.CreateQuery("select o.Id, i.Id from SalesOrder o left join o.Items i with i.Quantity = :pQuantity take :pTake");
					query.SetParameter("pQuantity", 1);
					query.SetParameter("pTake", 2);
					var result = await (query.ListAsync());
					Assert.That(result.Count, Is.EqualTo(2));
				}
		}
	}
}
#endif
