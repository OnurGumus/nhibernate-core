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
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionFilterKeyParameterSpecification : IParameterSpecification
	{
		public async Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			IType type = keyType;
			object value = queryParameters.PositionalParameterValues[queryParameterPosition];
			string backTrackId = GetIdsForBackTrack(session.Factory).First(); // just the first because IType suppose the oders in certain sequence
			int position = sqlQueryParametersList.GetEffectiveParameterLocations(backTrackId).Single(); // an HQL positional parameter can't appear more than once
			await (type.NullSafeSetAsync(command, value, position + singleSqlParametersOffset, session));
		}

		public Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			return BindAsync(command, sqlQueryParametersList, 0, sqlQueryParametersList, queryParameters, session);
		}
	}
}