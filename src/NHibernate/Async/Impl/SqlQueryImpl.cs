using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	/// <summary>
	/// Implements SQL query passthrough
	/// </summary>
	/// <example>
	/// An example mapping is:
	/// <code>
	/// &lt;sql-query-name name="mySqlQuery"&gt;
	/// &lt;return alias="person" class="eg.Person" /&gt;
	///		SELECT {person}.NAME AS {person.name}, {person}.AGE AS {person.age}, {person}.SEX AS {person.sex}
	///		FROM PERSON {person} WHERE {person}.NAME LIKE 'Hiber%'
	/// &lt;/sql-query-name&gt;
	/// </code>
	/// </example>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlQueryImpl : AbstractQueryImpl, ISQLQuery
	{
		public override async Task<IList> ListAsync()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
			QueryParameters qp = GetQueryParameters(namedParams);
			Before();
			try
			{
				return await (Session.ListAsync(spec, qp));
			}
			finally
			{
				After();
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
				await (Session.ListAsync(spec, qp, results));
			}
			finally
			{
				After();
			}
		}

		public override async Task<IList<T>> ListAsync<T>()
		{
			VerifyParameters();
			IDictionary<string, TypedValue> namedParams = NamedParams;
			NativeSQLQuerySpecification spec = GenerateQuerySpecification(namedParams);
			QueryParameters qp = GetQueryParameters(namedParams);
			Before();
			try
			{
				return await (Session.ListAsync<T>(spec, qp));
			}
			finally
			{
				After();
			}
		}

		public override async Task<IEnumerable> EnumerableAsync()
		{
			throw new NotSupportedException("SQL queries do not currently support enumeration");
		}

		public override async Task<IEnumerable<T>> EnumerableAsync<T>()
		{
			throw new NotSupportedException("SQL queries do not currently support enumeration");
		}

		protected internal override async Task<IEnumerable<ITranslator>> GetTranslatorsAsync(ISessionImplementor sessionImplementor, QueryParameters queryParameters)
		{
			// NOTE: updates queryParameters.NamedParameters as (desired) side effect
			ExpandParameterLists(queryParameters.NamedParameters);
			var sqlQuery = this as ISQLQuery;
			yield return new SqlTranslator(sqlQuery, sessionImplementor.Factory);
		}

		public override async Task<int> ExecuteUpdateAsync()
		{
			IDictionary<string, TypedValue> namedParams = NamedParams;
			Before();
			try
			{
				return await (Session.ExecuteNativeUpdateAsync(GenerateQuerySpecification(namedParams), GetQueryParameters(namedParams)));
			}
			finally
			{
				After();
			}
		}
	}
}