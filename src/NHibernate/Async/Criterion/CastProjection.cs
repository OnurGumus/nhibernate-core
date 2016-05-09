using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	using System;
	using System.Collections.Generic;
	using Engine;
	using SqlCommand;
	using SqlTypes;
	using Type;
	using Util;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CastProjection : SimpleProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			ISessionFactoryImplementor factory = criteriaQuery.Factory;
			SqlType[] sqlTypeCodes = type.SqlTypes(factory);
			if (sqlTypeCodes.Length != 1)
			{
				throw new QueryException("invalid Hibernate type for CastProjection");
			}

			string sqlType = factory.Dialect.GetCastTypeName(sqlTypeCodes[0]);
			int loc = position * GetHashCode();
			SqlString val = await (projection.ToSqlStringAsync(criteria, loc, criteriaQuery, enabledFilters));
			val = SqlStringHelper.RemoveAsAliasesFromSql(val);
			return new SqlString("cast( ", val, " as ", sqlType, ") as ", GetColumnAliases(position, criteria, criteriaQuery)[0]);
		}

		public override async Task<NHibernate.Engine.TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return await (projection.GetTypedValuesAsync(criteria, criteriaQuery));
		}
	}
}