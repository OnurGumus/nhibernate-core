using System;
using System.Collections.Generic;
using NHibernate.SqlCommand;
using NHibernate.Engine;
using NHibernate.Loader.Criteria;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IProjection
	{
		/// <summary>
		/// Render the SQL Fragment.
		/// </summary>
		/// <param name = "criteria">The criteria.</param>
		/// <param name = "position">The position.</param>
		/// <param name = "criteriaQuery">The criteria query.</param>
		/// <param name = "enabledFilters">The enabled filters.</param>
		/// <returns></returns>
		Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters);
		/// <summary>
		/// Gets the typed values for parameters in this projection
		/// </summary>
		/// <param name = "criteria">The criteria.</param>
		/// <param name = "criteriaQuery">The criteria query.</param>
		/// <returns></returns>
		Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery);
	}
}