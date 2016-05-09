using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InExpression : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			if (_projection == null)
			{
				AssertPropertyIsNotCollection(criteriaQuery, criteria);
			}

			if (_values.Length == 0)
			{
				// "something in ()" is always false
				return new SqlString("1=0");
			}

			//TODO: add default capacity
			SqlStringBuilder result = new SqlStringBuilder();
			SqlString[] columnNames = await (CriterionUtil.GetColumnNamesAsync(_propertyName, _projection, criteriaQuery, criteria, enabledFilters));
			// Generate SqlString of the form:
			// columnName1 in (values) and columnName2 in (values) and ...
			Parameter[] parameters = await (GetParameterTypedValuesAsync(criteria, criteriaQuery)).SelectMany(t => criteriaQuery.NewQueryParameter(t)).ToArray();
			for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
			{
				SqlString columnName = columnNames[columnIndex];
				if (columnIndex > 0)
				{
					result.Add(" and ");
				}

				result.Add(columnName).Add(" in (");
				for (int i = 0; i < _values.Length; i++)
				{
					if (i > 0)
					{
						result.Add(StringHelper.CommaSpace);
					}

					result.Add(parameters[i]);
				}

				result.Add(")");
			}

			return result.ToSqlString();
		}

		private async Task<List<TypedValue>> GetParameterTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			IType type = GetElementType(criteria, criteriaQuery);
			if (type.IsComponentType)
			{
				List<TypedValue> list = new List<TypedValue>();
				IAbstractComponentType actype = (IAbstractComponentType)type;
				IType[] types = actype.Subtypes;
				for (int i = 0; i < types.Length; i++)
				{
					for (int j = 0; j < _values.Length; j++)
					{
						object subval = _values[j] == null ? null : await (actype.GetPropertyValuesAsync(_values[j], EntityMode.Poco))[i];
						list.Add(new TypedValue(types[i], subval, EntityMode.Poco));
					}
				}

				return list;
			}
			else
			{
				return _values.Select(v => new TypedValue(type, v, EntityMode.Poco)).ToList();
			}
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			var list = await (GetParameterTypedValuesAsync(criteria, criteriaQuery));
			if (_projection != null)
				list.InsertRange(0, await (_projection.GetTypedValuesAsync(criteria, criteriaQuery)));
			return list.ToArray();
		}
	}
}