#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using NHibernate.AdoNet.Util;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that uses a database table to store the last
	/// generated value.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It is not intended that applications use this strategy directly. However,
	/// it may be used to build other (efficient) strategies. The return type is
	/// <c>System.Int32</c>
	/// </p>
	/// <p>
	/// The hi value MUST be fetched in a separate transaction to the <c>ISession</c>
	/// transaction so the generator must be able to obtain a new connection and commit it.
	/// Hence this implementation may not be used when the user is supplying connections.
	/// </p>
	/// <p>
	/// The mapping parameters <c>table</c> and <c>column</c> are required.
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableGenerator : TransactionHelper, IPersistentIdentifierGenerator, IConfigurable
	{
		private readonly AsyncLock _lock = new AsyncLock();
		/// <summary>
		/// Generate a <see cref = "short "/>, <see cref = "int "/>, or <see cref = "long "/> 
		/// for the identifier by selecting and updating a value in a table.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "short "/>, <see cref = "int "/>, or <see cref = "long "/>.</returns>
		public virtual async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			using (var releaser = await _lock.LockAsync())
			{
				// This has to be done using a different connection to the containing
				// transaction becase the new hi value must remain valid even if the
				// containing transaction rolls back.
				return await (DoWorkInNewTransactionAsync(session));
			}
		}

		public override async Task<object> DoWorkInCurrentTransactionAsync(ISessionImplementor session, DbConnection conn, DbTransaction transaction)
		{
			long result;
			int rows;
			do
			{
				//the loop ensure atomicitiy of the 
				//select + uspdate even for no transaction
				//or read committed isolation level (needed for .net?)
				DbCommand qps = conn.CreateCommand();
				DbDataReader rs = null;
				qps.CommandText = query;
				qps.CommandType = CommandType.Text;
				qps.Transaction = transaction;
				PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand("Reading high value:", qps, FormatStyle.Basic);
				try
				{
					rs = await (qps.ExecuteReaderAsync());
					if (!await (rs.ReadAsync()))
					{
						string err;
						if (string.IsNullOrEmpty(whereClause))
						{
							err = "could not read a hi value - you need to populate the table: " + tableName;
						}
						else
						{
							err = string.Format("could not read a hi value from table '{0}' using the where clause ({1})- you need to populate the table.", tableName, whereClause);
						}

						log.Error(err);
						throw new IdentifierGenerationException(err);
					}

					result = Convert.ToInt64(columnType.Get(rs, 0));
				}
				catch (Exception e)
				{
					log.Error("could not read a hi value", e);
					throw;
				}
				finally
				{
					if (rs != null)
					{
						rs.Close();
					}

					qps.Dispose();
				}

				DbCommand ups = session.Factory.ConnectionProvider.Driver.GenerateCommand(CommandType.Text, updateSql, parameterTypes);
				ups.Connection = conn;
				ups.Transaction = transaction;
				try
				{
					columnType.Set(ups, result + 1, 0);
					columnType.Set(ups, result, 1);
					PersistentIdGeneratorParmsNames.SqlStatementLogger.LogCommand("Updating high value:", ups, FormatStyle.Basic);
					rows = await (ups.ExecuteNonQueryAsync());
				}
				catch (Exception e)
				{
					log.Error("could not update hi value in: " + tableName, e);
					throw;
				}
				finally
				{
					ups.Dispose();
				}
			}
			while (rows == 0);
			return result;
		}
	}
}
#endif
