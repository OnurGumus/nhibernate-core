#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id.Insert
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractReturningDelegate : IInsertGeneratedIdentifierDelegate
	{
		public async Task<object> PerformInsertAsync(SqlCommandInfo insertSQL, ISessionImplementor session, IBinder binder)
		{
			try
			{
				// prepare and execute the insert
				DbCommand insert = await (PrepareAsync(insertSQL, session));
				try
				{
					await (binder.BindValuesAsync(insert));
					return await (ExecuteAndExtractAsync(insert, session));
				}
				finally
				{
					ReleaseStatement(insert, session);
				}
			}
			catch (DbException sqle)
			{
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not insert: " + persister.GetInfoString(), insertSQL.Text);
			}
		}

		protected internal abstract Task<DbCommand> PrepareAsync(SqlCommandInfo insertSQL, ISessionImplementor session);
		public abstract Task<object> ExecuteAndExtractAsync(DbCommand insert, ISessionImplementor session);
	}
}
#endif
