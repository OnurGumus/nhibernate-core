using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion.Lambda;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class QueryOver<TRoot> : QueryOver, IQueryOver<TRoot>
	{
		private async Task<TRoot> SingleOrDefaultAsync()
		{
			return await (criteria.UniqueResultAsync<TRoot>());
		}

		async Task<TRoot> IQueryOver<TRoot>.SingleOrDefaultAsync()
		{
			return SingleOrDefault();
		}

		async Task<U> IQueryOver<TRoot>.SingleOrDefaultAsync<U>()
		{
			return SingleOrDefault<U>();
		}

		private async Task<U> SingleOrDefaultAsync<U>()
		{
			return await (criteria.UniqueResultAsync<U>());
		}

		private async Task<IList<TRoot>> ListAsync()
		{
			return await (criteria.ListAsync<TRoot>());
		}

		async Task<IList<TRoot>> IQueryOver<TRoot>.ListAsync()
		{
			return List();
		}

		async Task<IList<U>> IQueryOver<TRoot>.ListAsync<U>()
		{
			return List<U>();
		}

		private async Task<IList<U>> ListAsync<U>()
		{
			return await (criteria.ListAsync<U>());
		}

		private async Task<IFutureValue<U>> FutureValueAsync<U>()
		{
			return await (criteria.FutureValueAsync<U>());
		}

		async Task<IFutureValue<TRoot>> IQueryOver<TRoot>.FutureValueAsync()
		{
			return FutureValue();
		}

		async Task<IFutureValue<U>> IQueryOver<TRoot>.FutureValueAsync<U>()
		{
			return FutureValue<U>();
		}

		private async Task<IFutureValue<TRoot>> FutureValueAsync()
		{
			return await (criteria.FutureValueAsync<TRoot>());
		}

		private async Task<IEnumerable<U>> FutureAsync<U>()
		{
			return await (criteria.FutureAsync<U>());
		}

		async Task<IEnumerable<TRoot>> IQueryOver<TRoot>.FutureAsync()
		{
			return Future();
		}

		async Task<IEnumerable<U>> IQueryOver<TRoot>.FutureAsync<U>()
		{
			return Future<U>();
		}

		private async Task<IEnumerable<TRoot>> FutureAsync()
		{
			return await (criteria.FutureAsync<TRoot>());
		}
	}
}