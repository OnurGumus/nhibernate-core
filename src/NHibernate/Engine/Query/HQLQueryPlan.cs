using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Linq;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace NHibernate.Engine.Query
{
	public interface IQueryPlan
	{
		ParameterMetadata ParameterMetadata { get; }
		ISet<string> QuerySpaces { get; }
		IQueryTranslator[] Translators { get; }
		ReturnMetadata ReturnMetadata { get; }
		Task PerformList(QueryParameters queryParameters, ISessionImplementor statelessSessionImpl, IList results);
		Task<int> PerformExecuteUpdate(QueryParameters queryParameters, ISessionImplementor statelessSessionImpl);
		Task<IEnumerable<T>> PerformIterate<T>(QueryParameters queryParameters, IEventSource session);
		Task<IEnumerable> PerformIterate(QueryParameters queryParameters, IEventSource session);
	}

	public interface IQueryExpressionPlan : IQueryPlan
	{
		IQueryExpression QueryExpression { get; }
	}

	/// <summary> Defines a query execution plan for an HQL query (or filter). </summary>
	[Serializable]
	public class HQLQueryPlan : IQueryPlan
	{
		protected static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof(HQLQueryPlan));

		private readonly string _sourceQuery;

        protected HQLQueryPlan(string sourceQuery, IQueryTranslator[] translators)
        {
            Translators = translators;
            _sourceQuery = sourceQuery;

            FinaliseQueryPlan();
        }

		internal HQLQueryPlan(HQLQueryPlan source)
		{
			Translators = source.Translators;
			_sourceQuery = source._sourceQuery;
			QuerySpaces = source.QuerySpaces;
			ParameterMetadata = source.ParameterMetadata;
			ReturnMetadata = source.ReturnMetadata;
			SqlStrings = source.SqlStrings;
		}

	    public ISet<string> QuerySpaces
		{
		    get;
		    private set;
		}

		public ParameterMetadata ParameterMetadata
		{
            get;
            private set;
        }

		public ReturnMetadata ReturnMetadata
		{
            get;
            private set;
        }

		public string[] SqlStrings
		{
            get;
            private set;
        }

		public IQueryTranslator[] Translators
		{
            get;
            private set;
        }

		public async Task PerformList(QueryParameters queryParameters, ISessionImplementor session, IList results)
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
				IList tmp = await Translators[i].List(session, queryParametersToUse).ConfigureAwait(false);
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

		public async Task<IEnumerable> PerformIterate(QueryParameters queryParameters, IEventSource session)
		{
			var data = await DoIterate(queryParameters, session).ConfigureAwait(false);
			return (data.Many.HasValue && data.Many.Value) ? new JoinedEnumerable(data.Results) : data.Result;
		}

		public async Task<IEnumerable<T>> PerformIterate<T>(QueryParameters queryParameters, IEventSource session)
		{
			return new SafetyEnumerable<T>(await PerformIterate(queryParameters, session).ConfigureAwait(false));
		}

		public async Task<int> PerformExecuteUpdate(QueryParameters queryParameters, ISessionImplementor session)
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
				result += await Translators[i].ExecuteUpdate(queryParameters, session).ConfigureAwait(false);
			}
			return result;
		}

		async Task<IterateResult> DoIterate(QueryParameters queryParameters, IEventSource session)
		{
			bool? isMany = null;
			IEnumerable[] results = null;
			IEnumerable result = null;
			if (Log.IsDebugEnabled)
			{
				Log.Debug("enumerable: " + _sourceQuery);
				queryParameters.LogParameters(session.Factory);
			}
			if (Translators.Length == 0)
			{
				result = CollectionHelper.EmptyEnumerable;
			}
			else
			{
				results = null;
				bool many = Translators.Length > 1;
				if (many)
				{
					results = new IEnumerable[Translators.Length];
				}

				result = null;
				for (int i = 0; i < Translators.Length; i++)
				{
					result = await Translators[i].GetEnumerable(queryParameters, session).ConfigureAwait(false);
					if (many)
						results[i] = result;
				}
				isMany = many;
			}
			return new IterateResult(isMany, results, result);
		}

		private void FinaliseQueryPlan()
		{
			BuildSqlStringsAndQuerySpaces();
			BuildMetaData();
		}

		private void BuildMetaData()
		{
			if (Translators.Length == 0)
			{
				ParameterMetadata = new ParameterMetadata(null, null);
				ReturnMetadata = null;
			}
			else
			{
				ParameterMetadata = Translators[0].BuildParameterMetadata();

				if (Translators[0].IsManipulationStatement)
				{
					ReturnMetadata = null;
				}
				else
				{
					if (Translators.Length > 1)
					{
						int returns = Translators[0].ReturnTypes.Length;
						ReturnMetadata = new ReturnMetadata(Translators[0].ReturnAliases, new IType[returns]);
					}
					else
					{
						ReturnMetadata = new ReturnMetadata(Translators[0].ReturnAliases, Translators[0].ReturnTypes);
					}
				}
			}
		}

		private void BuildSqlStringsAndQuerySpaces()
		{
			var combinedQuerySpaces = new HashSet<string>();
			var sqlStringList = new List<string>();

			foreach (var translator in Translators)
			{
				foreach (var qs in translator.QuerySpaces)
				{
					combinedQuerySpaces.Add(qs);
				}

				sqlStringList.AddRange(translator.CollectSqlStrings);
			}

			SqlStrings = sqlStringList.ToArray();
			QuerySpaces = combinedQuerySpaces;
		}

		private class IterateResult
		{
			public IterateResult(bool? many, IEnumerable[] results, IEnumerable result)
			{
				Many = many;
				Results = results;
				Result = result;
			}

			public bool? Many { get; private set; }
			public IEnumerable[] Results { get; private set; }
			public IEnumerable Result { get; private set; }
		}
	}
}
