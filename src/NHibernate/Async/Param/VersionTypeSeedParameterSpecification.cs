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
	public partial class VersionTypeSeedParameterSpecification : IParameterSpecification
	{
		public async Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			int position = sqlQueryParametersList.GetEffectiveParameterLocations(IdBackTrack).Single(); // version parameter can't appear more than once
			await (type.NullSafeSetAsync(command, await (type.SeedAsync(session)), position, session));
		}

		public async Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			throw new NotSupportedException("Not supported for multiquery loader.");
		}
	}
}