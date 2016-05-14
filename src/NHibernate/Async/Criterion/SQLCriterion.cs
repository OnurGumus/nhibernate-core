#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SQLCriterion : AbstractCriterion
	{
		public override Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			try
			{
				return Task.FromResult<SqlString>(ToSqlString(criteria, criteriaQuery, enabledFilters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<SqlString>(ex);
			}
		}

		public override Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			try
			{
				return Task.FromResult<TypedValue[]>(GetTypedValues(criteria, criteriaQuery));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<TypedValue[]>(ex);
			}
		}
	}
}
#endif
