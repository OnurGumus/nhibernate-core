﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Data;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NHibernate.Test.NHSpecificTest.NH1553.MsSQL
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// Test fixture for NH1553, which checks update conflict detection together with snapshot isolation transaction isolation level.
	/// </summary>
	[TestFixture]
	public class SnapshotIsolationUpdateConflictTestAsync : BugTestCase
	{
		private Person person;

		public override string BugNumber
		{
			get { return "NH1553.MsSQL"; }
		}

		private ITransaction BeginTransaction(ISession session)
		{
			return session.BeginTransaction(IsolationLevel.Snapshot);
		}

		private async Task<Person> LoadPersonAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tr = BeginTransaction(session))
				{
					var p = await (session.GetAsync<Person>(person.Id, cancellationToken));
					await (tr.CommitAsync(cancellationToken));
					return p;
				}
			}
		}

		private async Task SavePersonAsync(Person p, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tr = BeginTransaction(session))
				{
					await (session.SaveOrUpdateAsync(p, cancellationToken));
					await (session.FlushAsync(cancellationToken));
					await (tr.CommitAsync(cancellationToken));
				}
			}
		}

		private void SavePerson(Person p)
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tr = BeginTransaction(session))
				{
					session.SaveOrUpdate(p);
					session.Flush();
					tr.Commit();
				}
			}
		}

		/// <summary>
		/// Tests, that NHibernate detects the update conflict and returns a StaleObjectStateException as expected.
		/// This behaviour is part of the standard NHibernate feature set.
		/// </summary>
		[Test]
		public async Task UpdateConflictDetectedByNHAsync()
		{
			Person p1 = await (LoadPersonAsync());
			Person p2 = await (LoadPersonAsync());

			p1.IdentificationNumber++;
			p2.IdentificationNumber += 2;

			await (SavePersonAsync(p1));
			Assert.That(p1.Version, Is.EqualTo(person.Version + 1));

			var expectedException = Sfi.Settings.IsBatchVersionedDataEnabled
				? (IResolveConstraint) Throws.InstanceOf<StaleStateException>()
				: Throws.InstanceOf<StaleObjectStateException>()
				        .And.Property("EntityName").EqualTo(typeof(Person).FullName)
				        .And.Property("Identifier").EqualTo(p2.Id);

			Assert.That(() => SavePersonAsync(p2), expectedException);
		}

		/// <summary>
		/// Tests, that the extension provided wraps the returned SQL Exception inside a StaleObjectStateException,
		/// if the SQL Server detects an update conflict in snapshot isolation.
		/// </summary>
		[Test]
		public async Task UpdateConflictDetectedBySQLServerAsync()
		{
			Person p1 = await (LoadPersonAsync());

			p1.IdentificationNumber++;

			using (var session1 = OpenSession())
			using (var tr1 = BeginTransaction(session1))
			{
				await (session1.SaveOrUpdateAsync(p1));
				await (session1.FlushAsync());

				using (var session2 = OpenSession())
				using (var tr2 = BeginTransaction(session2))
				{
					var p2 = await (session2.GetAsync<Person>(person.Id));
					p2.IdentificationNumber += 2;

					await (tr1.CommitAsync());
					Assert.That(p1.Version, Is.EqualTo(person.Version + 1));

					await (session2.SaveOrUpdateAsync(p2));

					var expectedException = Sfi.Settings.IsBatchVersionedDataEnabled
						? (IConstraint) Throws.InstanceOf<StaleStateException>()
						: Throws.InstanceOf<StaleObjectStateException>()
						        .And.Property("EntityName").EqualTo(typeof(Person).FullName)
						        .And.Property("Identifier").EqualTo(p2.Id);

					Assert.That(
						async () =>
						{
							await (session2.FlushAsync());
							await (tr2.CommitAsync());
						},
						expectedException);
				}
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		private async Task SetAllowSnapshotIsolationAsync(bool on, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (ISession session = OpenSession())
			{
				var command = session.Connection.CreateCommand();
				command.CommandText = "ALTER DATABASE " + session.Connection.Database + " set allow_snapshot_isolation "
				                      + (on ? "on" : "off");
				await (command.ExecuteNonQueryAsync(cancellationToken));
			}
		}

		private void SetAllowSnapshotIsolation(bool on)
		{
			using (ISession session = OpenSession())
			{
				var command = session.Connection.CreateCommand();
				command.CommandText = "ALTER DATABASE " + session.Connection.Database + " set allow_snapshot_isolation "
				                      + (on ? "on" : "off");
				command.ExecuteNonQuery();
			}
		}

		protected override void OnSetUp()
		{
			base.OnSetUp();

			SetAllowSnapshotIsolation(true);

			person = new Person();
			person.IdentificationNumber = 123;
			SavePerson(person);
		}

		protected override void OnTearDown()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tr = session.BeginTransaction(IsolationLevel.Serializable))
				{
					string hql = "from Person";
					session.Delete(hql);
					session.Flush();
					tr.Commit();
				}
			}

			SetAllowSnapshotIsolation(false);

			base.OnTearDown();
		}

		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Environment.SqlExceptionConverter,
			                          typeof (SQLUpdateConflictToStaleStateExceptionConverter).AssemblyQualifiedName);
		}
	}
}