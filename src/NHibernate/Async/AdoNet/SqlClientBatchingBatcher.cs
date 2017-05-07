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
using System.Text;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;

namespace NHibernate.AdoNet
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class SqlClientBatchingBatcher : AbstractBatcher
	{

		public override Task AddToBatchAsync(IExpectation expectation)
		{
			try
			{
				_totalExpectedRowsAffected += expectation.ExpectedRowCount;
				var batchUpdate = CurrentCommand;
				Driver.AdjustCommand(batchUpdate);
				string lineWithParameters = null;
				var sqlStatementLogger = Factory.Settings.SqlStatementLogger;
				if (sqlStatementLogger.IsDebugEnabled || Log.IsDebugEnabled)
				{
					lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(batchUpdate);
					var formatStyle = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic);
					lineWithParameters = formatStyle.Formatter.Format(lineWithParameters);
					_currentBatchCommandsLog.Append("command ").Append(_currentBatch.CountOfCommands).Append(":").AppendLine(lineWithParameters);
				}

				if (Log.IsDebugEnabled)
				{
					Log.Debug("Adding to batch:" + lineWithParameters);
				}

				_currentBatch.Append((System.Data.SqlClient.SqlCommand)batchUpdate);
				if (_currentBatch.CountOfCommands >= _batchSize)
				{
					return ExecuteBatchWithTimingAsync(batchUpdate);
				}

				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		protected override async Task DoExecuteBatchAsync(DbCommand ps)
		{
			Log.DebugFormat("Executing batch");
			await (CheckReadersAsync()).ConfigureAwait(false);
			await (PrepareAsync(_currentBatch.BatchCommand)).ConfigureAwait(false);
			if (Factory.Settings.SqlStatementLogger.IsDebugEnabled)
			{
				Factory.Settings.SqlStatementLogger.LogBatchCommand(_currentBatchCommandsLog.ToString());
				_currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
			}

			int rowsAffected;
			try
			{
				rowsAffected = _currentBatch.ExecuteNonQuery();
			}
			catch (DbException e)
			{
				throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not execute batch command.");
			}

			Expectations.VerifyOutcomeBatched(_totalExpectedRowsAffected, rowsAffected);

			_currentBatch.Dispose();
			_totalExpectedRowsAffected = 0;
			_currentBatch = CreateConfiguredBatch();
		}
	}
}