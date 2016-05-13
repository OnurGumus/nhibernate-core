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
	/// <summary>
	/// Criteria is a simplified API for retrieving entities by composing
	/// <see cref = "Expression"/> objects.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Using criteria is a very convenient approach for functionality like "search" screens
	/// where there is a variable number of conditions to be placed upon the result set.
	/// </para>
	/// <para>
	/// The Session is a factory for ICriteria. Expression instances are usually obtained via
	/// the factory methods on <see cref = "Expression"/>. eg:
	/// </para>
	/// <code>
	/// IList cats = session.CreateCriteria(typeof(Cat))
	/// 	.Add(Expression.Like("name", "Iz%"))
	/// 	.Add(Expression.Gt("weight", minWeight))
	/// 	.AddOrder(Order.Asc("age"))
	/// 	.List();
	/// </code>
	/// You may navigate associations using <see cref = "CreateAlias(string, string)"/>
	/// or <see cref = "CreateCriteria(string)"/>. eg:
	/// <code>
	/// 	IList&lt;Cat&gt; cats = session.CreateCriteria&lt;Cat&gt;
	/// 		.CreateCriteria("kittens")
	/// 		.Add(Expression.like("name", "Iz%"))
	/// 		.List&lt;Cat&gt;();
	/// </code>
	/// <para>
	/// You may specify projection and aggregation using <c>Projection</c> instances obtained
	/// via the factory methods on <c>Projections</c>. eg:
	/// <code>
	/// 	IList&lt;Cat&gt; cats = session.CreateCriteria&lt;Cat&gt;
	/// 		.SetProjection(
	/// 			Projections.ProjectionList()
	/// 				.Add(Projections.RowCount())
	/// 				.Add(Projections.Avg("weight"))
	/// 				.Add(Projections.Max("weight"))
	/// 				.Add(Projections.Min("weight"))
	/// 				.Add(Projections.GroupProperty("color")))
	/// 		.AddOrder(Order.Asc("color"))
	/// 		.List&lt;Cat&gt;();
	/// </code>
	/// </para>
	/// </remarks>
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