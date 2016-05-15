#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DbTimestampType : TimestampType, IVersionType
	{
		public override async Task<object> SeedAsync(ISessionImplementor session)
		{
			if (session == null)
			{
				log.Debug("incoming session was null; using current vm time");
				return await (base.SeedAsync(session));
			}
			else if (!session.Factory.Dialect.SupportsCurrentTimestampSelection)
			{
				log.Debug("falling back to vm-based timestamp, as dialect does not support current timestamp selection");
				return await (base.SeedAsync(session));
			}
			else
			{
				return await (GetCurrentTimestampAsync(session));
			}
		}

		private async Task<object> GetCurrentTimestampAsync(ISessionImplementor session)
		{
			Dialect.Dialect dialect = session.Factory.Dialect;
			string timestampSelectString = dialect.CurrentTimestampSelectString;
			return await (UsePreparedStatementAsync(timestampSelectString, session));
		}

		protected virtual async Task<object> UsePreparedStatementAsync(string timestampSelectString, ISessionImplementor session)
		{
			var tsSelect = new SqlString(timestampSelectString);
			DbCommand ps = null;
			DbDataReader rs = null;
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					ps = await (session.Batcher.PrepareCommandAsync(CommandType.Text, tsSelect, EmptyParams));
					rs = await (session.Batcher.ExecuteReaderAsync(ps));
					await (rs.ReadAsync());
					DateTime ts = rs.GetDateTime(0);
					if (log.IsDebugEnabled)
					{
						log.Debug("current timestamp retreived from db : " + ts + " (tiks=" + ts.Ticks + ")");
					}

					return ts;
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not select current db timestamp", tsSelect);
				}
				finally
				{
					if (ps != null)
					{
						try
						{
							session.Batcher.CloseCommand(ps, rs);
						}
						catch (DbException sqle)
						{
							log.Warn("unable to clean up prepared statement", sqle);
						}
					}
				}
		}
	}
}
#endif
