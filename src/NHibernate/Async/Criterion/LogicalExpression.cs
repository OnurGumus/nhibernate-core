using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class LogicalExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			//TODO: add default capacity
			SqlStringBuilder sqlBuilder = new SqlStringBuilder();
			SqlString lhSqlString = await (_lhs.ToSqlStringAsync(criteria, criteriaQuery, enabledFilters));
			SqlString rhSqlString = await (_rhs.ToSqlStringAsync(criteria, criteriaQuery, enabledFilters));
			sqlBuilder.Add(new SqlString[]{lhSqlString, rhSqlString}, "(", Op, ")", false // not wrapping because the prefix and postfix params already take care of that	
			);
			return sqlBuilder.ToSqlString();
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			TypedValue[] lhstv = await (_lhs.GetTypedValuesAsync(criteria, criteriaQuery));
			TypedValue[] rhstv = await (_rhs.GetTypedValuesAsync(criteria, criteriaQuery));
			TypedValue[] result = new TypedValue[lhstv.Length + rhstv.Length];
			Array.Copy(lhstv, 0, result, 0, lhstv.Length);
			Array.Copy(rhstv, 0, result, lhstv.Length, rhstv.Length);
			return result;
		}
	}
}