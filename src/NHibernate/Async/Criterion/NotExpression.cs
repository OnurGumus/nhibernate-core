using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NotExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			//TODO: set default capacity
			var builder = new SqlStringBuilder();
			builder.Add("not (");
			builder.Add(await (_criterion.ToSqlStringAsync(criteria, criteriaQuery, enabledFilters)));
			builder.Add(")");
			return builder.ToSqlString();
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return await (_criterion.GetTypedValuesAsync(criteria, criteriaQuery));
		}
	}
}