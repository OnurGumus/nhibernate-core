using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Criterion
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SQLCriterion : AbstractCriterion
	{
		public override async Task<SqlString> ToSqlStringAsync(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
		{
			var parameters = _sql.GetParameters().ToList();
			var paramPos = 0;
			for (int i = 0; i < _typedValues.Length; i++)
			{
				var controlledParameters = criteriaQuery.NewQueryParameter(_typedValues[i]);
				foreach (Parameter parameter in controlledParameters)
				{
					parameters[paramPos++].BackTrack = parameter.BackTrack;
				}
			}

			return _sql.Replace("{alias}", criteriaQuery.GetSQLAlias(criteria));
		}

		public override async Task<TypedValue[]> GetTypedValuesAsync(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return _typedValues;
		}
	}
}