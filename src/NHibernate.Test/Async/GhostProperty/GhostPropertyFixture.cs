#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Tuple.Entity;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.GhostProperty
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GhostPropertyFixture : TestCase
	{
		[Test]
		public async Task CanGetActualValueFromLazyManyToOneAsync()
		{
			using (ISession s = OpenSession())
			{
				var order = await (s.GetAsync<Order>(1));
				Assert.IsTrue(order.Payment is WireTransfer);
			}
		}

		[Test]
		public async Task WillNotLoadGhostPropertyByDefaultAsync()
		{
			using (ISession s = OpenSession())
			{
				var order = await (s.GetAsync<Order>(1));
				Assert.IsFalse(NHibernateUtil.IsPropertyInitialized(order, "Payment"));
			}
		}

		[Test]
		public async Task GhostPropertyMaintainIdentityMapAsync()
		{
			using (ISession s = OpenSession())
			{
				var order = await (s.GetAsync<Order>(1));
				Assert.AreSame(order.Payment, s.Load<Payment>(1));
			}
		}

		[Test, Ignore("This shows an expected edge case")]
		public async Task GhostPropertyMaintainIdentityMapUsingGetAsync()
		{
			using (ISession s = OpenSession())
			{
				var payment = s.Load<Payment>(1);
				var order = await (s.GetAsync<Order>(1));
				Assert.AreSame(order.Payment, payment);
			}
		}

		[Test]
		public async Task WillLoadGhostAssociationOnAccessAsync()
		{
			// NH-2498
			using (ISession s = OpenSession())
			{
				Order order;
				using (var ls = new SqlLogSpy())
				{
					order = await (s.GetAsync<Order>(1));
					var logMessage = ls.GetWholeLog();
					Assert.That(logMessage, Is.Not.StringContaining("FROM Payment"));
				}

				Assert.That(NHibernateUtil.IsPropertyInitialized(order, "Payment"), Is.False);
				// trigger on-access lazy load 
				var x = order.Payment;
				Assert.That(NHibernateUtil.IsPropertyInitialized(order, "Payment"), Is.True);
			}
		}

		[Test]
		public async Task WhenGetThenLoadOnlyNoLazyPlainPropertiesAsync()
		{
			using (ISession s = OpenSession())
			{
				Order order;
				using (var ls = new SqlLogSpy())
				{
					order = await (s.GetAsync<Order>(1));
					var logMessage = ls.GetWholeLog();
					Assert.That(logMessage, Is.Not.StringContaining("ALazyProperty"));
					Assert.That(logMessage, Is.StringContaining("NoLazyProperty"));
				}

				Assert.That(NHibernateUtil.IsPropertyInitialized(order, "NoLazyProperty"), Is.True);
				Assert.That(NHibernateUtil.IsPropertyInitialized(order, "ALazyProperty"), Is.False);
				using (var ls = new SqlLogSpy())
				{
					var x = order.ALazyProperty;
					var logMessage = ls.GetWholeLog();
					Assert.That(logMessage, Is.StringContaining("ALazyProperty"));
				}

				Assert.That(NHibernateUtil.IsPropertyInitialized(order, "ALazyProperty"), Is.True);
			}
		}
	}
}
#endif
