#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Hql;
using NHibernate.Loader.Custom;
using NHibernate.Loader.Custom.Sql;
using NHibernate.Persister.Entity;
using NHibernate.Transaction;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractSessionImpl : ISessionImplementor
	{
		public abstract Task InitializeCollectionAsync(IPersistentCollection collection, bool writing);
		public abstract Task<object> InternalLoadAsync(string entityName, object id, bool eager, bool isNullable);
		public abstract Task<object> ImmediateLoadAsync(string entityName, object id);
		[Obsolete("Use overload with IQueryExpression")]
		public virtual async Task<IList> ListAsync(string query, QueryParameters parameters)
		{
			return await (ListAsync(query.ToQueryExpression(), parameters));
		}

		[Obsolete("Use overload with IQueryExpression")]
		public virtual async Task ListAsync(string query, QueryParameters queryParameters, IList results)
		{
			await (ListAsync(query.ToQueryExpression(), queryParameters, results));
		}

		[Obsolete("Use overload with IQueryExpression")]
		public virtual async Task<IList<T>> ListAsync<T>(string query, QueryParameters queryParameters)
		{
			return await (ListAsync<T>(query.ToQueryExpression(), queryParameters));
		}

		public virtual async Task<IList> ListAsync(IQueryExpression queryExpression, QueryParameters parameters)
		{
			var results = (IList)typeof (List<>).MakeGenericType(queryExpression.Type).GetConstructor(System.Type.EmptyTypes).Invoke(null);
			await (ListAsync(queryExpression, parameters, results));
			return results;
		}

		public abstract Task ListAsync(IQueryExpression queryExpression, QueryParameters queryParameters, IList results);
		public virtual async Task<IList<T>> ListAsync<T>(IQueryExpression query, QueryParameters parameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListAsync(query, parameters, results));
				return results;
			}
		}

		public virtual async Task<IList<T>> ListAsync<T>(CriteriaImpl criteria)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListAsync(criteria, results));
				return results;
			}
		}

		public abstract Task ListAsync(CriteriaImpl criteria, IList results);
		public virtual async Task<IList> ListAsync(CriteriaImpl criteria)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<object>();
				await (ListAsync(criteria, results));
				return results;
			}
		}

		public abstract Task<IList> ListFilterAsync(object collection, string filter, QueryParameters parameters);
		public abstract Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters parameters);
		public abstract Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters parameters);
		public abstract Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters parameters);
		public virtual async Task<IList> ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<object>();
				await (ListAsync(spec, queryParameters, results));
				return results;
			}
		}

		public virtual async Task ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters, IList results)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var query = new SQLCustomQuery(spec.SqlQueryReturns, spec.QueryString, spec.QuerySpaces, Factory);
				await (ListCustomQueryAsync(query, queryParameters, results));
			}
		}

		public virtual async Task<IList<T>> ListAsync<T>(NativeSQLQuerySpecification spec, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListAsync(spec, queryParameters, results));
				return results;
			}
		}

		public abstract Task ListCustomQueryAsync(ICustomQuery customQuery, QueryParameters queryParameters, IList results);
		public virtual async Task<IList<T>> ListCustomQueryAsync<T>(ICustomQuery customQuery, QueryParameters queryParameters)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListCustomQueryAsync(customQuery, queryParameters, results));
				return results;
			}
		}

		[Obsolete("Use overload with IQueryExpression")]
		public virtual async Task<IQueryTranslator[]> GetQueriesAsync(string query, bool scalar)
		{
			return await (GetQueriesAsync(query.ToQueryExpression(), scalar));
		}

		public abstract Task<IQueryTranslator[]> GetQueriesAsync(IQueryExpression query, bool scalar);
		public abstract Task<object> GetEntityUsingInterceptorAsync(EntityKey key);
		public abstract Task<string> BestGuessEntityNameAsync(object entity);
		public abstract Task<int> ExecuteNativeUpdateAsync(NativeSQLQuerySpecification specification, QueryParameters queryParameters);
		public abstract Task FlushAsync();
		public abstract Task<IEnumerable> EnumerableAsync(IQueryExpression queryExpression, QueryParameters queryParameters);
		[Obsolete("Use overload with IQueryExpression")]
		public virtual async Task<IEnumerable> EnumerableAsync(string query, QueryParameters queryParameters)
		{
			return await (EnumerableAsync(query.ToQueryExpression(), queryParameters));
		}

		[Obsolete("Use overload with IQueryExpression")]
		public virtual async Task<IEnumerable<T>> EnumerableAsync<T>(string query, QueryParameters queryParameters)
		{
			return await (EnumerableAsync<T>(query.ToQueryExpression(), queryParameters));
		}

		public abstract Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters);
		[Obsolete("Use overload with IQueryExpression")]
		public virtual async Task<int> ExecuteUpdateAsync(string query, QueryParameters queryParameters)
		{
			return await (ExecuteUpdateAsync(query.ToQueryExpression(), queryParameters));
		}

		public abstract Task<int> ExecuteUpdateAsync(IQueryExpression queryExpression, QueryParameters queryParameters);
	}
}
#endif
