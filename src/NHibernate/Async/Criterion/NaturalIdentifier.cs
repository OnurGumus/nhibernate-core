using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion.Lambda;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NaturalIdentifier : ICriterion
	{
		public Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			return conjunction.ToSqlStringAsync(criteria, criteriaQuery, enabledFilters);
		}

		public Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return conjunction.GetTypedValuesAsync(criteria, criteriaQuery);
		}
	}
}