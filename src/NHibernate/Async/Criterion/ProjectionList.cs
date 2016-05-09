using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProjectionList : IProjection
	{
		public async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlStringBuilder buf = new SqlStringBuilder();
			for (int i = 0; i < Length; i++)
			{
				IProjection proj = this[i];
				buf.Add(await (proj.ToSqlStringAsync(criteria, loc, criteriaQuery, enabledFilters)));
				loc += proj.GetColumnAliases(loc, criteria, criteriaQuery).Length;
				if (i < elements.Count - 1)
				{
					buf.Add(", ");
				}
			}

			return buf.ToSqlString();
		}

		public async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			List<TypedValue> values = new List<TypedValue>();
			foreach (IProjection element in elements)
			{
				values.AddRange(await (element.GetTypedValuesAsync(criteria, criteriaQuery)));
			}

			return values.ToArray();
		}
	}
}