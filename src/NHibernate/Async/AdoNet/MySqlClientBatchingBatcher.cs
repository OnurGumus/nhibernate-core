#if NET_4_5
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using System.Threading.Tasks;

namespace NHibernate.AdoNet
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MySqlClientBatchingBatcher : AbstractBatcher
	{
		public override async Task AddToBatchAsync(IExpectation expectation)
		{
			totalExpectedRowsAffected += expectation.ExpectedRowCount;
			DbCommand batchUpdate = CurrentCommand;
			await (PrepareAsync(batchUpdate));
			Driver.AdjustCommand(batchUpdate);
			string lineWithParameters = null;
			var sqlStatementLogger = Factory.Settings.SqlStatementLogger;
			if (sqlStatementLogger.IsDebugEnabled || Log.IsDebugEnabled)
			{
				lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(batchUpdate);
				var formatStyle = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic);
				lineWithParameters = formatStyle.Formatter.Format(lineWithParameters);
				currentBatchCommandsLog.Append("command ").Append(currentBatch.CountOfCommands).Append(":").AppendLine(lineWithParameters);
			}

			if (Log.IsDebugEnabled)
			{
				Log.Debug("Adding to batch:" + lineWithParameters);
			}

			currentBatch.Append(batchUpdate);
			if (currentBatch.CountOfCommands >= batchSize)
			{
				await (DoExecuteBatchAsync(batchUpdate));
			}
		}

		protected override async Task DoExecuteBatchAsync(DbCommand ps)
		{
			Log.DebugFormat("Executing batch");
			await (CheckReadersAsync());
			if (Factory.Settings.SqlStatementLogger.IsDebugEnabled)
			{
				Factory.Settings.SqlStatementLogger.LogBatchCommand(currentBatchCommandsLog.ToString());
				currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
			}

			int rowsAffected;
			try
			{
				rowsAffected = currentBatch.ExecuteNonQuery();
			}
			catch (DbException e)
			{
				throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not execute batch command.");
			}

			Expectations.VerifyOutcomeBatched(totalExpectedRowsAffected, rowsAffected);
			currentBatch.Dispose();
			totalExpectedRowsAffected = 0;
			currentBatch = CreateConfiguredBatch();
		}
	}
}
#endif
