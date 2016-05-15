#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1747
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinTraversalTest : BugTestCase
	{
		[Test]
		public async Task TraversingBagToJoinChildElementShouldWorkAsync()
		{
			using (ISession session = OpenSession())
			{
				var paymentBatch = await (session.GetAsync<PaymentBatch>(3));
				Assert.AreEqual(1, paymentBatch.Payments.Count);
			}
		}
	}
}
#endif
