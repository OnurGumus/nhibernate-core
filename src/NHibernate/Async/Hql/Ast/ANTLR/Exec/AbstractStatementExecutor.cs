#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Action;
using NHibernate.AdoNet.Util;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Event;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Transaction;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Hql.Ast.ANTLR.Exec
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractStatementExecutor : IStatementExecutor
	{
		public abstract Task<int> ExecuteAsync(QueryParameters parameters, ISessionImplementor session);
		protected virtual async Task CreateTemporaryTableIfNecessaryAsync(IQueryable persister, ISessionImplementor session)
		{
			// Don't really know all the codes required to adequately decipher returned ADO exceptions here.
			// simply allow the failure to be eaten and the subsequent insert-selects/deletes should fail
			IIsolatedWork work = new TmpIdTableCreationIsolatedWork(persister, log, session);
			if (ShouldIsolateTemporaryTableDDL())
			{
				if (Factory.Settings.IsDataDefinitionInTransactionSupported)
				{
					await (Isolater.DoIsolatedWorkAsync(work, session));
				}
				else
				{
					await (Isolater.DoNonTransactedWorkAsync(work, session));
				}
			}
			else
			{
				using (var dummyCommand = await (session.ConnectionManager.CreateCommandAsync()))
				{
					await (work.DoWorkAsync(dummyCommand.Connection, dummyCommand.Transaction));
					session.ConnectionManager.AfterStatement();
				}
			}
		}

		protected virtual async Task DropTemporaryTableIfNecessaryAsync(IQueryable persister, ISessionImplementor session)
		{
			if (Factory.Dialect.DropTemporaryTableAfterUse())
			{
				IIsolatedWork work = new TmpIdTableDropIsolatedWork(persister, log, session);
				if (ShouldIsolateTemporaryTableDDL())
				{
					session.ConnectionManager.Transaction.RegisterSynchronization(new AfterTransactionCompletes((success) =>
					{
						if (Factory.Settings.IsDataDefinitionInTransactionSupported)
						{
							Isolater.DoIsolatedWork(work, session);
						}
						else
						{
							Isolater.DoNonTransactedWork(work, session);
						}
					}

					));
				}
				else
				{
					using (var dummyCommand = await (session.ConnectionManager.CreateCommandAsync()))
					{
						await (work.DoWorkAsync(dummyCommand.Connection, dummyCommand.Transaction));
						session.ConnectionManager.AfterStatement();
					}
				}
			}
			else
			{
				// at the very least cleanup the data :)
				DbCommand ps = null;
				try
				{
					var commandText = new SqlString("delete from " + persister.TemporaryIdTableName);
					ps = await (session.Batcher.PrepareCommandAsync(CommandType.Text, commandText, new SqlType[0]));
					await (session.Batcher.ExecuteNonQueryAsync(ps));
				}
				catch (Exception t)
				{
					log.Warn("unable to cleanup temporary id table after use [" + t + "]");
				}
				finally
				{
					if (ps != null)
					{
						try
						{
							session.Batcher.CloseCommand(ps, null);
						}
						catch (Exception)
						{
						// ignore
						}
					}
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TmpIdTableCreationIsolatedWork : IIsolatedWork
		{
			public async Task DoWorkAsync(DbConnection connection, DbTransaction transaction)
			{
				DbCommand stmnt = null;
				try
				{
					stmnt = connection.CreateCommand();
					stmnt.Transaction = transaction;
					stmnt.CommandText = persister.TemporaryIdTableDDL;
					await (stmnt.ExecuteNonQueryAsync());
					session.Factory.Settings.SqlStatementLogger.LogCommand(stmnt, FormatStyle.Ddl);
				}
				catch (Exception t)
				{
					log.Debug("unable to create temporary id table [" + t.Message + "]");
				}
				finally
				{
					if (stmnt != null)
					{
						try
						{
							stmnt.Dispose();
						}
						catch (Exception)
						{
						// ignore
						}
					}
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TmpIdTableDropIsolatedWork : IIsolatedWork
		{
			public async Task DoWorkAsync(DbConnection connection, DbTransaction transaction)
			{
				DbCommand stmnt = null;
				try
				{
					stmnt = connection.CreateCommand();
					stmnt.Transaction = transaction;
					stmnt.CommandText = "drop table " + persister.TemporaryIdTableName;
					await (stmnt.ExecuteNonQueryAsync());
					session.Factory.Settings.SqlStatementLogger.LogCommand(stmnt, FormatStyle.Ddl);
				}
				catch (Exception t)
				{
					log.Warn("unable to drop temporary id table after use [" + t.Message + "]");
				}
				finally
				{
					if (stmnt != null)
					{
						try
						{
							stmnt.Dispose();
						}
						catch (Exception)
						{
						// ignore
						}
					}
				}
			}
		}
	}
}
#endif
