using System.Collections;
using System.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	internal partial class MultipleQueriesCacheAssembler : ICacheAssembler
	{
		public async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			IList srcList = (IList)value;
			var cacheable = new List<object>();
			for (int i = 0; i < srcList.Count; i++)
			{
				ICacheAssembler[] assemblers = (ICacheAssembler[])assemblersList[i];
				IList itemList = (IList)srcList[i];
				var singleQueryCached = new List<object>();
				foreach (object objToCache in itemList)
				{
					if (assemblers.Length == 1)
					{
						singleQueryCached.Add(await (assemblers[0].DisassembleAsync(objToCache, session, owner)));
					}
					else
					{
						singleQueryCached.Add(await (TypeHelper.DisassembleAsync((object[])objToCache, assemblers, null, session, null)));
					}
				}

				cacheable.Add(singleQueryCached);
			}

			return cacheable;
		}

		public async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			IList srcList = (IList)cached;
			var result = new List<object>();
			for (int i = 0; i < assemblersList.Count; i++)
			{
				ICacheAssembler[] assemblers = (ICacheAssembler[])assemblersList[i];
				IList queryFromCache = (IList)srcList[i];
				var queryResults = new List<object>();
				foreach (object fromCache in queryFromCache)
				{
					if (assemblers.Length == 1)
					{
						queryResults.Add(await (assemblers[0].AssembleAsync(fromCache, session, owner)));
					}
					else
					{
						queryResults.Add(await (TypeHelper.AssembleAsync((object[])fromCache, assemblers, session, owner)));
					}
				}

				result.Add(queryResults);
			}

			return result;
		}

		public Task BeforeAssembleAsync(object cached, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		}

		public async Task<IList> GetResultFromQueryCacheAsync(ISessionImplementor session, QueryParameters queryParameters, ISet<string> querySpaces, IQueryCache queryCache, QueryKey key)
		{
			if (!queryParameters.ForceCacheRefresh)
			{
				IList list = await (queryCache.GetAsync(key, new ICacheAssembler[]{this}, queryParameters.NaturalKeyLookup, querySpaces, session));
				//we had to wrap the query results in another list in order to save all
				//the queries in the same bucket, now we need to do it the other way around.
				if (list != null)
				{
					list = (IList)list[0];
				}

				return list;
			}

			return null;
		}
	}
}