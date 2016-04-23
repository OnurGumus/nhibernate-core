using System.Threading.Tasks;
using NHibernate.Transaction;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1082
{
	public class SynchronizationThatThrowsExceptionAtBeforeTransactionCompletion : ISynchronization
	{
		public Task BeforeCompletion()
		{
			return TaskHelper.FromException<bool>(new BadException());
		}

		public Task AfterCompletion(bool success)
		{
			return TaskHelper.CompletedTask;
		}
	}
}