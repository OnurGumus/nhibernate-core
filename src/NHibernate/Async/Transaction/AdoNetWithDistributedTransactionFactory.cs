#if NET_4_5
using System;
using System.Collections;
using System.Transactions;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Impl;
using System.Threading.Tasks;

namespace NHibernate.Transaction
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AdoNetWithDistributedTransactionFactory : ITransactionFactory
	{
		public async Task ExecuteWorkInIsolationAsync(ISessionImplementor session, IIsolatedWork work, bool transacted)
		{
			using (var tx = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
			{
				// instead of duplicating the logic, we suppress the DTC transaction and create
				// our own transaction instead
				await (adoNetTransactionFactory.ExecuteWorkInIsolationAsync(session, work, transacted));
				tx.Complete();
			}
		}
	}
}
#endif
