﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	/// <content>
	/// Contains generated async methods
	/// </content>
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
			results = await (GetResultsFromAsync(multiApproach)).ConfigureAwait(false);
			ClearCurrentFutureBatch();
			return results;
		}
		protected abstract Task<IList> GetResultsFromAsync(TMultiApproach multiApproach);
	}
}