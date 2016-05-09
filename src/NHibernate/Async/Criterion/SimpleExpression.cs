using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlString[] columnNames = await (CriterionUtil.GetColumnNamesForSimpleExpressionAsync(propertyName, _projection, criteriaQuery, criteria, enabledFilters, this, value));
			TypedValue typedValue = GetParameterTypedValue(criteria, criteriaQuery);
			Parameter[] parameters = criteriaQuery.NewQueryParameter(typedValue).ToArray();
			if (ignoreCase)
			{
				if (columnNames.Length != 1)
				{
					throw new HibernateException("case insensitive expression may only be applied to single-column properties: " + propertyName);
				}

				return new SqlString(criteriaQuery.Factory.Dialect.LowercaseFunction, StringHelper.OpenParen, columnNames[0], StringHelper.ClosedParen, Op, parameters.Single());
			}
			else
			{
				SqlStringBuilder sqlBuilder = new SqlStringBuilder(4 * columnNames.Length);
				var columnNullness = await (typedValue.Type.ToColumnNullnessAsync(typedValue.Value, criteriaQuery.Factory));
				if (columnNullness.Length != columnNames.Length)
				{
					throw new AssertionFailure("Column nullness length doesn't match number of columns.");
				}

				for (int i = 0; i < columnNames.Length; i++)
				{
					if (i > 0)
					{
						sqlBuilder.Add(" and ");
					}

					if (columnNullness[i])
					{
						sqlBuilder.Add(columnNames[i]).Add(Op).Add(parameters[i]);
					}
					else
					{
						sqlBuilder.Add(columnNames[i]).Add(" is null ");
					}
				}

				return sqlBuilder.ToSqlString();
			}
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			var typedValues = new List<TypedValue>();
			if (_projection != null)
			{
				typedValues.AddRange(await (_projection.GetTypedValuesAsync(criteria, criteriaQuery)));
			}

			typedValues.Add(GetParameterTypedValue(criteria, criteriaQuery));
			return typedValues.ToArray();
		}
	}
}