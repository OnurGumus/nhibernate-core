#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Connection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1985
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			if (0 == await (ExecuteStatementAsync("INSERT INTO DomainClass (Id, Label) VALUES (1, 'TEST record');")))
			{
				throw new ApplicationException("Insertion of test record failed.");
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			await (ExecuteStatementAsync("DELETE FROM DomainClass WHERE Id=1;"));
		}

		[Test]
		[Ignore("It is valid to be delete immutable entities")]
		public async Task AttemptToDeleteImmutableObjectShouldThrowAsync()
		{
			using (ISession session = OpenSession())
			{
				Assert.ThrowsAsync<HibernateException>(async () =>
				{
					using (ITransaction trans = session.BeginTransaction())
					{
						var entity = await (session.GetAsync<DomainClass>(1));
						await (session.DeleteAsync(entity));
						await (trans.CommitAsync()); // This used to throw...
					}
				}

				);
			}

			using (IConnectionProvider prov = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				DbConnection conn = await (prov.GetConnectionAsync());
				try
				{
					using (DbCommand comm = conn.CreateCommand())
					{
						comm.CommandText = "SELECT Id FROM DomainClass WHERE Id=1 AND Label='TEST record'";
						object result = await (comm.ExecuteScalarAsync());
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
						var entity = await (session.GetAsync<DomainClass>(1));
						await (session.DeleteAsync(entity));
						await (trans.CommitAsync());
					}
				}

				);
			}

			using (IConnectionProvider prov = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				DbConnection conn = await (prov.GetConnectionAsync());
				try
				{
					using (DbCommand comm = conn.CreateCommand())
					{
						comm.CommandText = "SELECT Id FROM DomainClass WHERE Id=1 AND Label='TEST record'";
						object result = await (comm.ExecuteScalarAsync());
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
#endif
