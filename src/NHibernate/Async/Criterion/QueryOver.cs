#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion.Lambda;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class QueryOver<TRoot> : QueryOver, IQueryOver<TRoot>
	{
		private Task<IList<TRoot>> ListAsync()
		{
			return criteria.ListAsync<TRoot>();
		}

		private Task<IList<U>> ListAsync<U>()
		{
			return criteria.ListAsync<U>();
		}

		private Task<TRoot> SingleOrDefaultAsync()
		{
			return criteria.UniqueResultAsync<TRoot>();
		}

		private Task<U> SingleOrDefaultAsync<U>()
		{
			return criteria.UniqueResultAsync<U>();
		}

		private Task<IEnumerable<TRoot>> FutureAsync()
		{
			return criteria.FutureAsync<TRoot>();
		}

		private Task<IEnumerable<U>> FutureAsync<U>()
		{
			return criteria.FutureAsync<U>();
		}

		private Task<IFutureValue<TRoot>> FutureValueAsync()
		{
			return criteria.FutureValueAsync<TRoot>();
		}

		private Task<IFutureValue<U>> FutureValueAsync<U>()
		{
			return criteria.FutureValueAsync<U>();
		}

		Task<IList<TRoot>> IQueryOver<TRoot>.ListAsync()
		{
			return ListAsync();
		}

		Task<IList<U>> IQueryOver<TRoot>.ListAsync<U>()
		{
			return ListAsync<U>();
		}

		async Task<int> IQueryOver<TRoot>.RowCountAsync()
		{
			return await (ToRowCountQuery().SingleOrDefaultAsync<int>());
		}

		async Task<long> IQueryOver<TRoot>.RowCountInt64Async()
		{
			return await (ToRowCountInt64Query().SingleOrDefaultAsync<long>());
		}

		Task<TRoot> IQueryOver<TRoot>.SingleOrDefaultAsync()
		{
			return SingleOrDefaultAsync();
		}

		Task<U> IQueryOver<TRoot>.SingleOrDefaultAsync<U>()
		{
			return SingleOrDefaultAsync<U>();
		}

		Task<IEnumerable<TRoot>> IQueryOver<TRoot>.FutureAsync()
		{
			return FutureAsync();
		}

		Task<IEnumerable<U>> IQueryOver<TRoot>.FutureAsync<U>()
		{
			return FutureAsync<U>();
		}

		Task<IFutureValue<TRoot>> IQueryOver<TRoot>.FutureValueAsync()
		{
			return FutureValueAsync();
		}

		Task<IFutureValue<U>> IQueryOver<TRoot>.FutureValueAsync<U>()
		{
			return FutureValueAsync<U>();
		}
	}
}
#endif
