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
	public partial class BetweenExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			//TODO: add a default capacity
			SqlStringBuilder sqlBuilder = new SqlStringBuilder();
			var parametersTypes = await (GetTypedValuesAsync(criteria, criteriaQuery)).ToArray();
			var lowType = parametersTypes[0];
			var highType = parametersTypes[1];
			SqlString[] columnNames = await (CriterionUtil.GetColumnNamesAsync(_propertyName, _projection, criteriaQuery, criteria, enabledFilters));
			if (columnNames.Length == 1)
			{
				sqlBuilder.Add(columnNames[0]).Add(" between ").Add(criteriaQuery.NewQueryParameter(lowType).Single()).Add(" and ").Add(criteriaQuery.NewQueryParameter(highType).Single());
			}
			else
			{
				bool andNeeded = false;
				var lowParameters = criteriaQuery.NewQueryParameter(lowType).ToArray();
				for (int i = 0; i < columnNames.Length; i++)
				{
					if (andNeeded)
					{
						sqlBuilder.Add(" AND ");
					}

					andNeeded = true;
					sqlBuilder.Add(columnNames[i]).Add(" >= ").Add(lowParameters[i]);
				}

				var highParameters = criteriaQuery.NewQueryParameter(highType).ToArray();
				for (int i = 0; i < columnNames.Length; i++)
				{
					sqlBuilder.Add(" AND ").Add(columnNames[i]).Add(" <= ").Add(highParameters[i]);
				}
			}

			return sqlBuilder.ToSqlString();
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return CriterionUtil.GetTypedValues(criteriaQuery, criteria, _projection, _propertyName, _lo, _hi);
		}
	}
}