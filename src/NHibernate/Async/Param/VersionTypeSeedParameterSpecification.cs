#if NET_4_5
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
	public partial class VersionTypeSeedParameterSpecification : IParameterSpecification
	{
		public async Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			int position = sqlQueryParametersList.GetEffectiveParameterLocations(IdBackTrack).Single(); // version parameter can't appear more than once
			await (type.NullSafeSetAsync(command, await (type.SeedAsync(session)), position, session));
		}

		public Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session)
		{
			try
			{
				Bind(command, multiSqlQueryParametersList, singleSqlParametersOffset, sqlQueryParametersList, queryParameters, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
