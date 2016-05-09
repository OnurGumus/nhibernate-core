using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NullExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			//TODO: add default capacity
			SqlStringBuilder sqlBuilder = new SqlStringBuilder();
			SqlString[] columnNames = await (CriterionUtil.GetColumnNamesAsync(_propertyName, _projection, criteriaQuery, criteria, enabledFilters));
			for (int i = 0; i < columnNames.Length; i++)
			{
				if (i > 0)
				{
					sqlBuilder.Add(" and ");
				}

				sqlBuilder.Add(columnNames[i]).Add(" is null");
			}

			if (columnNames.Length > 1)
			{
				sqlBuilder.Insert(0, "(");
				sqlBuilder.Add(")");
			}

			return sqlBuilder.ToSqlString();
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return _projection == null ? NoValues : await (_projection.GetTypedValuesAsync(criteria, criteriaQuery));
		}
	}
}