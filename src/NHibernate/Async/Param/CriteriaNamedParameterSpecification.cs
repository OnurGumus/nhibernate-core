#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Param
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CriteriaNamedParameterSpecification : IParameterSpecification
	{
		public Task BindAsync(DbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			return BindAsync(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
		}

		public async Task BindAsync(DbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
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
#endif
