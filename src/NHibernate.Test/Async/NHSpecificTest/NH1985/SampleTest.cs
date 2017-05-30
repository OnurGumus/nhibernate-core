﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data.Common;
using NHibernate.Connection;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1985
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class SampleTestAsync : BugTestCase
	{
		protected override void OnSetUp()
		{
			if (0 == ExecuteStatement("INSERT INTO DomainClass (Id, Label) VALUES (1, 'TEST record');"))
			{
				Assert.Fail("Insertion of test record failed.");
			}
		}

		protected override void OnTearDown()
		{
			base.OnTearDown();

			ExecuteStatement("DELETE FROM DomainClass WHERE Id=1;");
		}

		[Test]
		[Ignore("It is valid to delete immutable entities")]
		public async Task AttemptToDeleteImmutableObjectShouldThrowAsync()
		{
			using (ISession session = OpenSession())
			{
				Assert.ThrowsAsync<HibernateException>(async () =>
					{
						using (ITransaction trans = session.BeginTransaction())
						{
							var entity = await (session.GetAsync<DomainClass>(1, CancellationToken.None));
							await (session.DeleteAsync(entity, CancellationToken.None));

							await (trans.CommitAsync(CancellationToken.None)); // This used to throw...
						}
					});
			}

			using (IConnectionProvider prov = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				var conn = await (prov.GetConnectionAsync(CancellationToken.None));

				try
				{
					using (var comm = conn.CreateCommand())
					{
						comm.CommandText = "SELECT Id FROM DomainClass WHERE Id=1 AND Label='TEST record'";
						object result = await (comm.ExecuteScalarAsync(CancellationToken.None));

						Assert.That(result != null, "Immutable object has been deleted!");
					}
				}
				finally
				{
					prov.CloseConnection(conn);
				}
			}
		}
		
		[Test]
		public async Task AllowDeletionOfImmutableObjectAsync()
		{
			using (ISession session = OpenSession())
			{
				Assert.DoesNotThrowAsync(async () =>
					{
						using (ITransaction trans = session.BeginTransaction())
						{
							var entity = await (session.GetAsync<DomainClass>(1, CancellationToken.None));
							await (session.DeleteAsync(entity, CancellationToken.None));

							await (trans.CommitAsync(CancellationToken.None));
						}
					});
			}

			using (IConnectionProvider prov = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				var conn = await (prov.GetConnectionAsync(CancellationToken.None));

				try
				{
					using (var comm = conn.CreateCommand())
					{
						comm.CommandText = "SELECT Id FROM DomainClass WHERE Id=1 AND Label='TEST record'";
						object result = await (comm.ExecuteScalarAsync(CancellationToken.None));

						Assert.That(result == null, "Immutable object has not been deleted!");
					}
				}
				finally
				{
					prov.CloseConnection(conn);
				}
			}
		}
	}
}
