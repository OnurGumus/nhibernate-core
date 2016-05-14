#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InsensitiveLikeExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			//TODO: add default capacity
			SqlStringBuilder sqlBuilder = new SqlStringBuilder();
			SqlString[] columnNames = await (CriterionUtil.GetColumnNamesAsync(propertyName, projection, criteriaQuery, criteria, enabledFilters));
			if (columnNames.Length != 1)
			{
				throw new HibernateException("insensitive like may only be used with single-column properties");
			}

			if (criteriaQuery.Factory.Dialect is PostgreSQLDialect)
			{
				sqlBuilder.Add(columnNames[0]);
				sqlBuilder.Add(" ilike ");
			}
			else
			{
				sqlBuilder.Add(criteriaQuery.Factory.Dialect.LowercaseFunction).Add("(").Add(columnNames[0]).Add(")").Add(" like ");
			}

			sqlBuilder.Add(criteriaQuery.NewQueryParameter(GetParameterTypedValue(criteria, criteriaQuery)).Single());
			return sqlBuilder.ToSqlString();
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			List<TypedValue> typedValues = new List<TypedValue>();
			if (projection != null)
			{
				typedValues.AddRange(await (projection.GetTypedValuesAsync(criteria, criteriaQuery)));
			}

			typedValues.Add(GetParameterTypedValue(criteria, criteriaQuery));
			return typedValues.ToArray();
		}
	}
}
#endif
