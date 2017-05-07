﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;

using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Linq;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Engine.Query
{
    using System.Threading.Tasks;
    /// <content>
    /// Contains generated async methods
    /// </content>
    public partial interface IQueryPlan
    {
        Task PerformListAsync(QueryParameters queryParameters, ISessionImplementor statelessSessionImpl, IList results);
        Task<int> PerformExecuteUpdateAsync(QueryParameters queryParameters, ISessionImplementor statelessSessionImpl);
        Task<IEnumerable<T>> PerformIterateAsync<T>(QueryParameters queryParameters, IEventSource session);
        Task<IEnumerable> PerformIterateAsync(QueryParameters queryParameters, IEventSource session);
    }
	/// <content>
	/// Contains generated async methods
	/// </content>
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
				IList tmp = await (Translators[i].ListAsync(session, queryParametersToUse)).ConfigureAwait(false);
				if (needsLimit)
				{
					// NOTE : firstRow is zero-based
					int first = queryParameters.RowSelection.FirstRow == RowSelection.NoValue
												? 0
												: queryParameters.RowSelection.FirstRow;

					int max = queryParameters.RowSelection.MaxRows == RowSelection.NoValue
											? RowSelection.NoValue
											: queryParameters.RowSelection.MaxRows;

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

		public async Task<IEnumerable> PerformIterateAsync(QueryParameters queryParameters, IEventSource session)
		{
			if (Log.IsDebugEnabled)
			{
				Log.Debug("enumerable: " + _sourceQuery);
				queryParameters.LogParameters(session.Factory);
			}
			if (Translators.Length == 0)
			{
				return CollectionHelper.EmptyEnumerable;
			}
			if (Translators.Length == 1)
			{
				return await (Translators[0].GetEnumerableAsync(queryParameters, session)).ConfigureAwait(false);
			}
			var results = new IEnumerable[Translators.Length];
			for (int i = 0; i < Translators.Length; i++)
			{
				var result = await (Translators[i].GetEnumerableAsync(queryParameters, session)).ConfigureAwait(false);
				results[i] = result;
			}
			return new JoinedEnumerable(results);
		}

		public async Task<IEnumerable<T>> PerformIterateAsync<T>(QueryParameters queryParameters, IEventSource session)
		{
			return new SafetyEnumerable<T>(await (PerformIterateAsync(queryParameters, session)).ConfigureAwait(false));
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
                result += await (Translators[i].ExecuteUpdateAsync(queryParameters, session)).ConfigureAwait(false);
            }
            return result;
        }
    }
}
