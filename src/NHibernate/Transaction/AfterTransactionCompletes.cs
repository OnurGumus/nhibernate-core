using System;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Transaction
{
	public class AfterTransactionCompletes : ISynchronization
	{
		#region Fields

		private readonly Func<bool, Task> _whenCompleted;

		#endregion

		#region Constructors/Destructors

		/// <summary>
		/// Create an AfterTransactionCompletes that will execute the given delegate
		/// when the transaction is completed. The action delegate will receive
		/// the value 'true' if the transaction was completed successfully.
		/// </summary>
		/// <param name="whenCompleted"></param>
		public AfterTransactionCompletes(Func<bool, Task> whenCompleted)
		{
			_whenCompleted = whenCompleted;
		}

		#endregion

		#region ISynchronization Members

		public Task BeforeCompletion()
		{
			return TaskHelper.CompletedTask;
		}

		public Task AfterCompletion(bool success)
		{
			return _whenCompleted(success);
		}

		#endregion
	}
}
