#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IQueryOver<TRoot> : IQueryOver
	{
		/// <summary>
		/// Get the results of the root type and fill the <see cref = "IList&lt;T&gt;"/>
		/// </summary>
		/// <returns>The list filled with the results.</returns>
		Task<IList<TRoot>> ListAsync();
		/// <summary>
		/// Get the results of the root type and fill the <see cref = "IList&lt;T&gt;"/>
		/// </summary>
		/// <returns>The list filled with the results.</returns>
		Task<IList<U>> ListAsync<U>();
		/// <summary>
		/// Short for ToRowCountQuery().SingleOrDefault&lt;int&gt;()
		/// </summary>
		Task<int> RowCountAsync();
		/// <summary>
		/// Short for ToRowCountInt64Query().SingleOrDefault&lt;long&gt;()
		/// </summary>
		Task<long> RowCountInt64Async();
		/// <summary>
		/// Convenience method to return a single instance that matches
		/// the query, or null if the query returns no results.
		/// </summary>
		/// <returns>the single result or <see langword = "null"/></returns>
		/// <exception cref = "HibernateException">
		/// If there is more than one matching result
		/// </exception>
		Task<TRoot> SingleOrDefaultAsync();
		/// <summary>
		/// Override type of <see cref = "SingleOrDefault()"/>.
		/// </summary>
		Task<U> SingleOrDefaultAsync<U>();
		/// <summary>
		/// Get a enumerable that when enumerated will execute
		/// a batch of queries in a single database roundtrip
		/// </summary>
		Task<IEnumerable<TRoot>> FutureAsync();
		/// <summary>
		/// Get a enumerable that when enumerated will execute
		/// a batch of queries in a single database roundtrip
		/// </summary>
		Task<IEnumerable<U>> FutureAsync<U>();
		/// <summary>
		/// Get an IFutureValue instance, whose value can be retrieved through
		/// its Value property. The query is not executed until the Value property
		/// is retrieved, which will execute other Future queries as well in a
		/// single roundtrip
		/// </summary>
		Task<IFutureValue<TRoot>> FutureValueAsync();
		/// <summary>
		/// Get an IFutureValue instance, whose value can be retrieved through
		/// its Value property. The query is not executed until the Value property
		/// is retrieved, which will execute other Future queries as well in a
		/// single roundtrip
		/// </summary>
		Task<IFutureValue<U>> FutureValueAsync<U>();
	}
}
#endif
