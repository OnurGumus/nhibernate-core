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
		protected virtual async Task<object> UsePreparedStatementAsync(string timestampSelectString, ISessionImplementor session)
		{
			var tsSelect = new SqlString(timestampSelectString);
			IDbCommand ps = null;
			IDataReader rs = null;
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					ps = session.Batcher.PrepareCommand(CommandType.Text, tsSelect, EmptyParams);
					rs = await (session.Batcher.ExecuteReaderAsync(ps));
					rs.Read();
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