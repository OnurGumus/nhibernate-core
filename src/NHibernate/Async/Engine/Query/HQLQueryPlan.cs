#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Linq;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Engine.Query
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IQueryPlan
	{
		Task PerformListAsync(QueryParameters queryParameters, ISessionImplementor statelessSessionImpl, IList results);
		Task<int> PerformExecuteUpdateAsync(QueryParameters queryParameters, ISessionImplementor statelessSessionImpl);
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HQLQueryPlan : IQueryPlan
	{
		public async Task PerformListAsync(QueryParameters queryParameters, ISessionImplementor session, IList results)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("find: " + _sourceQuery);
				queryParameters.LogParameters(session.Factory);
			}

			bool hasLimit = queryParameters.RowSelection != null && queryParameters.RowSelection.DefinesLimits;
			bool needsLimit = hasLimit && Translators.Length > 1;
			QueryParameters queryParametersToUse;
			if (needsLimit)
			{
				Log.Warn("firstResult/maxResults specified on polymorphic query; applying in memory!");
				RowSelection selection = new RowSelection();
				selection.FetchSize = queryParameters.RowSelection.FetchSize;
				selection.Timeout = queryParameters.RowSelection.Timeout;
				queryParametersToUse = queryParameters.CreateCopyUsing(selection);
			}
			else
			{
				queryParametersToUse = queryParameters;
			}

			IList combinedResults = results ?? new List<object>();
			IdentitySet distinction = new IdentitySet();
			int includedCount = -1;
			for (int i = 0; i < Translators.Length; i++)
			{
				IList tmp = await (Translators[i].ListAsync(session, queryParametersToUse));
				if (needsLimit)
				{
					// NOTE : firstRow is zero-based
					int first = queryParameters.RowSelection.FirstRow == RowSelection.NoValue ? 0 : queryParameters.RowSelection.FirstRow;
					int max = queryParameters.RowSelection.MaxRows == RowSelection.NoValue ? RowSelection.NoValue : queryParameters.RowSelection.MaxRows;
					int size = tmp.Count;
					for (int x = 0; x < size; x++)
					{
						object result = tmp[x];
						if (distinction.Add(result))
						{
							continue;
						}

						includedCount++;
						if (includedCount < first)
						{
							continue;
						}

						combinedResults.Add(result);
						if (max >= 0 && includedCount > max)
						{
							// break the outer loop !!!
							return;
						}
					}
				}
				else
					ArrayHelper.AddAll(combinedResults, tmp);
			}
		}

		public async Task<int> PerformExecuteUpdateAsync(QueryParameters queryParameters, ISessionImplementor session)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("executeUpdate: " + _sourceQuery);
				queryParameters.LogParameters(session.Factory);
			}

			if (Translators.Length != 1)
			{
				Log.Warn("manipulation query [" + _sourceQuery + "] resulted in [" + Translators.Length + "] split queries");
			}

			int result = 0;
			for (int i = 0; i < Translators.Length; i++)
			{
				result += await (Translators[i].ExecuteUpdateAsync(queryParameters, session));
			}

			return result;
		}
	}
}
#endif
