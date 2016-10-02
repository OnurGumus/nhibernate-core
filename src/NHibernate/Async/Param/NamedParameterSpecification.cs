#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Param
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NamedParameterSpecification : AbstractExplicitParameterSpecification
	{
		public override Task BindAsync(DbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			return BindAsync(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
		}

		public override async Task BindAsync(DbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			TypedValue typedValue = queryParameters.NamedParameters[name];
			string backTrackId = GetIdsForBackTrack(session.Factory).First(); // just the first because IType suppose the oders in certain sequence
			foreach (int position in sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
			{
				await (ExpectedType.NullSafeSetAsync(command, GetPagingValue(typedValue.Value, session.Factory.Dialect, queryParameters), position + singleSqlParametersOffset, session));
			}
		}
	}
}
#endif
