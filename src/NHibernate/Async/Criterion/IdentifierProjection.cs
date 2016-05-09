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
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlStringBuilder buf = new SqlStringBuilder();
			string[] cols = criteriaQuery.GetIdentifierColumns(criteria);
			for (int i = 0; i < cols.Length; i++)
			{
				if (i > 0)
				{
					buf.Add(", ");
				}

				buf.Add(cols[i]).Add(" as y").Add((position + i).ToString()).Add("_");
			}

			return buf.ToSqlString();
		}
	}
}