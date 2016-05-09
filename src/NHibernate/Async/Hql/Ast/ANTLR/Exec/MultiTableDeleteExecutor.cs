using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using IQueryable = NHibernate.Persister.Entity.IQueryable;
using System.Threading.Tasks;

namespace NHibernate.Hql.Ast.ANTLR.Exec
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultiTableDeleteExecutor : AbstractStatementExecutor
	{
		public override async Task<int> ExecuteAsync(QueryParameters parameters, ISessionImplementor session)
		{
			CoordinateSharedCacheCleanup(session);
			CreateTemporaryTableIfNecessary(persister, session);
			try
			{
				// First, save off the pertinent ids, saving the number of pertinent ids for return
				IDbCommand ps = null;
				int resultCount;
				try
				{
					try
					{
						var paramsSpec = Walker.Parameters;
						var sqlQueryParametersList = idInsertSelect.GetParameters().ToList();
						SqlType[] parameterTypes = paramsSpec.GetQueryParameterTypes(sqlQueryParametersList, session.Factory);
						ps = session.Batcher.PrepareCommand(CommandType.Text, idInsertSelect, parameterTypes);
						foreach (var parameterSpecification in paramsSpec)
						{
							await (parameterSpecification.BindAsync(ps, sqlQueryParametersList, parameters, session));
						}

						resultCount = session.Batcher.ExecuteNonQuery(ps);
					}
					finally
					{
						if (ps != null)
						{
							session.Batcher.CloseCommand(ps, null);
						}
					}
				}
				catch (DbException e)
				{
					throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not insert/select ids for bulk delete", idInsertSelect);
				}

				// Start performing the deletes
				for (int i = 0; i < deletes.Length; i++)
				{
					try
					{
						try
						{
							ps = session.Batcher.PrepareCommand(CommandType.Text, deletes[i], new SqlType[0]);
							session.Batcher.ExecuteNonQuery(ps);
						}
						finally
						{
							if (ps != null)
							{
								session.Batcher.CloseCommand(ps, null);
							}
						}
					}
					catch (DbException e)
					{
						throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "error performing bulk delete", deletes[i]);
					}
				}

				return resultCount;
			}
			finally
			{
				DropTemporaryTableIfNecessary(persister, session);
			}
		}
	}
}