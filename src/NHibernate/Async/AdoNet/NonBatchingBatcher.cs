#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.AdoNet
{
	/// <summary>
	/// An implementation of the <see cref = "IBatcher"/> 
	/// interface that does no batching.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NonBatchingBatcher : AbstractBatcher
	{
		/// <summary>
		/// Executes the current <see cref = "DbCommand"/> and compares the row Count
		/// to the <c>expectedRowCount</c>.
		/// </summary>
		/// <param name = "expectation">
		/// The expected number of rows affected by the query.  A value of less than <c>0</c>
		/// indicates that the number of rows to expect is unknown or should not be a factor.
		/// </param>
		/// <exception cref = "HibernateException">
		/// Thrown when there is an expected number of rows to be affected and the
		/// actual number of rows is different.
		/// </exception>
		public override async Task AddToBatchAsync(IExpectation expectation)
		{
			DbCommand cmd = CurrentCommand;
			Driver.AdjustCommand(cmd);
			int rowCount = await (ExecuteNonQueryAsync(cmd));
			expectation.VerifyOutcomeNonBatched(rowCount, cmd);
		}

		/// <summary>
		/// This Batcher implementation does not support batching so this is a no-op call.  The
		/// actual execution of the <see cref = "DbCommand"/> is run in the <c>AddToBatch</c> 
		/// method.
		/// </summary>
		/// <param name = "ps"></param>
		protected override Task DoExecuteBatchAsync(DbCommand ps)
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
