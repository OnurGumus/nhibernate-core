#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id.Insert
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractSelectingDelegate : IInsertGeneratedIdentifierDelegate
	{
		public async Task<object> PerformInsertAsync(SqlCommandInfo insertSQL, ISessionImplementor session, IBinder binder)
		{
			try
			{
				// prepare and execute the insert
				DbCommand insert = await (session.Batcher.PrepareCommandAsync(insertSQL.CommandType, insertSQL.Text, insertSQL.ParameterTypes));
				try
				{
					await (binder.BindValuesAsync(insert));
					await (session.Batcher.ExecuteNonQueryAsync(insert));
				}
				finally
				{
					session.Batcher.CloseCommand(insert, null);
				}
			}
			catch (DbException sqle)
			{
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not insert: " + persister.GetInfoString(), insertSQL.Text);
			}

			SqlString selectSQL = SelectSQL;
			using (new SessionIdLoggingContext(session.SessionId))
				try
				{
					//fetch the generated id in a separate query
					DbCommand idSelect = await (session.Batcher.PrepareCommandAsync(CommandType.Text, selectSQL, ParametersTypes));
					try
					{
						await (BindParametersAsync(session, idSelect, binder.Entity));
						DbDataReader rs = await (session.Batcher.ExecuteReaderAsync(idSelect));
						try
						{
							return await (GetResultAsync(session, rs, binder.Entity));
						}
						finally
						{
							session.Batcher.CloseReader(rs);
						}
					}
					finally
					{
						session.Batcher.CloseCommand(idSelect, null);
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not retrieve generated id after insert: " + persister.GetInfoString(), insertSQL.Text);
				}
		}

		/// <summary> Extract the generated key value from the given result set. </summary>
		/// <param name = "session">The session </param>
		/// <param name = "rs">The result set containing the generated primary key values. </param>
		/// <param name = "entity">The entity being saved. </param>
		/// <returns> The generated identifier </returns>
		protected internal abstract Task<object> GetResultAsync(ISessionImplementor session, DbDataReader rs, object entity);
		/// <summary> Bind any required parameter values into the SQL command <see cref = "SelectSQL"/>. </summary>
		/// <param name = "session">The session </param>
		/// <param name = "ps">The prepared <see cref = "SelectSQL"/> command </param>
		/// <param name = "entity">The entity being saved. </param>
		protected internal virtual Task BindParametersAsync(ISessionImplementor session, DbCommand ps, object entity)
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
