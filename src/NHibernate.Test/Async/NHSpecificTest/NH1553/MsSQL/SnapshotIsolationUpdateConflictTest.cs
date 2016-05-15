#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1553.MsSQL
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SnapshotIsolationUpdateConflictTest : BugTestCase
	{
		private async Task<Person> LoadPersonAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tr = BeginTransaction(session))
				{
					var p = await (session.GetAsync<Person>(person.Id));
					await (tr.CommitAsync());
					return p;
				}
			}
		}

		private async Task SavePersonAsync(Person p)
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tr = BeginTransaction(session))
				{
					await (session.SaveOrUpdateAsync(p));
					await (session.FlushAsync());
					await (tr.CommitAsync());
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
			Assert.AreEqual(person.Version + 1, p1.Version);
			try
			{
				await (SavePersonAsync(p2));
				Assert.Fail("Expecting stale object state exception");
			}
			catch (StaleObjectStateException sose)
			{
				Assert.AreEqual(typeof (Person).FullName, sose.EntityName);
				Assert.AreEqual(p2.Id, sose.Identifier);
			// as expected.
			}
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
			using (ISession session1 = OpenSession())
			{
				using (ITransaction tr1 = BeginTransaction(session1))
				{
					await (session1.SaveOrUpdateAsync(p1));
					await (session1.FlushAsync());
					using (ISession session2 = OpenSession())
					{
						using (ITransaction tr2 = BeginTransaction(session2))
						{
							var p2 = await (session2.GetAsync<Person>(person.Id));
							p2.IdentificationNumber += 2;
							await (tr1.CommitAsync());
							Assert.AreEqual(person.Version + 1, p1.Version);
							try
							{
								await (session2.SaveOrUpdateAsync(p2));
								await (session2.FlushAsync());
								await (tr2.CommitAsync());
								Assert.Fail("StaleObjectStateException expected");
							}
							catch (StaleObjectStateException sose)
							{
								Assert.AreEqual(typeof (Person).FullName, sose.EntityName);
								Assert.AreEqual(p2.Id, sose.Identifier);
							// as expected
							}
						}
					}
				}
			}
		}

		private async Task SetAllowSnapshotIsolationAsync(bool on)
		{
			using (ISession session = OpenSession())
			{
				DbCommand command = session.Connection.CreateCommand();
				command.CommandText = "ALTER DATABASE " + session.Connection.Database + " set allow_snapshot_isolation " + (on ? "on" : "off");
				await (command.ExecuteNonQueryAsync());
			}
		}
	}
}
#endif
