#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Type;
using NHibernate.Util;
using NHibernate.SqlCommand;
using NHibernate.AdoNet.Util;
using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableGenerator : TransactionHelper, IPersistentIdentifierGenerator, IConfigurable
	{
		private readonly AsyncLock _lock = new AsyncLock();
		public virtual async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			using (var releaser = await _lock.LockAsync())
			{
				return await (Optimizer.GenerateAsync(new TableAccessCallback(session, this)));
			}
		}

		public override async Task<object> DoWorkInCurrentTransactionAsync(ISessionImplementor session, DbConnection conn, DbTransaction transaction)
		{
			long result;
			int updatedRows;
			do
			{
				object selectedValue;
				try
				{
					DbCommand selectCmd = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, selectQuery, selectParameterTypes);
					using (selectCmd)
					{
						selectCmd.Connection = conn;
						selectCmd.Transaction = transaction;
						string s = selectCmd.CommandText;
						((DbParameter)selectCmd.Parameters[0]).Value = SegmentValue;
						PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(selectCmd, FormatStyle.Basic);
						selectedValue = await (selectCmd.ExecuteScalarAsync());
					}

					if (selectedValue == null)
					{
						result = InitialValue;
						DbCommand insertCmd = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, insertQuery, insertParameterTypes);
						using (insertCmd)
						{
							insertCmd.Connection = conn;
							insertCmd.Transaction = transaction;
							((DbParameter)insertCmd.Parameters[0]).Value = SegmentValue;
							((DbParameter)insertCmd.Parameters[1]).Value = result;
							PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(insertCmd, FormatStyle.Basic);
							await (insertCmd.ExecuteNonQueryAsync());
						}
					}
					else
					{
						result = Convert.ToInt64(selectedValue);
					}
				}
				catch (Exception ex)
				{
					log.Error("Unable to read or initialize hi value in " + TableName, ex);
					throw;
				}

				try
				{
					DbCommand updateCmd = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, updateQuery, updateParameterTypes);
					using (updateCmd)
					{
						updateCmd.Connection = conn;
						updateCmd.Transaction = transaction;
						int increment = Optimizer.ApplyIncrementSizeToSourceValues ? IncrementSize : 1;
						((DbParameter)updateCmd.Parameters[0]).Value = result + increment;
						((DbParameter)updateCmd.Parameters[1]).Value = result;
						((DbParameter)updateCmd.Parameters[2]).Value = SegmentValue;
						PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(updateCmd, FormatStyle.Basic);
						updatedRows = await (updateCmd.ExecuteNonQueryAsync());
					}
				}
				catch (Exception ex)
				{
					log.Error("Unable to update hi value in " + TableName, ex);
					throw;
				}
			}
			while (updatedRows == 0);
			TableAccessCount++;
			return result;
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TableAccessCallback : IAccessCallback
		{
			public async Task<long> GetNextValueAsync()
			{
				return Convert.ToInt64(await (owner.DoWorkInNewTransactionAsync(session)));
			}
		}
	}
}
#endif
