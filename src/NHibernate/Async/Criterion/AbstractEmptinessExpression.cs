using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEmptinessExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			var entityName = criteriaQuery.GetEntityName(criteria, propertyName);
			var actualPropertyName = criteriaQuery.GetPropertyName(propertyName);
			var sqlAlias = criteriaQuery.GetSQLAlias(criteria, propertyName);
			var factory = criteriaQuery.Factory;
			var collectionPersister = GetQueryableCollection(entityName, actualPropertyName, factory);
			var collectionKeys = collectionPersister.KeyColumnNames;
			var ownerKeys = ((ILoadable)factory.GetEntityPersister(entityName)).IdentifierColumnNames;
			var innerSelect = new StringBuilder();
			innerSelect.Append("(select 1 from ").Append(collectionPersister.TableName).Append(" where ").Append(new ConditionalFragment().SetTableAlias(sqlAlias).SetCondition(ownerKeys, collectionKeys).ToSqlStringFragment());
			if (collectionPersister.HasWhere)
			{
				innerSelect.Append(" and (").Append(collectionPersister.GetSQLWhereString(collectionPersister.TableName)).Append(") ");
			}

			innerSelect.Append(")");
			return new SqlString(new object[]{ExcludeEmpty ? "exists" : "not exists", innerSelect.ToString()});
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return NO_VALUES;
		}
	}
}