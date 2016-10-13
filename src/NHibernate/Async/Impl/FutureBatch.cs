#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

#endif
namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class FutureBatch<TQueryApproach, TMultiApproach>
	{
		private async Task<IList> GetResultsAsync()
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

			results = await (GetResultsFromAsync(multiApproach));
			ClearCurrentFutureBatch();
			return results;
		}

		protected abstract Task<IList> GetResultsFromAsync(TMultiApproach multiApproach);
	}
}
#endif
