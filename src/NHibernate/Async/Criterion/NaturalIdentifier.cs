using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion.Lambda;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NaturalIdentifier : ICriterion
	{
		public async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			return await (conjunction.ToSqlStringAsync(criteria, criteriaQuery, enabledFilters));
		}

		public async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return await (conjunction.GetTypedValuesAsync(criteria, criteriaQuery));
		}
	}
}