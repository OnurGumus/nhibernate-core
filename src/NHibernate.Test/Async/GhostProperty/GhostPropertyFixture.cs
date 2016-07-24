#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Tuple.Entity;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.GhostProperty
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GhostPropertyFixtureAsync : TestCaseAsync
	{
		private string log;
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"GhostProperty.Mappings.hbm.xml"};
			}
		}

		protected override Task ConfigureAsync(Cfg.Configuration configuration)
		{
			try
			{
				configuration.DataBaseIntegration(x => x.LogFormattedSql = false);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var wireTransfer = new WireTransfer{Id = 1};
					await (s.PersistAsync(wireTransfer));
					await (s.PersistAsync(new Order{Id = 1, Payment = wireTransfer}));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Order"));
					await (s.DeleteAsync("from Payment"));
					await (tx.CommitAsync());
				}
		}

		protected override void BuildSessionFactory()
		{
			using (var logSpy = new LogSpy(typeof (EntityMetamodel)))
			{
				base.BuildSessionFactory();
				log = logSpy.GetWholeLog();
			}
		}

		[Test]
		public void ShouldGenerateErrorForNonAutoPropGhostProp()
		{
			Assert.IsTrue(log.Contains("NHibernate.Test.GhostProperty.Order.Payment is not an auto property, which may result in uninitialized property access"));
		}

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
				Assert.IsFalse(await (NHibernateUtil.IsPropertyInitializedAsync(order, "Payment")));
			}
		}

		[Test]
		public async Task GhostPropertyMaintainIdentityMapAsync()
		{
			using (ISession s = OpenSession())
			{
				var order = await (s.GetAsync<Order>(1));
				Assert.AreSame(order.Payment, await (s.LoadAsync<Payment>(1)));
			}
		}

		[Test, Ignore("This shows an expected edge case")]
		public async Task GhostPropertyMaintainIdentityMapUsingGetAsync()
		{
			using (ISession s = OpenSession())
			{
				var payment = await (s.LoadAsync<Payment>(1));
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

				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(order, "Payment")), Is.False);
				// trigger on-access lazy load 
				var x = order.Payment;
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(order, "Payment")), Is.True);
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

				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(order, "NoLazyProperty")), Is.True);
				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(order, "ALazyProperty")), Is.False);
				using (var ls = new SqlLogSpy())
				{
					var x = order.ALazyProperty;
					var logMessage = ls.GetWholeLog();
					Assert.That(logMessage, Is.StringContaining("ALazyProperty"));
				}

				Assert.That(await (NHibernateUtil.IsPropertyInitializedAsync(order, "ALazyProperty")), Is.True);
			}
		}
	}
}
#endif
