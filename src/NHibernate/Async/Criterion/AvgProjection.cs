using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AvgProjection : AggregateProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			ISessionFactoryImplementor factory = criteriaQuery.Factory;
			SqlType[] sqlTypeCodes = NHibernateUtil.Double.SqlTypes(factory);
			string sqlType = factory.Dialect.GetCastTypeName(sqlTypeCodes[0]);
			var sql = new SqlStringBuilder().Add(aggregate).Add("(");
			sql.Add("cast(");
			if (projection != null)
			{
				sql.Add(SqlStringHelper.RemoveAsAliasesFromSql(await (projection.ToSqlStringAsync(criteria, loc, criteriaQuery, enabledFilters))));
			}
			else
			{
				sql.Add(criteriaQuery.GetColumn(criteria, propertyName));
			}

			sql.Add(" as ").Add(sqlType).Add(")");
			sql.Add(") as ").Add(GetColumnAliases(loc, criteria, criteriaQuery)[0]);
			return sql.ToSqlString();
		}
	}
}