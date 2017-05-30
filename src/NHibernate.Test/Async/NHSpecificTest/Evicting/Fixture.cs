﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Stat;

namespace NHibernate.Test.NHSpecificTest.Evicting
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "Evicting"; }
		}

		protected override void OnSetUp()
		{
			base.OnSetUp();
			using (var session = sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Save(new Employee
				{
                    Id = 1,
					FirstName = "a",
					LastName = "b"
				});
				tx.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Delete(session.Load<Employee>(1));

				tx.Commit();
			}
			base.OnTearDown();
		}


		[Test]
		public async Task Can_evict_entity_from_sessionAsync()
		{
			using (var session = sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var employee = await (session.LoadAsync<Employee>(1, CancellationToken.None));
				Assert.IsTrue(session.Contains(employee));

				await (session.EvictAsync(employee, CancellationToken.None));

				Assert.IsFalse(session.Contains(employee));

				await (tx.CommitAsync(CancellationToken.None));
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

				await (session.EvictAsync(employee, CancellationToken.None));

				Assert.IsFalse(session.Contains(employee));

				await (tx.CommitAsync(CancellationToken.None));
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
					var employee = await (session2.LoadAsync<Employee>(1, CancellationToken.None));
					Assert.IsFalse(session1.Contains(employee));
					Assert.IsTrue(session2.Contains(employee));

					await (session1.EvictAsync(employee, CancellationToken.None));

					Assert.IsFalse(session1.Contains(employee));

					Assert.IsTrue(session2.Contains(employee));

					await (tx2.CommitAsync(CancellationToken.None));
				}
				
				await (tx1.CommitAsync(CancellationToken.None));
			}
		}
	
	}
}
