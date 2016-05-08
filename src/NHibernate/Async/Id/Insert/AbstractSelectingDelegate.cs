using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Id.Insert
{
	/// <summary> 
	/// Abstract InsertGeneratedIdentifierDelegate implementation where the
	/// underlying strategy requires an subsequent select after the insert
	/// to determine the generated identifier. 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractSelectingDelegate : IInsertGeneratedIdentifierDelegate
	{
		public async Task<object> PerformInsertAsync(SqlCommandInfo insertSQL, ISessionImplementor session, IBinder binder)
		{
			try
			{
				// prepare and execute the insert
				IDbCommand insert = session.Batcher.PrepareCommand(insertSQL.CommandType, insertSQL.Text, insertSQL.ParameterTypes);
				try
				{
					binder.BindValues(insert);
					session.Batcher.ExecuteNonQuery(insert);
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
					IDbCommand idSelect = session.Batcher.PrepareCommand(CommandType.Text, selectSQL, ParametersTypes);
					try
					{
						BindParameters(session, idSelect, binder.Entity);
						IDataReader rs = await (session.Batcher.ExecuteReaderAsync(idSelect));
						try
						{
							return GetResult(session, rs, binder.Entity);
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
	}
}