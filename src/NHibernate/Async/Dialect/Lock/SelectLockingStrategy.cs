#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Impl;
using NHibernate.Exceptions;
using System.Threading.Tasks;

namespace NHibernate.Dialect.Lock
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SelectLockingStrategy : ILockingStrategy
	{
		public async Task LockAsync(object id, object version, object obj, ISessionImplementor session)
		{
			ISessionFactoryImplementor factory = session.Factory;
			try
			{
				DbCommand st = await (session.Batcher.PrepareCommandAsync(CommandType.Text, sql, lockable.IdAndVersionSqlTypes));
				DbDataReader rs = null;
				try
				{
					await (lockable.IdentifierType.NullSafeSetAsync(st, id, 0, session));
					if (lockable.IsVersioned)
					{
						await (lockable.VersionType.NullSafeSetAsync(st, version, lockable.IdentifierType.GetColumnSpan(factory), session));
					}

					rs = await (session.Batcher.ExecuteReaderAsync(st));
					try
					{
						if (!await (rs.ReadAsync()))
						{
							if (factory.Statistics.IsStatisticsEnabled)
							{
								factory.StatisticsImplementor.OptimisticFailure(lockable.EntityName);
							}

							throw new StaleObjectStateException(lockable.EntityName, id);
						}
					}
					finally
					{
						rs.Close();
					}
				}
				finally
				{
					session.Batcher.CloseCommand(st, rs);
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
