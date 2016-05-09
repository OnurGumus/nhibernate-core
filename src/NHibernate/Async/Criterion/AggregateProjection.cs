using System;
using System.Collections.Generic;
using NHibernate.SqlCommand;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AggregateProjection : SimpleProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			if (projection == null)
			{
				return new SqlString(aggregate, "(", criteriaQuery.GetColumn(criteria, propertyName), ") as y", loc.ToString(), "_");
			}

			return new SqlString(aggregate, "(", SqlStringHelper.RemoveAsAliasesFromSql(await (projection.ToSqlStringAsync(criteria, loc, criteriaQuery, enabledFilters))), ") as y", loc.ToString(), "_");
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			if (projection != null)
				return await (projection.GetTypedValuesAsync(criteria, criteriaQuery));
			return await (base.GetTypedValuesAsync(criteria, criteriaQuery));
		}
	}
}