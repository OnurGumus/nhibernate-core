using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Distinct : IProjection
	{
		public virtual async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			return new SqlString("distinct ").Append(await (projection.ToSqlStringAsync(criteria, position, criteriaQuery, enabledFilters)));
		}

		public async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return await (projection.GetTypedValuesAsync(criteria, criteriaQuery));
		}
	}
}