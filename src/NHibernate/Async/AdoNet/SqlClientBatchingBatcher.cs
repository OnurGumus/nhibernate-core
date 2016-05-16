#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.AdoNet
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlClientBatchingBatcher : AbstractBatcher
	{
		public override async Task AddToBatchAsync(IExpectation expectation)
		{
			_totalExpectedRowsAffected += expectation.ExpectedRowCount;
			DbCommand batchUpdate = CurrentCommand;
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
				await (ExecuteBatchWithTimingAsync(batchUpdate));
			}
		}

		protected override async Task DoExecuteBatchAsync(DbCommand ps)
		{
			Log.DebugFormat("Executing batch");
			CheckReaders();
			await (PrepareAsync(_currentBatch.BatchCommand));
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
#endif
