#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Impl;
using NHibernate.Loader.Custom;
using NHibernate.Persister.Entity;
using NHibernate.Transaction;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ISessionImplementor
	{
		/// <summary>
		/// Initialize the collection (if not already initialized)
		/// </summary>
		/// <param name = "collection"></param>
		/// <param name = "writing"></param>
		Task InitializeCollectionAsync(IPersistentCollection collection, bool writing);
		// NH-268
		/// <summary>
		/// Load an instance without checking if it was deleted. If it does not exist and isn't nullable, throw an exception.
		/// This method may create a new proxy or return an existing proxy.
		/// </summary>
		/// <param name = "entityName">The entityName (or class full name) to load.</param>
		/// <param name = "id">The identifier of the object in the database.</param>
		/// <param name = "isNullable">Allow null instance</param>
		/// <param name = "eager">When enabled, the object is eagerly fetched.</param>
		/// <returns>
		/// A proxy of the object or an instance of the object if the <c>persistentClass</c> does not have a proxy.
		/// </returns>
		/// <exception cref = "ObjectNotFoundException">No object could be found with that <c>id</c>.</exception>
		Task<object> InternalLoadAsync(string entityName, object id, bool eager, bool isNullable);
		/// <summary>
		/// Load an instance immediately. Do not return a proxy.
		/// </summary>
		/// <param name = "entityName"></param>
		/// <param name = "id"></param>
		/// <returns></returns>
		Task<object> ImmediateLoadAsync(string entityName, object id);
		/// <summary>
		/// Execute a <c>List()</c> query
		/// </summary>
		/// <param name = "query"></param>
		/// <param name = "parameters"></param>
		/// <returns></returns>
		[Obsolete("Use overload with IQueryExpression")]
		Task<IList> ListAsync(string query, QueryParameters parameters);
		/// <summary>
		/// Execute a <c>List()</c> expression query
		/// </summary>
		/// <param name = "queryExpression"></param>
		/// <param name = "parameters"></param>
		/// <returns></returns>
		Task<IList> ListAsync(IQueryExpression queryExpression, QueryParameters parameters);
		[Obsolete("Use overload with IQueryExpression")]
		Task ListAsync(string query, QueryParameters parameters, IList results);
		Task ListAsync(IQueryExpression queryExpression, QueryParameters queryParameters, IList results);
		/// <summary>
		/// Strongly-typed version of <see cref = "List(string, QueryParameters)"/>
		/// </summary>
		[Obsolete("Use overload with IQueryExpression")]
		Task<IList<T>> ListAsync<T>(string query, QueryParameters queryParameters);
		/// <summary>
		/// Strongly-typed version of <see cref = "List(IQueryExpression, QueryParameters)"/>
		/// </summary>
		Task<IList<T>> ListAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters);
		/// <summary>
		/// Strongly-typed version of <see cref = "List(CriteriaImpl)"/>
		/// </summary>
		Task<IList<T>> ListAsync<T>(CriteriaImpl criteria);
		Task ListAsync(CriteriaImpl criteria, IList results);
		Task<IList> ListAsync(CriteriaImpl criteria);
		/// <summary>
		/// Execute an <c>Iterate()</c> query
		/// </summary>
		/// <param name = "query"></param>
		/// <param name = "parameters"></param>
		/// <returns></returns>
		[Obsolete("Use overload with IQueryExpression")]
		Task<IEnumerable> EnumerableAsync(string query, QueryParameters parameters);
		/// <summary>
		/// Execute an <c>Iterate()</c> query
		/// </summary>
		/// <param name = "query"></param>
		/// <param name = "parameters"></param>
		/// <returns></returns>
		Task<IEnumerable> EnumerableAsync(IQueryExpression query, QueryParameters parameters);
		/// <summary>
		/// Strongly-typed version of <see cref = "Enumerable(string, QueryParameters)"/>
		/// </summary>
		[Obsolete("Use overload with IQueryExpression")]
		Task<IEnumerable<T>> EnumerableAsync<T>(string query, QueryParameters queryParameters);
		/// <summary>
		/// Strongly-typed version of <see cref = "Enumerable(IQueryExpression, QueryParameters)"/>
		/// </summary>
		Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression query, QueryParameters queryParameters);
		/// <summary>
		/// Execute a filter
		/// </summary>
		Task<IList> ListFilterAsync(object collection, string filter, QueryParameters parameters);
		/// <summary>
		/// Execute a filter (strongly-typed version).
		/// </summary>
		Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters parameters);
		/// <summary>
		/// Collection from a filter
		/// </summary>
		Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters parameters);
		/// <summary>
		/// Strongly-typed version of <see cref = "EnumerableFilter(object, string, QueryParameters)"/>
		/// </summary>
		Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters parameters);
		/// <summary>
		/// Execute an SQL Query
		/// </summary>
		Task<IList> ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters);
		Task ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters, IList results);
		/// <summary>
		/// Strongly-typed version of <see cref = "List(NativeSQLQuerySpecification, QueryParameters)"/>
		/// </summary>
		Task<IList<T>> ListAsync<T>(NativeSQLQuerySpecification spec, QueryParameters queryParameters);
		/// <summary> Execute an SQL Query</summary>
		Task ListCustomQueryAsync(ICustomQuery customQuery, QueryParameters queryParameters, IList results);
		Task<IList<T>> ListCustomQueryAsync<T>(ICustomQuery customQuery, QueryParameters queryParameters);
		[Obsolete("Use overload with IQueryExpression")]
		Task<IQueryTranslator[]> GetQueriesAsync(string query, bool scalar); // NH specific for MultiQuery
		Task<IQueryTranslator[]> GetQueriesAsync(IQueryExpression query, bool scalar); // NH specific for MultiQuery
		/// <summary> 
		/// Get the entity instance associated with the given <tt>Key</tt>,
		/// calling the Interceptor if necessary
		/// </summary>
		Task<object> GetEntityUsingInterceptorAsync(EntityKey key);
		/// <summary> The best guess entity name for an entity not in an association</summary>
		Task<string> BestGuessEntityNameAsync(object entity);
		Task FlushAsync();
		/// <summary> Execute a native SQL update or delete query</summary>
		Task<int> ExecuteNativeUpdateAsync(NativeSQLQuerySpecification specification, QueryParameters queryParameters);
		/// <summary> Execute a HQL update or delete query</summary>
		[Obsolete("Use overload with IQueryExpression")]
		Task<int> ExecuteUpdateAsync(string query, QueryParameters queryParameters);
		/// <summary> Execute a HQL update or delete query</summary>
		Task<int> ExecuteUpdateAsync(IQueryExpression query, QueryParameters queryParameters);
	}
}
#endif
