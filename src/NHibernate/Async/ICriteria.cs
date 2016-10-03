#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ICriteria : ICloneable
	{
		/// <summary>
		/// Get the results
		/// </summary>
		/// <returns></returns>
		Task<IList> ListAsync();
		/// <summary>
		/// Convenience method to return a single instance that matches
		/// the query, or null if the query returns no results.
		/// </summary>
		/// <returns>the single result or <see langword = "null"/></returns>
		/// <exception cref = "HibernateException">
		/// If there is more than one matching result
		/// </exception>
		Task<object> UniqueResultAsync();
		/// <summary>
		/// Get a enumerable that when enumerated will execute
		/// a batch of queries in a single database roundtrip
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <returns></returns>
		Task<IEnumerable<T>> FutureAsync<T>();
		/// <summary>
		/// Get an IFutureValue instance, whose value can be retrieved through
		/// its Value property. The query is not executed until the Value property
		/// is retrieved, which will execute other Future queries as well in a
		/// single roundtrip
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <returns></returns>
		Task<IFutureValue<T>> FutureValueAsync<T>();
		/// <summary>
		/// Get the results and fill the <see cref = "IList"/>
		/// </summary>
		/// <param name = "results">The list to fill with the results.</param>
		Task ListAsync(IList results);
		/// <summary>
		/// Strongly-typed version of <see cref = "List()"/>.
		/// </summary>
		Task<IList<T>> ListAsync<T>();
		/// <summary>
		/// Strongly-typed version of <see cref = "UniqueResult()"/>.
		/// </summary>
		Task<T> UniqueResultAsync<T>();
	}
}
#endif
