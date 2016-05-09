using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CriteriaImpl : ICriteria
	{
		public async Task ListAsync(IList results)
		{
			Before();
			try
			{
				await (session.ListAsync(this, results));
			}
			finally
			{
				After();
			}
		}

		public async Task<IList> ListAsync()
		{
			var results = new List<object>();
			await (ListAsync(results));
			return results;
		}

		public async Task<IList<T>> ListAsync<T>()
		{
			List<T> results = new List<T>();
			await (ListAsync(results));
			return results;
		}

		public async Task<object> UniqueResultAsync()
		{
			return AbstractQueryImpl.UniqueElement(await (ListAsync()));
		}

		public async Task<T> UniqueResultAsync<T>()
		{
			object result = await (UniqueResultAsync());
			if (result == null && typeof (T).IsValueType)
			{
				return default (T);
			}
			else
			{
				return (T)result;
			}
		}

		public async Task<IFutureValue<T>> FutureValueAsync<T>()
		{
			if (!session.Factory.ConnectionProvider.Driver.SupportsMultipleQueries)
			{
				return new FutureValue<T>(ListAsync<T>);
			}

			session.FutureCriteriaBatch.Add<T>(this);
			return session.FutureCriteriaBatch.GetFutureValue<T>();
		}

		public async Task<IEnumerable<T>> FutureAsync<T>()
		{
			if (!session.Factory.ConnectionProvider.Driver.SupportsMultipleQueries)
			{
				return await (ListAsync<T>());
			}

			session.FutureCriteriaBatch.Add<T>(this);
			return session.FutureCriteriaBatch.GetEnumerator<T>();
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public sealed partial class Subcriteria : ICriteria
		{
			public async Task<IList> ListAsync()
			{
				return await (root.ListAsync());
			}

			public async Task ListAsync(IList results)
			{
				await (root.ListAsync(results));
			}

			public async Task<IList<T>> ListAsync<T>()
			{
				return await (root.ListAsync<T>());
			}

			public async Task<object> UniqueResultAsync()
			{
				return await (root.UniqueResultAsync());
			}

			public async Task<T> UniqueResultAsync<T>()
			{
				object result = await (UniqueResultAsync());
				if (result == null && typeof (T).IsValueType)
				{
					throw new InvalidCastException("UniqueResult<T>() cannot cast null result to value type. Call UniqueResult<T?>() instead");
				}
				else
				{
					return (T)result;
				}
			}

			public async Task<IFutureValue<T>> FutureValueAsync<T>()
			{
				return await (root.FutureValueAsync<T>());
			}

			public async Task<IEnumerable<T>> FutureAsync<T>()
			{
				return await (root.FutureAsync<T>());
			}
		}
	}
}