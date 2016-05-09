using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Param
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CriteriaNamedParameterSpecification : IParameterSpecification
	{
		public async Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			await (BindAsync(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session));
		}

		public async Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			TypedValue typedValue = queryParameters.NamedParameters[name];
			string backTrackId = GetIdsForBackTrack(session.Factory).First();
			foreach (int position in sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
			{
				await (ExpectedType.NullSafeSetAsync(command, typedValue.Value, position + singleSqlParametersOffset, session));
			}
		}
	}
}