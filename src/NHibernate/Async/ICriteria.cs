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
using System.Collections.Generic;
using System.Threading;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace NHibernate
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial interface ICriteria : ICloneable
	{

		/// <summary>
		/// Get the results
		/// </summary>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns></returns>
		Task<IList> ListAsync(CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Convenience method to return a single instance that matches
		/// the query, or null if the query returns no results.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns>the single result or <see langword="null" /></returns>
		/// <exception cref="HibernateException">
		/// If there is more than one matching result
		/// </exception>
		Task<object> UniqueResultAsync(CancellationToken cancellationToken = default(CancellationToken));
	
		#region NHibernate specific

		/// <summary>
		/// Get the results and fill the <see cref="IList"/>
		/// </summary>
		/// <param name="results">The list to fill with the results.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		Task ListAsync(IList results, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Strongly-typed version of <see cref="List()" />.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		Task<IList<T>> ListAsync<T>(CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Strongly-typed version of <see cref="UniqueResult()" />.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		Task<T> UniqueResultAsync<T>(CancellationToken cancellationToken = default(CancellationToken));

		#endregion
	}
}
