#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1747
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinTraversalTestAsync : BugTestCaseAsync
	{
		[Test]
		public void PaymentTableNotContainsFk()
		{
			var cm = cfg.GetClassMapping(typeof (Payment));
			bool containsFK = false;
			foreach (var column in cm.Table.ColumnIterator)
			{
				if (column.Name.Equals("PaymentBatchFK_ID"))
					containsFK = true;
			}

			Assert.That(containsFK, Is.False);
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				var payment = new Payment{Amount = 5m, Id = 1};
				var paymentBatch = new PaymentBatch{Id = 3};
				paymentBatch.AddPayment(payment);
				await (session.SaveAsync(paymentBatch));
				await (session.SaveAsync(payment));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				var hql = "from System.Object";
				await (session.DeleteAsync(hql));
				await (session.FlushAsync());
			}
		}

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
