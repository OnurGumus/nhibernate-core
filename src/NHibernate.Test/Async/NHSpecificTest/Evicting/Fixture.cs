#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Stat;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Evicting
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task Can_evict_entity_from_sessionAsync()
		{
			using (var session = sessions.OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var employee = session.Load<Employee>(1);
					Assert.IsTrue(session.Contains(employee));
					session.Evict(employee);
					Assert.IsFalse(session.Contains(employee));
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
					Assert.IsFalse(session.Contains(employee));
					session.Evict(employee);
					Assert.IsFalse(session.Contains(employee));
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
							var employee = session2.Load<Employee>(1);
							Assert.IsFalse(session1.Contains(employee));
							Assert.IsTrue(session2.Contains(employee));
							session1.Evict(employee);
							Assert.IsFalse(session1.Contains(employee));
							Assert.IsTrue(session2.Contains(employee));
							await (tx2.CommitAsync());
						}

					await (tx1.CommitAsync());
				}
		}
	}
}
#endif
