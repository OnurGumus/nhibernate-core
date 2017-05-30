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

namespace NHibernate.Impl
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public abstract partial class AbstractSessionImpl : ISessionImplementor
	{

		#region ISessionImplementor Members

		public abstract Task InitializeCollectionAsync(IPersistentCollection collection, bool writing, CancellationToken cancellationToken);
		public abstract Task<object> InternalLoadAsync(string entityName, object id, bool eager, bool isNullable, CancellationToken cancellationToken);
		public abstract Task<object> ImmediateLoadAsync(string entityName, object id, CancellationToken cancellationToken);

		public virtual async Task<IList> ListAsync(IQueryExpression queryExpression, QueryParameters parameters, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var results = (IList) typeof (List<>).MakeGenericType(queryExpression.Type)
												 .GetConstructor(System.Type.EmptyTypes)
												 .Invoke(null);
			await (ListAsync(queryExpression, parameters, results, cancellationToken)).ConfigureAwait(false);
			return results;
		}

		public abstract Task ListAsync(IQueryExpression queryExpression, QueryParameters queryParameters, IList results, CancellationToken cancellationToken);

		public virtual async Task<IList<T>> ListAsync<T>(IQueryExpression query, QueryParameters parameters, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListAsync(query, parameters, results, cancellationToken)).ConfigureAwait(false);
				return results;
			}
		}

		public virtual async Task<IList<T>> ListAsync<T>(CriteriaImpl criteria, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListAsync(criteria, results, cancellationToken)).ConfigureAwait(false);
				return results;
			}
		}

		public abstract Task ListAsync(CriteriaImpl criteria, IList results, CancellationToken cancellationToken);
		
		public virtual async Task<IList> ListAsync(CriteriaImpl criteria, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<object>();
				await (ListAsync(criteria, results, cancellationToken)).ConfigureAwait(false);
				return results;
			}
		}

		public abstract Task<IList> ListFilterAsync(object collection, string filter, QueryParameters parameters, CancellationToken cancellationToken);
		public abstract Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters parameters, CancellationToken cancellationToken);
		public abstract Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters parameters, CancellationToken cancellationToken);
		public abstract Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters parameters, CancellationToken cancellationToken);

		public virtual async Task<IList> ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<object>();
				await (ListAsync(spec, queryParameters, results, cancellationToken)).ConfigureAwait(false);
				return results;
			}
		}

		public virtual async Task ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters, IList results, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (new SessionIdLoggingContext(SessionId))
			{
				var query = new SQLCustomQuery(
					spec.SqlQueryReturns,
					spec.QueryString,
					spec.QuerySpaces,
					Factory);
				await (ListCustomQueryAsync(query, queryParameters, results, cancellationToken)).ConfigureAwait(false);
			}
		}

		public virtual async Task<IList<T>> ListAsync<T>(NativeSQLQuerySpecification spec, QueryParameters queryParameters, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListAsync(spec, queryParameters, results, cancellationToken)).ConfigureAwait(false);
				return results;
			}
		}

		public abstract Task ListCustomQueryAsync(ICustomQuery customQuery, QueryParameters queryParameters, IList results, CancellationToken cancellationToken);

		public virtual async Task<IList<T>> ListCustomQueryAsync<T>(ICustomQuery customQuery, QueryParameters queryParameters, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			using (new SessionIdLoggingContext(SessionId))
			{
				var results = new List<T>();
				await (ListCustomQueryAsync(customQuery, queryParameters, results, cancellationToken)).ConfigureAwait(false);
				return results;
			}
		}
		
		public abstract Task<IQueryTranslator[]> GetQueriesAsync(IQueryExpression query, bool scalar, CancellationToken cancellationToken);
		public abstract Task<object> GetEntityUsingInterceptorAsync(EntityKey key, CancellationToken cancellationToken);
		public abstract Task<int> ExecuteNativeUpdateAsync(NativeSQLQuerySpecification specification, QueryParameters queryParameters, CancellationToken cancellationToken);

		public abstract Task FlushAsync(CancellationToken cancellationToken);

		#endregion

		public abstract Task<IEnumerable> EnumerableAsync(IQueryExpression queryExpression, QueryParameters queryParameters, CancellationToken cancellationToken);

		public abstract Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters, CancellationToken cancellationToken);

		public abstract Task<int> ExecuteUpdateAsync(IQueryExpression queryExpression, QueryParameters queryParameters, CancellationToken cancellationToken);
	}
}
