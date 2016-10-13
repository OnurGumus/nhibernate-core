#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Hql;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Linq;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractQueryImpl : IQuery
	{
		public async Task<IQuery> SetEntityAsync(int position, object val)
		{
			SetParameter(position, val, NHibernateUtil.Entity(await (NHibernateProxyHelper.GuessClassAsync(val))));
			return this;
		}

		public async Task<IQuery> SetEntityAsync(string name, object val)
		{
			SetParameter(name, val, NHibernateUtil.Entity(await (NHibernateProxyHelper.GuessClassAsync(val))));
			return this;
		}

		public abstract Task<int> ExecuteUpdateAsync();
		public abstract Task<IEnumerable> EnumerableAsync();
		public abstract Task<IEnumerable<T>> EnumerableAsync<T>();
		public abstract Task<IList> ListAsync();
		public abstract Task ListAsync(IList results);
		public abstract Task<IList<T>> ListAsync<T>();
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

		public async Task<object> UniqueResultAsync()
		{
			return UniqueElement(await (ListAsync()));
		}

		protected internal abstract Task<IEnumerable<ITranslator>> GetTranslatorsAsync(ISessionImplementor sessionImplementor, QueryParameters queryParameters);
	}
}
#endif
