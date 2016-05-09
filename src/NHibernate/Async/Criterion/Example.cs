using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Example : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			SqlStringBuilder builder = new SqlStringBuilder();
			builder.Add(StringHelper.OpenParen);
			IEntityPersister meta = criteriaQuery.Factory.GetEntityPersister(criteriaQuery.GetEntityName(criteria));
			String[] propertyNames = meta.PropertyNames;
			IType[] propertyTypes = meta.PropertyTypes;
			object[] propertyValues = GetPropertyValues(meta, criteria, criteriaQuery);
			for (int i = 0; i < propertyNames.Length; i++)
			{
				object propertyValue = propertyValues[i];
				String propertyName = propertyNames[i];
				bool isPropertyIncluded = i != meta.VersionProperty && IsPropertyIncluded(propertyValue, propertyName, propertyTypes[i]);
				if (isPropertyIncluded)
				{
					if (propertyTypes[i].IsComponentType)
					{
						await (AppendComponentConditionAsync(propertyName, propertyValue, (IAbstractComponentType)propertyTypes[i], criteria, criteriaQuery, enabledFilters, builder));
					}
					else
					{
						await (AppendPropertyConditionAsync(propertyName, propertyValue, criteria, criteriaQuery, enabledFilters, builder));
					}
				}
			}

			if (builder.Count == 1)
			{
				builder.Add("1=1"); // yuck!
			}

			builder.Add(StringHelper.ClosedParen);
			return builder.ToSqlString();
		}

		protected async Task AppendPropertyConditionAsync(String propertyName, object propertyValue, ICriteria criteria, ICriteriaQuery cq, IDictionary<string, IFilter> enabledFilters, SqlStringBuilder builder)
		{
			if (builder.Count > 1)
			{
				builder.Add(" and ");
			}

			ICriterion crit = propertyValue != null ? GetNotNullPropertyCriterion(propertyValue, propertyName) : new NullExpression(propertyName);
			builder.Add(await (crit.ToSqlStringAsync(criteria, cq, enabledFilters)));
		}

		protected async Task AppendComponentConditionAsync(String path, object component, IAbstractComponentType type, ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters, SqlStringBuilder builder)
		{
			if (component != null)
			{
				String[] propertyNames = type.PropertyNames;
				object[] values = await (type.GetPropertyValuesAsync(component, GetEntityMode(criteria, criteriaQuery)));
				IType[] subtypes = type.Subtypes;
				for (int i = 0; i < propertyNames.Length; i++)
				{
					String subpath = StringHelper.Qualify(path, propertyNames[i]);
					object value = values[i];
					if (IsPropertyIncluded(value, subpath, subtypes[i]))
					{
						IType subtype = subtypes[i];
						if (subtype.IsComponentType)
						{
							await (AppendComponentConditionAsync(subpath, value, (IAbstractComponentType)subtype, criteria, criteriaQuery, enabledFilters, builder));
						}
						else
						{
							await (AppendPropertyConditionAsync(subpath, value, criteria, criteriaQuery, enabledFilters, builder));
						}
					}
				}
			}
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			IEntityPersister meta = criteriaQuery.Factory.GetEntityPersister(criteriaQuery.GetEntityName(criteria));
			string[] propertyNames = meta.PropertyNames;
			IType[] propertyTypes = meta.PropertyTypes;
			object[] values = GetPropertyValues(meta, criteria, criteriaQuery);
			var list = new List<TypedValue>();
			for (int i = 0; i < propertyNames.Length; i++)
			{
				object value = values[i];
				IType type = propertyTypes[i];
				string name = propertyNames[i];
				bool isPropertyIncluded = (i != meta.VersionProperty && IsPropertyIncluded(value, name, type));
				if (isPropertyIncluded)
				{
					if (propertyTypes[i].IsComponentType)
					{
						await (AddComponentTypedValuesAsync(name, value, (IAbstractComponentType)type, list, criteria, criteriaQuery));
					}
					else
					{
						AddPropertyTypedValue(value, type, list);
					}
				}
			}

			return list.ToArray();
		}

		protected async Task AddComponentTypedValuesAsync(string path, object component, IAbstractComponentType type, IList list, ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			if (component != null)
			{
				string[] propertyNames = type.PropertyNames;
				IType[] subtypes = type.Subtypes;
				object[] values = await (type.GetPropertyValuesAsync(component, GetEntityMode(criteria, criteriaQuery)));
				for (int i = 0; i < propertyNames.Length; i++)
				{
					object value = values[i];
					IType subtype = subtypes[i];
					string subpath = StringHelper.Qualify(path, propertyNames[i]);
					if (IsPropertyIncluded(value, subpath, subtype))
					{
						if (subtype.IsComponentType)
						{
							await (AddComponentTypedValuesAsync(subpath, value, (IAbstractComponentType)subtype, list, criteria, criteriaQuery));
						}
						else
						{
							AddPropertyTypedValue(value, subtype, list);
						}
					}
				}
			}
		}
	}
}