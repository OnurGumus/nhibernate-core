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
	public partial class IdentifierEqExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			//Implementation changed from H3.2 to use SqlString
			string[] columns = criteriaQuery.GetIdentifierColumns(criteria);
			Parameter[] parameters = await (GetTypedValuesAsync(criteria, criteriaQuery)).SelectMany(t => criteriaQuery.NewQueryParameter(t)).ToArray();
			SqlStringBuilder result = new SqlStringBuilder(4 * columns.Length + 2);
			if (columns.Length > 1)
			{
				result.Add(StringHelper.OpenParen);
			}

			for (int i = 0; i < columns.Length; i++)
			{
				if (i > 0)
				{
					result.Add(" and ");
				}

				result.Add(columns[i]).Add(" = ");
				await (AddValueOrProjectionAsync(parameters, i, criteria, criteriaQuery, enabledFilters, result));
			}

			if (columns.Length > 1)
			{
				result.Add(StringHelper.ClosedParen);
			}

			return result.ToSqlString();
		}

		private async Task AddValueOrProjectionAsync(Parameter[] parameters, int paramIndex, ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters, SqlStringBuilder result)
		{
			if (_projection == null)
			{
				result.Add(parameters[paramIndex]);
			}
			else
			{
				SqlString sql = await (_projection.ToSqlStringAsync(criteria, GetHashCode(), criteriaQuery, enabledFilters));
				result.Add(SqlStringHelper.RemoveAsAliasesFromSql(sql));
			}
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			if (_projection != null)
				return await (_projection.GetTypedValuesAsync(criteria, criteriaQuery));
			return new TypedValue[]{criteriaQuery.GetTypedIdentifierValue(criteria, value)};
		}
	}
}