using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GroupedProjection : IProjection
	{
		public virtual async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			return renderedProjection = await (projection.ToSqlStringAsync(criteria, position, criteriaQuery, enabledFilters));
		}

		/// <summary>
		/// Gets the typed values for parameters in this projection
		/// </summary>
		/// <param name = "criteria">The criteria.</param>
		/// <param name = "criteriaQuery">The criteria query.</param>
		/// <returns></returns>
		public Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return projection.GetTypedValuesAsync(criteria, criteriaQuery);
		}
	}
}