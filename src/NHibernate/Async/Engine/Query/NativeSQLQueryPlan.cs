using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Action;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.Impl;
using NHibernate.Loader.Custom.Sql;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Engine.Query
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeSQLQueryPlan
	{
		// DONE : H3.2 Executable query (now can be supported for named SQL query/ storedProcedure)
		public async Task<int> PerformExecuteUpdateAsync(QueryParameters queryParameters, ISessionImplementor session)
		{
			CoordinateSharedCacheCleanup(session);
			if (queryParameters.Callable)
			{
				throw new ArgumentException("callable not yet supported for native queries");
			}

			RowSelection selection = queryParameters.RowSelection;
			int result;
			try
			{
				var parametersSpecifications = customQuery.CollectedParametersSpecifications.ToList();
				SqlString sql = ExpandDynamicFilterParameters(customQuery.SQL, parametersSpecifications, session);
				// After the last modification to the SqlString we can collect all parameters types.
				parametersSpecifications.ResetEffectiveExpectedType(queryParameters);
				var sqlParametersList = sql.GetParameters().ToList();
				SqlType[] sqlTypes = parametersSpecifications.GetQueryParameterTypes(sqlParametersList, session.Factory);
				IDbCommand ps = session.Batcher.PrepareCommand(CommandType.Text, sql, sqlTypes);
				try
				{
					if (selection != null && selection.Timeout != RowSelection.NoValue)
					{
						// NH Difference : set Timeout for native query
						ps.CommandTimeout = selection.Timeout;
					}

					foreach (IParameterSpecification parameterSpecification in parametersSpecifications)
					{
						await (parameterSpecification.BindAsync(ps, sqlParametersList, queryParameters, session));
					}

					result = session.Batcher.ExecuteNonQuery(ps);
				}
				finally
				{
					if (ps != null)
					{
						session.Batcher.CloseCommand(ps, null);
					}
				}
			}
			catch (HibernateException)
			{
				throw;
			}
			catch (Exception sqle)
			{
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not execute native bulk manipulation query:" + sourceQuery);
			}

			return result;
		}
	}
}