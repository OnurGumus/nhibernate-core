#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using IQueryable = NHibernate.Persister.Entity.IQueryable;
using System.Threading.Tasks;

namespace NHibernate.Hql.Ast.ANTLR.Exec
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicExecutor : AbstractStatementExecutor
	{
		public override async Task<int> ExecuteAsync(QueryParameters parameters, ISessionImplementor session)
		{
			CoordinateSharedCacheCleanup(session);
			DbCommand st = null;
			RowSelection selection = parameters.RowSelection;
			try
			{
				try
				{
					CheckParametersExpectedType(parameters); // NH Different behavior (NH-1898)
					var sqlQueryParametersList = sql.GetParameters().ToList();
					SqlType[] parameterTypes = Parameters.GetQueryParameterTypes(sqlQueryParametersList, session.Factory);
					st = await (session.Batcher.PrepareCommandAsync(CommandType.Text, sql, parameterTypes));
					foreach (var parameterSpecification in Parameters)
					{
						await (parameterSpecification.BindAsync(st, sqlQueryParametersList, parameters, session));
					}

					if (selection != null)
					{
						if (selection.Timeout != RowSelection.NoValue)
						{
							st.CommandTimeout = selection.Timeout;
						}
					}

					return await (session.Batcher.ExecuteNonQueryAsync(st));
				}
				finally
				{
					if (st != null)
					{
						session.Batcher.CloseCommand(st, null);
					}
				}
			}
			catch (DbException sqle)
			{
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not execute update query", sql);
			}
		}
	}
}
#endif
