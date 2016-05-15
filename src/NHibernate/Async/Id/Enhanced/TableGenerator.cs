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
	/// <summary>
	/// An enhanced version of table-based id generation.
	/// </summary>
	/// <remarks>
	/// Unlike the simplistic legacy one (which, btw, was only ever intended for subclassing
	/// support) we "segment" the table into multiple values. Thus a single table can
	/// actually serve as the persistent storage for multiple independent generators.  One
	/// approach would be to segment the values by the name of the entity for which we are
	/// performing generation, which would mean that we would have a row in the generator
	/// table for each entity name.  Or any configuration really; the setup is very flexible.
	/// <para>
	/// In this respect it is very similar to the legacy
	/// MultipleHiLoPerTableGenerator (not available in NHibernate) in terms of the
	/// underlying storage structure (namely a single table capable of holding
	/// multiple generator values). The differentiator is, as with
	/// <see cref = "SequenceStyleGenerator"/> as well, the externalized notion
	/// of an optimizer.
	/// </para>
	/// <para>
	/// <b>NOTE</b> that by default we use a single row for all generators (based
	/// on <see cref = "DefaultSegmentValue"/>).  The configuration parameter
	/// <see cref = "ConfigPreferSegmentPerEntity"/> can be used to change that to
	/// instead default to using a row for each entity name.
	/// </para>
	/// Configuration parameters:
	///<table>
	///	 <tr>
	///    <td><b>NAME</b></td>
	///    <td><b>DEFAULT</b></td>
	///    <td><b>DESCRIPTION</b></td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "TableParam"/></td>
	///    <td><see cref = "DefaultTable"/></td>
	///    <td>The name of the table to use to store/retrieve values</td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "ValueColumnParam"/></td>
	///    <td><see cref = "DefaultValueColumn"/></td>
	///    <td>The name of column which holds the sequence value for the given segment</td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "SegmentColumnParam"/></td>
	///    <td><see cref = "DefaultSegmentColumn"/></td>
	///    <td>The name of the column which holds the segment key</td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "SegmentValueParam"/></td>
	///    <td><see cref = "DefaultSegmentValue"/></td>
	///    <td>The value indicating which segment is used by this generator; refers to values in the <see cref = "SegmentColumnParam"/> column</td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "SegmentLengthParam"/></td>
	///    <td><see cref = "DefaultSegmentLength"/></td>
	///    <td>The data length of the <see cref = "SegmentColumnParam"/> column; used for schema creation</td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "InitialParam"/></td>
	///    <td><see cref = "DefaltInitialValue"/></td>
	///    <td>The initial value to be stored for the given segment</td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "IncrementParam"/></td>
	///    <td><see cref = "DefaultIncrementSize"/></td>
	///    <td>The increment size for the underlying segment; see the discussion on <see cref = "Optimizer"/> for more details.</td>
	///  </tr>
	///  <tr>
	///    <td><see cref = "OptimizerParam"/></td>
	///    <td><i>depends on defined increment size</i></td>
	///    <td>Allows explicit definition of which optimization strategy to use</td>
	///  </tr>
	///</table>
	/// </remarks>
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
