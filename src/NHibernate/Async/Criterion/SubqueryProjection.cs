using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SubqueryProjection : SimpleProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			return await (_subQuery.ToSqlStringAsync(criteria, criteriaQuery, enabledFilters)).Append(new SqlString(" as y", loc.ToString(), "_"));
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return await (_subQuery.GetTypedValuesAsync(criteria, criteriaQuery));
		}
	}
}