#if NET_4_5
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
		public async Task<IList> ListAsync()
		{
			var results = new List<object>();
			await (ListAsync(results));
			return results;
		}

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

		public async Task<IList<T>> ListAsync<T>()
		{
			List<T> results = new List<T>();
			await (ListAsync(results));
			return results;
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

		public Task<IFutureValue<T>> FutureValueAsync<T>()
		{
			try
			{
				return Task.FromResult<IFutureValue<T>>(FutureValue<T>());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IFutureValue<T>>(ex);
			}
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

		public async Task<object> UniqueResultAsync()
		{
			return AbstractQueryImpl.UniqueElement(await (ListAsync()));
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public sealed partial class Subcriteria : ICriteria
		{
			public Task<IList> ListAsync()
			{
				return root.ListAsync();
			}

			public Task<IFutureValue<T>> FutureValueAsync<T>()
			{
				return root.FutureValueAsync<T>();
			}

			public Task<IEnumerable<T>> FutureAsync<T>()
			{
				return root.FutureAsync<T>();
			}

			public Task ListAsync(IList results)
			{
				return root.ListAsync(results);
			}

			public Task<IList<T>> ListAsync<T>()
			{
				return root.ListAsync<T>();
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

			public Task<object> UniqueResultAsync()
			{
				return root.UniqueResultAsync();
			}
		}
	}
}
#endif
