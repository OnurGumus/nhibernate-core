using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Engine.Transaction;
using NHibernate.Exceptions;

namespace NHibernate.Engine
{
	/// <summary>
	/// Allows work to be done outside the current transaction, by suspending it,
	/// and performing work in a new transaction
	/// </summary>
	public abstract class TransactionHelper
	{
		public class Work : IIsolatedWork
		{
			private readonly ISessionImplementor session;
			private readonly TransactionHelper owner;
			internal object generatedValue;

			public Work(ISessionImplementor session, TransactionHelper owner)
			{
				this.session = session;
				this.owner = owner;
			}

			#region Implementation of IIsolatedWork

			public async Task DoWork(DbConnection connection, DbTransaction transaction)
			{
				try
				{
					generatedValue = await owner.DoWorkInCurrentTransaction(session, connection, transaction).ConfigureAwait(false);
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not get or update next value", null);
				}
			}

			#endregion
		}

		/// <summary> The work to be done</summary>
		public abstract Task<object> DoWorkInCurrentTransaction(ISessionImplementor session, DbConnection conn, DbTransaction transaction);

		/// <summary> Suspend the current transaction and perform work in a new transaction</summary>
		public virtual async Task<object> DoWorkInNewTransaction(ISessionImplementor session)
		{
			Work work = new Work(session, this);
			await Isolater.DoIsolatedWork(work, session).ConfigureAwait(false);
			return work.generatedValue;
		}
	}
}