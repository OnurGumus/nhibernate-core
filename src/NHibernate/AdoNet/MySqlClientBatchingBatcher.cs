using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;

namespace NHibernate.AdoNet
{
	public class MySqlClientBatchingBatcher : AbstractBatcher
	{
		private int batchSize;
		private int totalExpectedRowsAffected;
		private MySqlClientSqlCommandSet currentBatch;
		private StringBuilder currentBatchCommandsLog;

		public MySqlClientBatchingBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
			: base(connectionManager, interceptor)
		{
			batchSize = Factory.Settings.AdoBatchSize;
			currentBatch = CreateConfiguredBatch();

			//we always create this, because we need to deal with a scenario in which
			//the user change the logging configuration at runtime. Trying to put this
			//behind an if(log.IsDebugEnabled) will cause a null reference exception 
			//at that point.
			currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
		}

		public override int BatchSize
		{
			get { return batchSize; }
			set { batchSize = value; }
		}

		protected override int CountOfStatementsInCurrentBatch
		{
			get { return currentBatch.CountOfCommands; }
		}

		public override async Task AddToBatch(IExpectation expectation)
		{
			totalExpectedRowsAffected += expectation.ExpectedRowCount;
			IDbCommand batchUpdate = CurrentCommand;
			await Prepare(batchUpdate).ConfigureAwait(false);
			Driver.AdjustCommand(batchUpdate);
			string lineWithParameters = null;
			var sqlStatementLogger = Factory.Settings.SqlStatementLogger;
			if (sqlStatementLogger.IsDebugEnabled || Log.IsDebugEnabled)
			{
				lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(batchUpdate);
				var formatStyle = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic);
				lineWithParameters = formatStyle.Formatter.Format(lineWithParameters);
				currentBatchCommandsLog.Append("command ")
					.Append(currentBatch.CountOfCommands)
					.Append(":")
					.AppendLine(lineWithParameters);
			}
			if (Log.IsDebugEnabled)
			{
				Log.Debug("Adding to batch:" + lineWithParameters);
			}
			currentBatch.Append(batchUpdate);

			if (currentBatch.CountOfCommands >= batchSize)
			{
				await DoExecuteBatch(batchUpdate).ConfigureAwait(false);
			}
		}

		protected override async Task DoExecuteBatch(IDbCommand ps)
		{
			Log.DebugFormat("Executing batch");
			await CheckReaders().ConfigureAwait(false);
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

		private MySqlClientSqlCommandSet CreateConfiguredBatch()
		{
			return new MySqlClientSqlCommandSet(batchSize);
		}
	}
}