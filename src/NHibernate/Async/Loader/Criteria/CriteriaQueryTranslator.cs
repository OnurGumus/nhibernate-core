using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Hql.Util;
using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate_Persister_Entity = NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Loader.Criteria
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CriteriaQueryTranslator : ICriteriaQuery
	{
		public Task<SqlString> GetSelectAsync(IDictionary<string, IFilter> enabledFilters)
		{
			return rootCriteria.Projection.ToSqlStringAsync(rootCriteria.ProjectionCriteria, 0, this, enabledFilters);
		}

		public async Task<SqlString> GetWhereConditionAsync(IDictionary<string, IFilter> enabledFilters)
		{
			SqlStringBuilder condition = new SqlStringBuilder(30);
			bool first = true;
			foreach (CriteriaImpl.CriterionEntry entry in rootCriteria.IterateExpressionEntries())
			{
				if (!HasGroupedOrAggregateProjection(entry.Criterion.GetProjections()))
				{
					if (!first)
					{
						condition.Add(" and ");
					}

					first = false;
					SqlString sqlString = await (entry.Criterion.ToSqlStringAsync(entry.Criteria, this, enabledFilters));
					condition.Add(sqlString);
				}
			}

			return condition.ToSqlString();
		}

		public async Task<SqlString> GetOrderByAsync()
		{
			SqlStringBuilder orderBy = new SqlStringBuilder(30);
			bool first = true;
			foreach (CriteriaImpl.OrderEntry oe in rootCriteria.IterateOrderings())
			{
				if (!first)
				{
					orderBy.Add(StringHelper.CommaSpace);
				}

				first = false;
				orderBy.Add(await (oe.Order.ToSqlStringAsync(oe.Criteria, this)));
			}

			return orderBy.ToSqlString();
		}

		public async Task<SqlString> GetWithClauseAsync(string path, IDictionary<string, IFilter> enabledFilters)
		{
			ICriterion crit;
			if (withClauseMap.TryGetValue(path, out crit))
			{
				return crit == null ? null : await (crit.ToSqlStringAsync(GetCriteria(path), this, enabledFilters));
			}

			return null;
		}

		public async Task<SqlString> GetHavingConditionAsync(IDictionary<string, IFilter> enabledFilters)
		{
			SqlStringBuilder condition = new SqlStringBuilder(30);
			bool first = true;
			foreach (CriteriaImpl.CriterionEntry entry in rootCriteria.IterateExpressionEntries())
			{
				if (HasGroupedOrAggregateProjection(entry.Criterion.GetProjections()))
				{
					if (!first)
					{
						condition.Add(" and ");
					}

					first = false;
					SqlString sqlString = await (entry.Criterion.ToSqlStringAsync(entry.Criteria, this, enabledFilters));
					condition.Add(sqlString);
				}
			}

			return condition.ToSqlString();
		}
	}
}