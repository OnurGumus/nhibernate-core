using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlFunctionProjection : SimpleProjection
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			ISQLFunction sqlFunction = GetFunction(criteriaQuery);
			var arguments = new List<object>();
			for (int i = 0; i < args.Length; i++)
			{
				SqlString projectArg = await (GetProjectionArgumentAsync(criteriaQuery, criteria, args[i], 0, enabledFilters)); // The loc parameter is unused.
				arguments.Add(projectArg);
			}

			return new SqlString(sqlFunction.Render(arguments, criteriaQuery.Factory), " as ", GetColumnAliases(position, criteria, criteriaQuery)[0]);
		}

		private static async Task<SqlString> GetProjectionArgumentAsync(ICriteriaQuery criteriaQuery, ICriteria criteria, IProjection projection, int loc, IDictionary<string, IFilter> enabledFilters)
		{
			SqlString sql = await (projection.ToSqlStringAsync(criteria, loc, criteriaQuery, enabledFilters));
			return SqlStringHelper.RemoveAsAliasesFromSql(sql);
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			List<TypedValue> types = new List<TypedValue>();
			foreach (IProjection projection in args)
			{
				TypedValue[] argTypes = await (projection.GetTypedValuesAsync(criteria, criteriaQuery));
				types.AddRange(argTypes);
			}

			return types.ToArray();
		}
	}
}