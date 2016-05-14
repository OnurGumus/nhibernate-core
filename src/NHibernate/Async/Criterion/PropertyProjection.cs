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
	public partial class PropertyProjection : SimpleProjection, IPropertyProjection
	{
		public override Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			try
			{
				return Task.FromResult<SqlString>(ToSqlString(criteria, loc, criteriaQuery, enabledFilters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<SqlString>(ex);
			}
		}
	}
}
#endif
