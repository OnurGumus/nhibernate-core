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
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int loc, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlStringBuilder s = new SqlStringBuilder();
			string[] cols = criteriaQuery.GetColumnsUsingProjection(criteria, propertyName);
			for (int i = 0; i < cols.Length; i++)
			{
				s.Add(cols[i]);
				s.Add(" as y");
				s.Add((loc + i).ToString());
				s.Add("_");
				if (i < cols.Length - 1)
					s.Add(", ");
			}

			return s.ToSqlString();
		}
	}
}