using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class Junction : AbstractCriterion
	{
		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			var typedValues = new List<TypedValue>();
			foreach (ICriterion criterion in this.criteria)
			{
				TypedValue[] subvalues = await (criterion.GetTypedValuesAsync(criteria, criteriaQuery));
				typedValues.AddRange(subvalues);
			}

			return typedValues.ToArray();
		}

		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			if (this.criteria.Count == 0)
			{
				return EmptyExpression;
			}

			//TODO: add default capacity
			SqlStringBuilder sqlBuilder = new SqlStringBuilder();
			sqlBuilder.Add("(");
			for (int i = 0; i < this.criteria.Count - 1; i++)
			{
				sqlBuilder.Add(await (this.criteria[i].ToSqlStringAsync(criteria, criteriaQuery, enabledFilters)));
				sqlBuilder.Add(Op);
			}

			sqlBuilder.Add(await (this.criteria[this.criteria.Count - 1].ToSqlStringAsync(criteria, criteriaQuery, enabledFilters)));
			sqlBuilder.Add(")");
			return sqlBuilder.ToSqlString();
		}
	}
}