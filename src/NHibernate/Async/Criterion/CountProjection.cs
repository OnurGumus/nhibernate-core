#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CountProjection : AggregateProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			var buf = new SqlStringBuilder().Add("count(");
			if (distinct)
			{
				buf.Add("distinct ");
			}

			if (projection != null)
			{
				buf.Add(SqlStringHelper.RemoveAsAliasesFromSql(await (projection.ToSqlStringAsync(criteria, position, criteriaQuery, enabledFilters))));
			}
			else
			{
				buf.Add(criteriaQuery.GetColumn(criteria, propertyName));
			}

			buf.Add(") as y").Add(position.ToString()).Add("_");
			return buf.ToSqlString();
		}
	}
}
#endif
