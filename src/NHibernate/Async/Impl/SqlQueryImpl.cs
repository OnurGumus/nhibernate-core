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
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Impl
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class SqlQueryImpl : AbstractQueryImpl, ISQLQuery
	{

		public override Task<IList> ListAsync()
		{
			try
			{
				VerifyParameters();
				IDictionary<string, TypedValue> namedParams = NamedParams;
				NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
				QueryParameters qp = GetQueryParameters(namedParams);
				Before();
				try
				{
					return Session.ListAsync(spec, qp);
				}
				finally
				{
					After();
				}
			}
			catch (Exception ex)
			{
				return Task.FromException<IList>(ex);
			}
		}

		public override async Task ListAsync(IList results)
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
			QueryParameters qp = GetQueryParameters(namedParams);

			Before();
			try
			{
				await (Session.ListAsync(spec, qp, results)).ConfigureAwait(false);
			}
			finally
			{
				After();
			}
		}

		public override Task<IList<T>> ListAsync<T>()
		{
			try
			{
				VerifyParameters();
				IDictionary<string, TypedValue> namedParams = NamedParams;
				NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
				QueryParameters qp = GetQueryParameters(namedParams);
				Before();
				try
				{
					return Session.ListAsync<T>(spec, qp);
				}
				finally
				{
					After();
				}
			}
			catch (Exception ex)
			{
				return Task.FromException<IList<T>>(ex);
			}
		}

		public override Task<IEnumerable> EnumerableAsync()
		{
			throw new NotSupportedException("SQL queries do not currently support enumeration");
		}

		public override Task<IEnumerable<T>> EnumerableAsync<T>()
		{
			throw new NotSupportedException("SQL queries do not currently support enumeration");
		}

		public override Task<int> ExecuteUpdateAsync()
		{
			try
			{
				IDictionary<string, TypedValue> namedParams = NamedParams;
				Before();
				try
				{
					return Session.ExecuteNativeUpdateAsync(GenerateQuerySpecification(namedParams), GetQueryParameters(namedParams));
				}
				finally
				{
					After();
				}
			}
			catch (Exception ex)
			{
				return Task.FromException<int>(ex);
			}
		}

		protected internal override Task<IEnumerable<ITranslator>> GetTranslatorsAsync(ISessionImplementor sessionImplementor, QueryParameters queryParameters)
		{
			try
			{
				var yields = new List<ITranslator>();
				// NOTE: updates queryParameters.NamedParameters as (desired) side effect
				ExpandParameterLists(queryParameters.NamedParameters);
				var sqlQuery = this as ISQLQuery;
				yields.Add(new SqlTranslator(sqlQuery, sessionImplementor.Factory));
				return Task.FromResult<IEnumerable<ITranslator>>(yields);
			}
			catch (Exception ex)
			{
				return Task.FromException<IEnumerable<ITranslator>>(ex);
			}
		}
	}
}
