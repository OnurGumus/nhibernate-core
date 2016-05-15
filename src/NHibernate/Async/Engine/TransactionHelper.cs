#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Engine.Transaction;
using NHibernate.Exceptions;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Engine
{
	/// <summary>
	/// Allows work to be done outside the current transaction, by suspending it,
	/// and performing work in a new transaction
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class TransactionHelper
	{
		/// <summary> The work to be done</summary>
		public abstract Task<object> DoWorkInCurrentTransactionAsync(ISessionImplementor session, DbConnection conn, DbTransaction transaction);
		/// <summary> Suspend the current transaction and perform work in a new transaction</summary>
		public virtual async Task<object> DoWorkInNewTransactionAsync(ISessionImplementor session)
		{
			Work work = new Work(session, this);
			await (Isolater.DoIsolatedWorkAsync(work, session));
			return work.generatedValue;
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class Work : IIsolatedWork
		{
			public async Task DoWorkAsync(DbConnection connection, DbTransaction transaction)
			{
				try
				{
					generatedValue = await (owner.DoWorkInCurrentTransactionAsync(session, connection, transaction));
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not get or update next value", null);
				}
			}
		}
	}
}
#endif
