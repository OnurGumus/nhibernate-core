#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LikeExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlString[] columns = await (CriterionUtil.GetColumnNamesUsingProjectionAsync(projection, criteriaQuery, criteria, enabledFilters));
			if (columns.Length != 1)
				throw new HibernateException("Like may only be used with single-column properties / projections.");
			SqlStringBuilder lhs = new SqlStringBuilder(6);
			if (ignoreCase)
			{
				Dialect.Dialect dialect = criteriaQuery.Factory.Dialect;
				lhs.Add(dialect.LowercaseFunction).Add(StringHelper.OpenParen).Add(columns[0]).Add(StringHelper.ClosedParen);
			}
			else
				lhs.Add(columns[0]);
			if (ignoreCase)
			{
				Dialect.Dialect dialect = criteriaQuery.Factory.Dialect;
				lhs.Add(" like ").Add(dialect.LowercaseFunction).Add(StringHelper.OpenParen).Add(criteriaQuery.NewQueryParameter(typedValue).Single()).Add(StringHelper.ClosedParen);
			}
			else
				lhs.Add(" like ").Add(criteriaQuery.NewQueryParameter(typedValue).Single());
			if (escapeChar.HasValue)
				lhs.Add(" escape '" + escapeChar + "'");
			return lhs.ToSqlString();
		}

		public override Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
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
#endif
