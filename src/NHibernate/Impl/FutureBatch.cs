using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	public abstract class FutureBatch<TQueryApproach, TMultiApproach>
	{
		private readonly List<TQueryApproach> queries = new List<TQueryApproach>();
		private readonly IList<System.Type> resultTypes = new List<System.Type>();
		private int index;
		private IList results;
		private bool isCacheable = true;
		private string cacheRegion;

		protected readonly SessionImpl session;

		protected FutureBatch(SessionImpl session)
		{
			this.session = session;
		}

		public void Add<TResult>(TQueryApproach query)
		{
			if (queries.Count == 0)
			{
				cacheRegion = CacheRegion(query);
			}

			queries.Add(query);
			resultTypes.Add(typeof(TResult));
			index = queries.Count - 1;
			isCacheable = isCacheable && IsQueryCacheable(query);
			isCacheable = isCacheable && (cacheRegion == CacheRegion(query));
		}

		public void Add(TQueryApproach query)
		{
			Add<object>(query);
		}

		public IFutureValue<TResult> GetFutureValue<TResult>()
		{
			int currentIndex = index;
			return new FutureValue<TResult>(
				() =>
				{
					try
					{
						return GetCurrentResult<TResult>(currentIndex, false).Result;
					}
					catch (AggregateException e)
					{
						throw e.InnerException;
					}
				});
		}

		public IFutureValueAsync<TResult> GetFutureValueAsync<TResult>()
		{
			int currentIndex = index;
			return new FutureValueAsync<TResult>(async () => await GetCurrentResult<TResult>(currentIndex, true));
		}

		public IEnumerable<TResult> GetEnumerator<TResult>()
		{
			int currentIndex = index;
			return new DelayedEnumerator<TResult>(
				() =>
				{
					try
					{
						return GetCurrentResult<TResult>(currentIndex, false).Result;
					}
					catch (AggregateException e)
					{
						throw e.InnerException;
					}
				});
		}

		public IAsyncEnumerable<TResult> GetAsyncEnumerator<TResult>()
		{
			int currentIndex = index;
			return new DelayedAsyncEnumerator<TResult>(async () => await GetCurrentResult<TResult>(currentIndex, true));
		}

		private async Task<IList> GetResults(bool async)
		{
			if (results != null)
			{
				return results;
			}
			var multiApproach = CreateMultiApproach(isCacheable, cacheRegion);
			for (int i = 0; i < queries.Count; i++)
			{
				AddTo(multiApproach, queries[i], resultTypes[i]);
			}
			results = await GetResultsFrom(multiApproach, async);
			ClearCurrentFutureBatch();
			return results;
		}

		private async Task<IEnumerable<TResult>> GetCurrentResult<TResult>(int currentIndex, bool async)
		{
			var result = await GetResults(async);
			return ((IList) (result)[currentIndex]).Cast<TResult>();
		}

		protected abstract TMultiApproach CreateMultiApproach(bool isCacheable, string cacheRegion);
		protected abstract void AddTo(TMultiApproach multiApproach, TQueryApproach query, System.Type resultType);
		protected abstract Task<IList> GetResultsFrom(TMultiApproach multiApproach, bool async);
		protected abstract void ClearCurrentFutureBatch();
		protected abstract bool IsQueryCacheable(TQueryApproach query);
		protected abstract string CacheRegion(TQueryApproach query);
	}
}