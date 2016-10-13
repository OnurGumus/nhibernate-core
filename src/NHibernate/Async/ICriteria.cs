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
