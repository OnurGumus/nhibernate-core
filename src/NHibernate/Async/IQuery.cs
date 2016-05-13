using System;
using System.Collections;
using NHibernate.Transform;
using NHibernate.Type;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// An object-oriented representation of a NHibernate query.
	/// </summary>
	/// <remarks>
	/// An <c>IQuery</c> instance is obtained by calling <see cref = "ISession.CreateQuery(string)"/>.
	/// Key features of this interface include:
	/// <list type = "bullet">
	///		<item>
	///			Paging: A particular page of the result set may be selected by calling
	///			<see cref = "SetMaxResults(int)"/>, <see cref = "SetFirstResult(int)"/>.  The generated SQL
	///			depends on the capabilities of the <see cref = "Dialect.Dialect"/>.  Some
	///			Dialects are for databases that have built in paging (LIMIT) and those capabilities
	///			will be used to limit the number of records returned by the SQL statement.
	///			If the database does not support LIMITs then all of the records will be returned,
	///			but the objects created will be limited to the specific results requested.
	///		</item>
	///		<item>
	///			Named parameters
	///		</item>
	///		<item>
	///			Ability to return 'read-only' entities
	///		</item>
	/// </list>
	/// <para>
	/// Named query parameters are tokens of the form <c>:name</c> in the query string. For example, a
	/// value is bound to the <c>Int32</c> parameter <c>:foo</c> by calling:
	/// <code>
	/// SetParameter("foo", foo, NHibernateUtil.Int32);
	/// </code>
	/// A name may appear multiple times in the query string.
	/// </para>
	/// <para>
	/// Unnamed parameters <c>?</c> are also supported. To bind a value to an unnamed
	/// parameter use a Set method that accepts an <c>Int32</c> positional argument - numbered from
	/// zero.
	/// </para>
	/// <para>
	/// You may not mix and match unnamed parameters and named parameters in the same query.
	/// </para>
	/// <para>
	/// Queries are executed by calling <see cref = "IQuery.List()"/> or <see cref = "IQuery.Enumerable()"/>. A query
	/// may be re-executed by subsequent invocations. Its lifespan is, however, bounded by the lifespan
	/// of the <c>ISession</c> that created it.
	/// </para>
	/// <para>
	/// Implementors are not intended to be threadsafe.
	/// </para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IQuery
	{
		/// <summary>
		/// Return the query results as an <see cref = "IEnumerable"/>. If the query contains multiple results
		/// per row, the results are returned in an instance of <c>object[]</c>.
		/// </summary>
		/// <remarks>
		/// <p>
		/// Entities returned as results are initialized on demand. The first SQL query returns
		/// identifiers only.
		/// </p>
		/// <p>
		/// This is a good strategy to use if you expect a high number of the objects
		/// returned to be already loaded in the <see cref = "ISession"/> or in the 2nd level cache.
		/// </p>
		/// </remarks>
		Task<IEnumerable> EnumerableAsync();
		/// <summary>
		/// Strongly-typed version of <see cref = "Enumerable()"/>.
		/// </summary>
		/// <typeparam name = "T"></typeparam>
		/// <returns></returns>
		Task<IEnumerable<T>> EnumerableAsync<T>();
		/// <summary>
		/// Return the query results as an <see cref = "IList"/>. If the query contains multiple results per row,
		/// the results are returned in an instance of <c>object[]</c>.
		/// </summary>
		/// <returns>The <see cref = "IList"/> filled with the results.</returns>
		/// <remarks>
		/// This is a good strategy to use if you expect few of the objects being returned are already loaded
		/// or if you want to fill the 2nd level cache.
		/// </remarks>
		Task<IList> ListAsync();
		/// <summary>
		/// Return the query results an place them into the <see cref = "IList"/>.
		/// </summary>
		/// <param name = "results">The <see cref = "IList"/> to place the results in.</param>
		Task ListAsync(IList results);
		/// <summary>
		/// Strongly-typed version of <see cref = "List()"/>.
		/// </summary>
		Task<IList<T>> ListAsync<T>();
		/// <summary>
		/// Convenience method to return a single instance that matches
		/// the query, or null if the query returns no results.
		/// </summary>
		/// <returns>the single result or <see langword = "null"/></returns>
		/// <exception cref = "HibernateException">
		/// Thrown when there is more than one matching result.
		/// </exception>
		Task<object> UniqueResultAsync();
		/// <summary>
		/// Strongly-typed version of <see cref = "UniqueResult()"/>.
		/// </summary>
		Task<T> UniqueResultAsync<T>();
		/// <summary>
		/// Execute the update or delete statement.
		/// </summary>
		/// <returns> The number of entities updated or deleted. </returns>
		Task<int> ExecuteUpdateAsync();
		/// <summary>
		/// Bind an instance of a mapped persistent class to an indexed parameter.
		/// </summary>
		/// <param name = "position">Position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name = "val">A non-null instance of a persistent class</param>
		Task<IQuery> SetEntityAsync(int position, object val);
		/// <summary>
		/// Bind an instance of a mapped persistent class to a named parameter.
		/// </summary>
		/// <param name = "name">The name of the parameter</param>
		/// <param name = "val">A non-null instance of a persistent class</param>
		Task<IQuery> SetEntityAsync(string name, object val);
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
	}
}