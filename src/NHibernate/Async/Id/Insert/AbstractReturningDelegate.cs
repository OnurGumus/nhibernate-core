#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Id.Insert
{
	/// <summary> 
	/// Abstract InsertGeneratedIdentifierDelegate implementation where the
	/// underlying strategy causes the generated identifier to be returned as an
	/// effect of performing the insert statement.  Thus, there is no need for an
	/// additional sql statement to determine the generated identifier. 
	/// </summary>
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
