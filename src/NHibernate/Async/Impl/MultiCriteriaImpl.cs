#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NHibernate.Cache;
using NHibernate.Criterion;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Loader.Criteria;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MultiCriteriaImpl : IMultiCriteria
	{
		public async Task<IList> ListAsync()
		{
			using (new SessionIdLoggingContext(session.SessionId))
			{
				bool cacheable = session.Factory.Settings.IsQueryCacheEnabled && isCacheable;
				CreateCriteriaLoaders();
				CombineCriteriaQueries();
				if (log.IsDebugEnabled)
				{
					log.DebugFormat("Multi criteria with {0} criteria queries.", criteriaQueries.Count);
					for (int i = 0; i < criteriaQueries.Count; i++)
					{
						log.DebugFormat("Query #{0}: {1}", i, criteriaQueries[i]);
					}
				}

				if (cacheable)
				{
					criteriaResults = await (ListUsingQueryCacheAsync());
				}
				else
				{
					criteriaResults = await (ListIgnoreQueryCacheAsync());
				}

				return criteriaResults;
			}
		}

		private async Task<IList> ListUsingQueryCacheAsync()
		{
			IQueryCache queryCache = session.Factory.GetQueryCache(cacheRegion);
			ISet<FilterKey> filterKeys = FilterKey.CreateFilterKeys(session.EnabledFilters, session.EntityMode);
			ISet<string> querySpaces = new HashSet<string>();
			List<IType[]> resultTypesList = new List<IType[]>();
			int[] maxRows = new int[loaders.Count];
			int[] firstRows = new int[loaders.Count];
			for (int i = 0; i < loaders.Count; i++)
			{
				querySpaces.UnionWith(loaders[i].QuerySpaces);
				resultTypesList.Add(loaders[i].ResultTypes);
				firstRows[i] = parameters[i].RowSelection.FirstRow;
				maxRows[i] = parameters[i].RowSelection.MaxRows;
			}

			MultipleQueriesCacheAssembler assembler = new MultipleQueriesCacheAssembler(resultTypesList);
			QueryParameters combinedParameters = CreateCombinedQueryParameters();
			QueryKey key = new QueryKey(session.Factory, SqlString, combinedParameters, filterKeys, null).SetFirstRows(firstRows).SetMaxRows(maxRows);
			IList result = await (assembler.GetResultFromQueryCacheAsync(session, combinedParameters, querySpaces, queryCache, key));
			if (factory.Statistics.IsStatisticsEnabled)
			{
				if (result == null)
				{
					factory.StatisticsImplementor.QueryCacheMiss(key.ToString(), queryCache.RegionName);
				}
				else
				{
					factory.StatisticsImplementor.QueryCacheHit(key.ToString(), queryCache.RegionName);
				}
			}

			if (result == null)
			{
				log.Debug("Cache miss for multi criteria query");
				IList list = await (DoListAsync());
				result = list;
				if ((session.CacheMode & CacheMode.Put) == CacheMode.Put)
				{
					bool put = await (queryCache.PutAsync(key, new ICacheAssembler[]{assembler}, new object[]{list}, combinedParameters.NaturalKeyLookup, session));
					if (put && factory.Statistics.IsStatisticsEnabled)
					{
						factory.StatisticsImplementor.QueryCachePut(key.ToString(), queryCache.RegionName);
					}
				}
			}

			return GetResultList(result);
		}

		private async Task<IList> ListIgnoreQueryCacheAsync()
		{
			return GetResultList(await (DoListAsync()));
		}

		private async Task<IList> DoListAsync()
		{
			List<IList> results = new List<IList>();
			await (GetResultsFromDatabaseAsync(results));
			return results;
		}

		private async Task GetResultsFromDatabaseAsync(IList results)
		{
			bool statsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
			var stopWatch = new Stopwatch();
			if (statsEnabled)
			{
				stopWatch.Start();
			}

			int rowCount = 0;
			try
			{
				using (var reader = await (resultSetsCommand.GetReaderAsync(null)))
				{
					var hydratedObjects = new List<object>[loaders.Count];
					List<EntityKey[]>[] subselectResultKeys = new List<EntityKey[]>[loaders.Count];
					bool[] createSubselects = new bool[loaders.Count];
					for (int i = 0; i < loaders.Count; i++)
					{
						CriteriaLoader loader = loaders[i];
						int entitySpan = loader.EntityPersisters.Length;
						hydratedObjects[i] = entitySpan == 0 ? null : new List<object>(entitySpan);
						EntityKey[] keys = new EntityKey[entitySpan];
						QueryParameters queryParameters = parameters[i];
						IList tmpResults = new List<object>();
						RowSelection selection = parameters[i].RowSelection;
						createSubselects[i] = loader.IsSubselectLoadingEnabled;
						subselectResultKeys[i] = createSubselects[i] ? new List<EntityKey[]>() : null;
						int maxRows = Loader.Loader.HasMaxRows(selection) ? selection.MaxRows : int.MaxValue;
						if (!dialect.SupportsLimitOffset || !loader.UseLimit(selection, dialect))
						{
							await (Loader.Loader.AdvanceAsync(reader, selection));
						}

						int count;
						for (count = 0; count < maxRows && await (reader.ReadAsync()); count++)
						{
							rowCount++;
							object o = await (loader.GetRowFromResultSetAsync(reader, session, queryParameters, loader.GetLockModes(queryParameters.LockModes), null, hydratedObjects[i], keys, true));
							if (createSubselects[i])
							{
								subselectResultKeys[i].Add(keys);
								keys = new EntityKey[entitySpan]; //can't reuse in this case
							}

							tmpResults.Add(o);
						}

						results.Add(tmpResults);
						reader.NextResult();
					}

					for (int i = 0; i < loaders.Count; i++)
					{
						CriteriaLoader loader = loaders[i];
						await (loader.InitializeEntitiesAndCollectionsAsync(hydratedObjects[i], reader, session, session.DefaultReadOnly));
						if (createSubselects[i])
						{
							loader.CreateSubselects(subselectResultKeys[i], parameters[i], session);
						}
					}
				}
			}
			catch (Exception sqle)
			{
				var message = string.Format("Failed to execute multi criteria: [{0}]", resultSetsCommand.Sql);
				log.Error(message, sqle);
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "Failed to execute multi criteria", resultSetsCommand.Sql);
			}

			if (statsEnabled)
			{
				stopWatch.Stop();
				session.Factory.StatisticsImplementor.QueryExecuted(string.Format("{0} queries (MultiCriteria)", loaders.Count), rowCount, stopWatch.Elapsed);
			}
		}

		public async Task<object> GetResultAsync(string key)
		{
			if (criteriaResults == null)
				await (ListAsync());
			int criteriaResultPosition;
			if (!criteriaResultPositions.TryGetValue(key, out criteriaResultPosition))
				throw new InvalidOperationException(String.Format("The key '{0}' is unknown", key));
			return criteriaResults[criteriaResultPosition];
		}
	}
}
#endif
