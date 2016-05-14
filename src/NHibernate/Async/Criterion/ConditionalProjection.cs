#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	using System;
	using System.Collections.Generic;
	using Engine;
	using SqlCommand;
	using Type;
	using Util;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ConditionalProjection : SimpleProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlString condition = await (criterion.ToSqlStringAsync(criteria, criteriaQuery, enabledFilters));
			SqlString ifTrue = await (whenTrue.ToSqlStringAsync(criteria, position + GetHashCode() + 1, criteriaQuery, enabledFilters));
			ifTrue = SqlStringHelper.RemoveAsAliasesFromSql(ifTrue);
			SqlString ifFalse = await (whenFalse.ToSqlStringAsync(criteria, position + GetHashCode() + 2, criteriaQuery, enabledFilters));
			ifFalse = SqlStringHelper.RemoveAsAliasesFromSql(ifFalse);
			return new SqlString("(case when ", condition, " then ", ifTrue, " else ", ifFalse, " end) as ", GetColumnAliases(position, criteria, criteriaQuery)[0]);
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			List<TypedValue> tv = new List<TypedValue>();
			tv.AddRange(await (criterion.GetTypedValuesAsync(criteria, criteriaQuery)));
			tv.AddRange(await (whenTrue.GetTypedValuesAsync(criteria, criteriaQuery)));
			tv.AddRange(await (whenFalse.GetTypedValuesAsync(criteria, criteriaQuery)));
			return tv.ToArray();
		}
	}
}
#endif
