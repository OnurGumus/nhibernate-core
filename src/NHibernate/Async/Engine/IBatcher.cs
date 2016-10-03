﻿#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IBatcher : IDisposable
	{
		/// <summary>
		/// Get a non-batchable an <see cref = "DbCommand"/> to use for inserting / deleting / updating.
		/// Must be explicitly released by <c>CloseCommand()</c>
		/// </summary>
		/// <param name = "sql">The <see cref = "SqlString"/> to convert to an <see cref = "DbCommand"/>.</param>
		/// <param name = "commandType">The <see cref = "CommandType"/> of the command.</param>
		/// <param name = "parameterTypes">The <see cref = "SqlType">SqlTypes</see> of parameters
		/// in <paramref name = "sql"/>.</param>
		/// <returns>
		/// An <see cref = "DbCommand"/> that is ready to have the parameter values set
		/// and then executed.
		/// </returns>
		Task<DbCommand> PrepareCommandAsync(CommandType commandType, SqlString sql, SqlType[] parameterTypes);
		/// <summary>
		/// Get a batchable <see cref = "DbCommand"/> to use for inserting / deleting / updating
		/// (might be called many times before a single call to <c>ExecuteBatch()</c>
		/// </summary>
		/// <remarks>
		/// After setting parameters, call <c>AddToBatch()</c> - do not execute the statement
		/// explicitly.
		/// </remarks>
		/// <param name = "sql">The <see cref = "SqlString"/> to convert to an <see cref = "DbCommand"/>.</param>
		/// <param name = "commandType">The <see cref = "CommandType"/> of the command.</param>
		/// <param name = "parameterTypes">The <see cref = "SqlType">SqlTypes</see> of parameters
		/// in <paramref name = "sql"/>.</param>
		/// <returns></returns>
		Task<DbCommand> PrepareBatchCommandAsync(CommandType commandType, SqlString sql, SqlType[] parameterTypes);
		/// <summary>
		/// Add an insert / delete / update to the current batch (might be called multiple times
		/// for a single <c>PrepareBatchStatement()</c>)
		/// </summary>
		/// <param name = "expectation">Determines whether the number of rows affected by query is correct.</param>
		Task AddToBatchAsync(IExpectation expectation);
		/// <summary>
		/// Execute the batch
		/// </summary>
		Task ExecuteBatchAsync();
		Task<DbDataReader> ExecuteReaderAsync(DbCommand cmd);
		Task<int> ExecuteNonQueryAsync(DbCommand cmd);
	}
}
#endif
