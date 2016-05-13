using System;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	using System.Collections.Generic;
	using Engine;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class SQLProjection : IProjection
	{
		public Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
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

		/// <summary>
		/// Gets the typed values for parameters in this projection
		/// </summary>
		/// <param name = "criteria">The criteria.</param>
		/// <param name = "criteriaQuery">The criteria query.</param>
		/// <returns></returns>
		public Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
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