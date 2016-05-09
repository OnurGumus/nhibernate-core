using System;
using System.Collections.Generic;
using System.Data;
using NHibernate.Engine;
using NHibernate.Impl;
using System.Threading.Tasks;

namespace NHibernate.Transaction
{
	/// <summary>
	/// Wraps an ADO.NET <see cref = "IDbTransaction"/> to implement
	/// the <see cref = "ITransaction"/> interface.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AdoTransaction : ITransaction
	{
		public async Task CommitAsync()
		{
			using (new SessionIdLoggingContext(sessionId))
			{
				CheckNotDisposed();
				CheckBegun();
				CheckNotZombied();
				log.Debug("Start Commit");
				if (session.FlushMode != FlushMode.Never)
				{
					await (session.FlushAsync());
				}

				NotifyLocalSynchsBeforeTransactionCompletion();
				session.BeforeTransactionCompletion(this);
				try
				{
					trans.Commit();
					log.Debug("IDbTransaction Committed");
					committed = true;
					AfterTransactionCompletion(true);
					Dispose();
				}
				catch (HibernateException e)
				{
					log.Error("Commit failed", e);
					AfterTransactionCompletion(false);
					commitFailed = true;
					// Don't wrap HibernateExceptions
					throw;
				}
				catch (Exception e)
				{
					log.Error("Commit failed", e);
					AfterTransactionCompletion(false);
					commitFailed = true;
					throw new TransactionException("Commit failed with SQL exception", e);
				}
				finally
				{
					CloseIfRequired();
				}
			}
		}
	}
}