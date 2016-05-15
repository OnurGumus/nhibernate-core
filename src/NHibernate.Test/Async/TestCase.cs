#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Reflection;
using log4net;
using log4net.Config;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using NUnit.Framework;
using NHibernate.Hql.Ast.ANTLR;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class TestCase
	{
		/// <summary>
		/// Checks that the test case cleans up after itself. This method
		/// is not overridable, but it calls <see cref = "OnTearDown"/> which is.
		/// </summary>
		[TearDown]
		public async Task TearDownAsync()
		{
			OnTearDown();
			bool wasClosed = CheckSessionWasClosed();
			bool wasCleaned = await (CheckDatabaseWasCleanedAsync());
			bool wereConnectionsClosed = CheckConnectionsWereClosed();
			bool fail = !wasClosed || !wasCleaned || !wereConnectionsClosed;
			if (fail)
			{
				Assert.Fail("Test didn't clean up after itself. session closed: " + wasClosed + " database cleaned: " + wasCleaned + " connection closed: " + wereConnectionsClosed);
			}
		}

		protected virtual async Task<bool> CheckDatabaseWasCleanedAsync()
		{
			if (sessions.GetAllClassMetadata().Count == 0)
			{
				// Return early in the case of no mappings, also avoiding
				// a warning when executing the HQL below.
				return true;
			}

			bool empty;
			using (ISession s = sessions.OpenSession())
			{
				IList objects = await (s.CreateQuery("from System.Object o").ListAsync());
				empty = objects.Count == 0;
			}

			if (!empty)
			{
				log.Error("Test case didn't clean up the database after itself, re-creating the schema");
				await (DropSchemaAsync());
				await (CreateSchemaAsync());
			}

			return empty;
		}

		protected virtual Task CreateSchemaAsync()
		{
			return new SchemaExport(cfg).CreateAsync(OutputDdl, true);
		}

		protected virtual Task DropSchemaAsync()
		{
			return new SchemaExport(cfg).DropAsync(OutputDdl, true);
		}

		private async Task CleanupAsync()
		{
			if (sessions != null)
			{
				await (sessions.CloseAsync());
			}

			sessions = null;
			connectionProvider = null;
			lastOpenedSession = null;
			cfg = null;
		}

		public async Task<int> ExecuteStatementAsync(string sql)
		{
			if (cfg == null)
			{
				cfg = TestConfigurationHelper.GetDefaultConfiguration();
			}

			using (IConnectionProvider prov = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				DbConnection conn = await (prov.GetConnectionAsync());
				try
				{
					using (DbTransaction tran = conn.BeginTransaction())
						using (DbCommand comm = conn.CreateCommand())
						{
							comm.CommandText = sql;
							comm.Transaction = tran;
							comm.CommandType = CommandType.Text;
							int result = await (comm.ExecuteNonQueryAsync());
							tran.Commit();
							return result;
						}
				}
				finally
				{
					prov.CloseConnection(conn);
				}
			}
		}

		public async Task<int> ExecuteStatementAsync(ISession session, ITransaction transaction, string sql)
		{
			using (DbCommand cmd = session.Connection.CreateCommand())
			{
				cmd.CommandText = sql;
				if (transaction != null)
					transaction.Enlist(cmd);
				return await (cmd.ExecuteNonQueryAsync());
			}
		}
	}
}
#endif
