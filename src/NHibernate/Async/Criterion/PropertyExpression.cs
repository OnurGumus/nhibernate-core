#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	using System;
	using System.Collections.Generic;
	using Engine;
	using SqlCommand;
	using Util;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class PropertyExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlString[] columnNames = await (CriterionUtil.GetColumnNamesAsync(_lhsPropertyName, _lhsProjection, criteriaQuery, criteria, enabledFilters));
			SqlString[] otherColumnNames = await (CriterionUtil.GetColumnNamesAsync(_rhsPropertyName, _rhsProjection, criteriaQuery, criteria, enabledFilters));
			SqlStringBuilder sb = new SqlStringBuilder();
			if (columnNames.Length > 1)
			{
				sb.Add(StringHelper.OpenParen);
			}

			bool first = true;
			foreach (SqlString sqlString in SqlStringHelper.Add(columnNames, Op, otherColumnNames))
			{
				if (first == false)
				{
					sb.Add(" and ");
				}

				first = false;
				sb.Add(sqlString);
			}

			if (columnNames.Length > 1)
			{
				sb.Add(StringHelper.ClosedParen);
			}

			return sb.ToSqlString();
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			if (_lhsProjection == null && _rhsProjection == null)
			{
				return NoTypedValues;
			}
			else
			{
				List<TypedValue> types = new List<TypedValue>();
				if (_lhsProjection != null)
				{
					types.AddRange(await (_lhsProjection.GetTypedValuesAsync(criteria, criteriaQuery)));
				}

				if (_rhsProjection != null)
				{
					types.AddRange(await (_rhsProjection.GetTypedValuesAsync(criteria, criteriaQuery)));
				}

				return types.ToArray();
			}
		}
	}
}
#endif
