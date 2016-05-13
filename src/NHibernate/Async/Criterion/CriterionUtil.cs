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
	public static partial class CriterionUtil
	{
		public static async Task<SqlString[]> GetColumnNamesAsync(string propertyName, IProjection projection, ICriteriaQuery criteriaQuery, ICriteria criteria, IDictionary<string, IFilter> enabledFilters)
		{
			if (projection == null)
				return GetColumnNamesUsingPropertyName(criteriaQuery, criteria, propertyName);
			else
				return await (GetColumnNamesUsingProjectionAsync(projection, criteriaQuery, criteria, enabledFilters));
		}

		public static async Task<SqlString[]> GetColumnNamesForSimpleExpressionAsync(string propertyName, IProjection projection, ICriteriaQuery criteriaQuery, ICriteria criteria, IDictionary<string, IFilter> enabledFilters, ICriterion criterion, object value)
		{
			if (projection == null)
			{
				return GetColumnNamesUsingPropertyName(criteriaQuery, criteria, propertyName, value, criterion);
			}
			else
			{
				return await (GetColumnNamesUsingProjectionAsync(projection, criteriaQuery, criteria, enabledFilters));
			}
		}

		internal static async Task<SqlString[]> GetColumnNamesUsingProjectionAsync(IProjection projection, ICriteriaQuery criteriaQuery, ICriteria criteria, IDictionary<string, IFilter> enabledFilters)
		{
			SqlString sqlString = await (projection.ToSqlStringAsync(criteria, criteriaQuery.GetIndexForAlias(), criteriaQuery, enabledFilters));
			return new SqlString[]{SqlStringHelper.RemoveAsAliasesFromSql(sqlString)};
		}
	}
}