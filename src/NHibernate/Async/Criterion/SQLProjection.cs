using System;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	using System.Collections.Generic;
	using Engine;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class SQLProjection : IProjection
	{
		public async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			//SqlString result = new SqlString(criteriaQuery.GetSQLAlias(criteria));
			//result.Replace(sql, "{alias}");
			//return result;
			return new SqlString(StringHelper.Replace(sql, "{alias}", criteriaQuery.GetSQLAlias(criteria)));
		}

		/// <summary>
		/// Gets the typed values for parameters in this projection
		/// </summary>
		/// <param name = "criteria">The criteria.</param>
		/// <param name = "criteriaQuery">The criteria query.</param>
		/// <returns></returns>
		public async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return new TypedValue[0];
		}
	}
}