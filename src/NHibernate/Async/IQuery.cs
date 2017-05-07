﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NHibernate.Transform;
using NHibernate.Type;
using System.Collections.Generic;

namespace NHibernate
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial interface IQuery
	{

		/// <summary>
		/// Return the query results as an <see cref="IEnumerable"/>. If the query contains multiple results
		/// per row, the results are returned in an instance of <c>object[]</c>.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Entities returned as results are initialized on demand. The first SQL query returns
		/// identifiers only.
		/// </p>
		/// <p>
		/// This is a good strategy to use if you expect a high number of the objects
		/// returned to be already loaded in the <see cref="ISession"/> or in the 2nd level cache.
		/// </p>
		/// </remarks>
		Task<IEnumerable> EnumerableAsync();

		/// <summary>
		/// Strongly-typed version of <see cref="Enumerable()"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		Task<IEnumerable<T>> EnumerableAsync<T>();

		/// <summary>
		/// Return the query results as an <see cref="IList"/>. If the query contains multiple results per row,
		/// the results are returned in an instance of <c>object[]</c>.
		/// </summary>
		/// <returns>The <see cref="IList"/> filled with the results.</returns>
		/// <remarks>
		/// This is a good strategy to use if you expect few of the objects being returned are already loaded
		/// or if you want to fill the 2nd level cache.
		/// </remarks>
		Task<IList> ListAsync();

		/// <summary>
		/// Return the query results an place them into the <see cref="IList"/>.
		/// </summary>
		/// <param name="results">The <see cref="IList"/> to place the results in.</param>
		Task ListAsync(IList results);

		/// <summary>
		/// Strongly-typed version of <see cref="List()"/>.
		/// </summary>
		Task<IList<T>> ListAsync<T>();

		/// <summary>
		/// Convenience method to return a single instance that matches
		/// the query, or null if the query returns no results.
		/// </summary>
		/// <returns>the single result or <see langword="null" /></returns>
		/// <exception cref="HibernateException">
		/// Thrown when there is more than one matching result.
		/// </exception>
		Task<object> UniqueResultAsync();

		/// <summary>
		/// Strongly-typed version of <see cref="UniqueResult()"/>.
		/// </summary>
		Task<T> UniqueResultAsync<T>();

		/// <summary>
		/// Execute the update or delete statement.
		/// </summary>
		/// <returns> The number of entities updated or deleted. </returns>
		Task<int> ExecuteUpdateAsync();
	}
}
