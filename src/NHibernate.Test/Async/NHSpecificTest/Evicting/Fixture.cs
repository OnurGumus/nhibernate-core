#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Stat;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Evicting
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "Evicting";
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = sessions.OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.SaveAsync(new Employee{Id = 1, FirstName = "a", LastName = "b"}));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = sessions.OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync(await (session.LoadAsync<Employee>(1))));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task Can_evict_entity_from_sessionAsync()
		{
			using (var session = sessions.OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var employee = await (session.LoadAsync<Employee>(1));
					Assert.IsTrue(await (session.ContainsAsync(employee)));
					await (session.EvictAsync(employee));
					Assert.IsFalse(await (session.ContainsAsync(employee)));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task Can_evict_non_persistent_objectAsync()
		{
			using (var session = sessions.OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var employee = new Employee();
					Assert.IsFalse(await (session.ContainsAsync(employee)));
					await (session.EvictAsync(employee));
					Assert.IsFalse(await (session.ContainsAsync(employee)));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task Can_evict_when_trying_to_evict_entity_from_another_sessionAsync()
		{
			using (var session1 = sessions.OpenSession())
				using (var tx1 = session1.BeginTransaction())
				{
					using (var session2 = sessions.OpenSession())
						using (var tx2 = session2.BeginTransaction())
						{
							var employee = await (session2.LoadAsync<Employee>(1));
							Assert.IsFalse(await (session1.ContainsAsync(employee)));
							Assert.IsTrue(await (session2.ContainsAsync(employee)));
							await (session1.EvictAsync(employee));
							Assert.IsFalse(await (session1.ContainsAsync(employee)));
							Assert.IsTrue(await (session2.ContainsAsync(employee)));
							await (tx2.CommitAsync());
						}

					await (tx1.CommitAsync());
				}
		}
	}
}
#endif
