#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.AdoNet.Util;
using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	/// <summary>
	/// Describes a table used to mimic sequence behavior
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableStructure : TransactionHelper, IDatabaseStructure
	{
		public override async Task<object> DoWorkInCurrentTransactionAsync(ISessionImplementor session, DbConnection conn, DbTransaction transaction)
		{
			long result;
			int updatedRows;
			do
			{
				try
				{
					object selectedValue;
					DbCommand selectCmd = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, _selectQuery, SqlTypeFactory.NoTypes);
					using (selectCmd)
					{
						selectCmd.Connection = conn;
						selectCmd.Transaction = transaction;
						PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(selectCmd, FormatStyle.Basic);
						selectedValue = await (selectCmd.ExecuteScalarAsync());
					}

					if (selectedValue == null)
					{
						string err = "could not read a hi value - you need to populate the table: " + _tableName;
						Log.Error(err);
						throw new IdentifierGenerationException(err);
					}

					result = Convert.ToInt64(selectedValue);
				}
				catch (Exception sqle)
				{
					Log.Error("could not read a hi value", sqle);
					throw;
				}

				try
				{
					DbCommand updateCmd = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, _updateQuery, _updateParameterTypes);
					using (updateCmd)
					{
						updateCmd.Connection = conn;
						updateCmd.Transaction = transaction;
						PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand(updateCmd, FormatStyle.Basic);
						int increment = _applyIncrementSizeToSourceValues ? _incrementSize : 1;
						((DbParameter)updateCmd.Parameters[0]).Value = result + increment;
						((DbParameter)updateCmd.Parameters[1]).Value = result;
						updatedRows = await (updateCmd.ExecuteNonQueryAsync());
					}
				}
				catch (Exception sqle)
				{
					Log.Error("could not update hi value in: " + _tableName, sqle);
					throw;
				}
			}
			while (updatedRows == 0);
			_accessCounter++;
			return result;
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TableAccessCallback : IAccessCallback
		{
			public virtual async Task<long> GetNextValueAsync()
			{
				return Convert.ToInt64(await (_owner.DoWorkInNewTransactionAsync(_session)));
			}
		}
	}
}
#endif
