using System;
using System.Collections.Generic;
using NHibernate.SqlCommand;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class SimpleProjection : IProjection
	{
		public abstract Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters);
		/// <summary>
		/// Gets the typed values for parameters in this projection
		/// </summary>
		/// <param name = "criteria">The criteria.</param>
		/// <param name = "criteriaQuery">The criteria query.</param>
		/// <returns></returns>
		public virtual async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return new TypedValue[0];
		}
	}
}