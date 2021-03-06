using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Id.Insert
{
	/// <summary> 
	/// Abstract InsertGeneratedIdentifierDelegate implementation where the
	/// underlying strategy causes the generated identifier to be returned as an
	/// effect of performing the insert statement.  Thus, there is no need for an
	/// additional sql statement to determine the generated identifier. 
	/// </summary>
	public abstract class AbstractReturningDelegate : IInsertGeneratedIdentifierDelegate
	{
		private readonly IPostInsertIdentityPersister persister;

		protected AbstractReturningDelegate(IPostInsertIdentityPersister persister)
		{
			this.persister = persister;
		}

		protected IPostInsertIdentityPersister Persister
		{
			get { return persister; }
		}

		#region IInsertGeneratedIdentifierDelegate Members

		public abstract IdentifierGeneratingInsert PrepareIdentifierGeneratingInsert();

		public async Task<object> PerformInsert(SqlCommandInfo insertSQL, ISessionImplementor session, IBinder binder)
		{
			try
			{
				// prepare and execute the insert
				DbCommand insert = await Prepare(insertSQL, session).ConfigureAwait(false);
				try
				{
					await binder.BindValues(insert).ConfigureAwait(false);
					return await ExecuteAndExtract(insert, session).ConfigureAwait(false);
				}
				finally
				{
					ReleaseStatement(insert, session);
				}
			}
			catch (DbException sqle)
			{
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle,
				                                 "could not insert: " + persister.GetInfoString(), insertSQL.Text);
			}
		}

		#endregion

		protected internal virtual void ReleaseStatement(IDbCommand insert, ISessionImplementor session)
		{
			session.Batcher.CloseCommand(insert, null);
		}

		protected internal abstract Task<DbCommand> Prepare(SqlCommandInfo insertSQL, ISessionImplementor session);

		public abstract Task<object> ExecuteAndExtract(DbCommand insert, ISessionImplementor session);
	}
}