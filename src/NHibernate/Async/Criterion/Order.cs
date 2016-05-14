#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Order
	{
		/// <summary>
		/// Render the SQL fragment
		/// </summary>
		public virtual async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			if (projection != null)
			{
				SqlString sb = SqlString.Empty;
				SqlString produced = await (this.projection.ToSqlStringAsync(criteria, 0, criteriaQuery, new Dictionary<string, IFilter>()));
				SqlString truncated = SqlStringHelper.RemoveAsAliasesFromSql(produced);
				sb = sb.Append(truncated);
				sb = sb.Append(ascending ? " asc" : " desc");
				return sb;
			}

			string[] columns = criteriaQuery.GetColumnAliasesUsingProjection(criteria, propertyName);
			Type.IType type = criteriaQuery.GetTypeUsingProjection(criteria, propertyName);
			StringBuilder fragment = new StringBuilder();
			ISessionFactoryImplementor factory = criteriaQuery.Factory;
			for (int i = 0; i < columns.Length; i++)
			{
				bool lower = ignoreCase && IsStringType(type.SqlTypes(factory)[i]);
				if (lower)
				{
					fragment.Append(factory.Dialect.LowercaseFunction).Append("(");
				}

				fragment.Append(columns[i]);
				if (lower)
				{
					fragment.Append(")");
				}

				fragment.Append(ascending ? " asc" : " desc");
				if (i < columns.Length - 1)
				{
					fragment.Append(", ");
				}
			}

			return new SqlString(fragment.ToString());
		}

		public async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			if (projection != null)
				return await (projection.GetTypedValuesAsync(criteria, criteriaQuery));
			return new TypedValue[0]; // not using parameters for ORDER BY columns
		}
	}
}
#endif
