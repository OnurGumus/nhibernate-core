#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Connection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1985
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		[Ignore("It is valid to be delete immutable entities")]
		public async Task AttemptToDeleteImmutableObjectShouldThrowAsync()
		{
			using (ISession session = OpenSession())
			{
				Assert.Throws<HibernateException>(() =>
				{
					using (ITransaction trans = session.BeginTransaction())
					{
						var entity = session.Get<DomainClass>(1);
						session.Delete(entity);
						trans.Commit(); // This used to throw...
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
				Assert.DoesNotThrow(() =>
				{
					using (ITransaction trans = session.BeginTransaction())
					{
						var entity = session.Get<DomainClass>(1);
						session.Delete(entity);
						trans.Commit();
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
