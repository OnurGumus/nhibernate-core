﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Transactions;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Impl;

namespace NHibernate.Transaction
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class AdoNetWithDistributedTransactionFactory : ITransactionFactory
	{

		public async Task ExecuteWorkInIsolationAsync(ISessionImplementor session, IIsolatedWork work, bool transacted, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (var tx = new TransactionScope(TransactionScopeOption.Suppress,TransactionScopeAsyncFlowOption.Enabled))
			{
				// instead of duplicating the logic, we suppress the DTC transaction and create
				// our own transaction instead
				await (adoNetTransactionFactory.ExecuteWorkInIsolationAsync(session, work, transacted, cancellationToken)).ConfigureAwait(false);
				tx.Complete();
			}
		}
	}
}