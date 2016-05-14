#if NET_4_5
using System;
using System.Data;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Dialect.Lock
{
	/// <summary> 
	/// A locking strategy where the locks are obtained through update statements.
	/// </summary>
	/// <remarks> This strategy is not valid for read style locks. </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UpdateLockingStrategy : ILockingStrategy
	{
		public async Task LockAsync(object id, object version, object obj, ISessionImplementor session)
		{
			if (!lockable.IsVersioned)
			{
				throw new HibernateException("write locks via update not supported for non-versioned entities [" + lockable.EntityName + "]");
			}

			// todo : should we additionally check the current isolation mode explicitly?
			ISessionFactoryImplementor factory = session.Factory;
			try
			{
				IDbCommand st = session.Batcher.PrepareCommand(CommandType.Text, sql, lockable.IdAndVersionSqlTypes);
				try
				{
					await (lockable.VersionType.NullSafeSetAsync(st, version, 1, session));
					int offset = 2;
					await (lockable.IdentifierType.NullSafeSetAsync(st, id, offset, session));
					offset += lockable.IdentifierType.GetColumnSpan(factory);
					if (lockable.IsVersioned)
					{
						await (lockable.VersionType.NullSafeSetAsync(st, version, offset, session));
					}

					int affected = session.Batcher.ExecuteNonQuery(st);
					if (affected < 0)
					{
						factory.StatisticsImplementor.OptimisticFailure(lockable.EntityName);
						throw new StaleObjectStateException(lockable.EntityName, id);
					}
				}
				finally
				{
					session.Batcher.CloseCommand(st, null);
				}
			}
			catch (HibernateException)
			{
				// Do not call Convert on HibernateExceptions
				throw;
			}
			catch (Exception sqle)
			{
				var exceptionContext = new AdoExceptionContextInfo{SqlException = sqle, Message = "could not lock: " + MessageHelper.InfoString(lockable, id, factory), Sql = sql.ToString(), EntityName = lockable.EntityName, EntityId = id};
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, exceptionContext);
			}
		}
	}
}
#endif
