#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class FutureBatch<TQueryApproach, TMultiApproach>
	{
		private async Task GetResultsAsync()
		{
			var multiApproach = CreateMultiApproach(isCacheable, cacheRegion);
			for (int i = 0; i < queries.Count; i++)
			{
				AddTo(multiApproach, queries[i], resultTypes[i]);
			}

			results = await (GetResultsFromAsync(multiApproach));
			ClearCurrentFutureBatch();
		}

		protected abstract Task<IList> GetResultsFromAsync(TMultiApproach multiApproach);
	}
}
#endif
