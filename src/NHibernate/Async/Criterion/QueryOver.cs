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
			try
			{
				return Task.FromResult<IList<TRoot>>(List());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IList<TRoot>>(ex);
			}
		}

		Task<IList<U>> IQueryOver<TRoot>.ListAsync<U>()
		{
			try
			{
				return Task.FromResult<IList<U>>(List<U>());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IList<U>>(ex);
			}
		}

		Task<TRoot> IQueryOver<TRoot>.SingleOrDefaultAsync()
		{
			try
			{
				return Task.FromResult<TRoot>(SingleOrDefault());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<TRoot>(ex);
			}
		}

		Task<U> IQueryOver<TRoot>.SingleOrDefaultAsync<U>()
		{
			try
			{
				return Task.FromResult<U>(SingleOrDefault<U>());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<U>(ex);
			}
		}

		Task<IEnumerable<TRoot>> IQueryOver<TRoot>.FutureAsync()
		{
			try
			{
				return Task.FromResult<IEnumerable<TRoot>>(Future());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable<TRoot>>(ex);
			}
		}

		Task<IEnumerable<U>> IQueryOver<TRoot>.FutureAsync<U>()
		{
			try
			{
				return Task.FromResult<IEnumerable<U>>(Future<U>());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable<U>>(ex);
			}
		}

		Task<IFutureValue<TRoot>> IQueryOver<TRoot>.FutureValueAsync()
		{
			try
			{
				return Task.FromResult<IFutureValue<TRoot>>(FutureValue());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IFutureValue<TRoot>>(ex);
			}
		}

		Task<IFutureValue<U>> IQueryOver<TRoot>.FutureValueAsync<U>()
		{
			try
			{
				return Task.FromResult<IFutureValue<U>>(FutureValue<U>());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IFutureValue<U>>(ex);
			}
		}
	}
}
#endif
