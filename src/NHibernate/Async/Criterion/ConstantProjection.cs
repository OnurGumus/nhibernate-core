using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ConstantProjection : SimpleProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			return new SqlString(criteriaQuery.NewQueryParameter(typedValue).Single(), " as ", GetColumnAliases(position, criteria, criteriaQuery)[0]);
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return new TypedValue[]{typedValue};
		}
	}
}