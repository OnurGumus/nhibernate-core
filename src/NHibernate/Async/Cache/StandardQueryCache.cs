#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Cache
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StandardQueryCache : IQueryCache
	{
		public async Task<bool> PutAsync(QueryKey key, ICacheAssembler[] returnTypes, IList result, bool isNaturalKeyLookup, ISessionImplementor session)
		{
			if (isNaturalKeyLookup && result.Count == 0)
				return false;
			long ts = session.Timestamp;
			if (Log.IsDebugEnabled)
				Log.DebugFormat("caching query results in region: '{0}'; {1}", _regionName, key);
			IList cacheable = new List<object>(result.Count + 1)
			{ts};
			for (int i = 0; i < result.Count; i++)
			{
				if (returnTypes.Length == 1)
				{
					cacheable.Add(await (returnTypes[0].DisassembleAsync(result[i], session, null)));
				}
				else
				{
					cacheable.Add(await (TypeHelper.DisassembleAsync((object[])result[i], returnTypes, null, session, null)));
				}
			}

			_queryCache.Put(key, cacheable);
			return true;
		}

		public async Task<IList> GetAsync(QueryKey key, ICacheAssembler[] returnTypes, bool isNaturalKeyLookup, ISet<string> spaces, ISessionImplementor session)
		{
			if (Log.IsDebugEnabled)
				Log.DebugFormat("checking cached query results in region: '{0}'; {1}", _regionName, key);
			var cacheable = (IList)_queryCache.Get(key);
			if (cacheable == null)
			{
				Log.DebugFormat("query results were not found in cache: {0}", key);
				return null;
			}

			var timestamp = (long)cacheable[0];
			if (Log.IsDebugEnabled)
				Log.DebugFormat("Checking query spaces for up-to-dateness [{0}]", StringHelper.CollectionToString(spaces));
			if (!isNaturalKeyLookup && !IsUpToDate(spaces, timestamp))
			{
				Log.DebugFormat("cached query results were not up to date for: {0}", key);
				return null;
			}

			Log.DebugFormat("returning cached query results for: {0}", key);
			for (int i = 1; i < cacheable.Count; i++)
			{
				if (returnTypes.Length == 1)
				{
					await (returnTypes[0].BeforeAssembleAsync(cacheable[i], session));
				}
				else
				{
					await (TypeHelper.BeforeAssembleAsync((object[])cacheable[i], returnTypes, session));
				}
			}

			IList result = new List<object>(cacheable.Count - 1);
			for (int i = 1; i < cacheable.Count; i++)
			{
				try
				{
					if (returnTypes.Length == 1)
					{
						result.Add(await (returnTypes[0].AssembleAsync(cacheable[i], session, null)));
					}
					else
					{
						result.Add(await (TypeHelper.AssembleAsync((object[])cacheable[i], returnTypes, session, null)));
					}
				}
				catch (UnresolvableObjectException)
				{
					if (isNaturalKeyLookup)
					{
						//TODO: not really completely correct, since
						//      the UnresolvableObjectException could occur while resolving
						//      associations, leaving the PC in an inconsistent state
						Log.Debug("could not reassemble cached result set");
						_queryCache.Remove(key);
						return null;
					}

					throw;
				}
			}

			return result;
		}
	}
}
#endif
