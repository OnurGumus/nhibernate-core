using System;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	using System.Collections.Generic;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IdentifierProjection : SimpleProjection
	{
		public override Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			try
			{
				return Task.FromResult<SqlString>(ToSqlString(criteria, position, criteriaQuery, enabledFilters));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<SqlString>(ex);
			}
		}
	}
}