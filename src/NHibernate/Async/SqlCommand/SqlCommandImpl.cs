using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.SqlCommand
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlCommandImpl : ISqlCommand
	{
		public async Task BindAsync(IDbCommand command, ISessionImplementor session)
		{
			foreach (IParameterSpecification parameterSpecification in Specifications)
			{
				await (parameterSpecification.BindAsync(command, SqlQueryParametersList, QueryParameters, session));
			}
		}

		public async Task BindAsync(IDbCommand command, IList<Parameter> commandQueryParametersList, int singleSqlParametersOffset, ISessionImplementor session)
		{
			foreach (IParameterSpecification parameterSpecification in Specifications)
			{
				await (parameterSpecification.BindAsync(command, commandQueryParametersList, singleSqlParametersOffset, SqlQueryParametersList, QueryParameters, session));
			}
		}
	}
}