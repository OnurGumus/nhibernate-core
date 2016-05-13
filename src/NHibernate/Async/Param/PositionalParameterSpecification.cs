using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Param
{
	/// <summary>
	/// Parameter bind specification for an explicit  positional (or ordinal) parameter.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PositionalParameterSpecification : AbstractExplicitParameterSpecification
	{
		public override Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			return BindAsync(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
		}

		public override async Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			IType type = ExpectedType;
			object value = queryParameters.PositionalParameterValues[hqlPosition];
			string backTrackId = GetIdsForBackTrack(session.Factory).First(); // just the first because IType suppose the oders in certain sequence
			// an HQL positional parameter can appear more than once because a custom HQL-Function can duplicate it
			foreach (int position in sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId))
			{
				await (type.NullSafeSetAsync(command, GetPagingValue(value, session.Factory.Dialect, queryParameters), position + singleSqlParametersOffset, session));
			}
		}
	}
}