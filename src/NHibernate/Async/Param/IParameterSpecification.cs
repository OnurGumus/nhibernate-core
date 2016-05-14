#if NET_4_5
using System.Collections.Generic;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Param
{
	/// <summary>
	/// Maintains information relating to parameters which need to get bound into a <see cref = "IDbCommand"/>.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IParameterSpecification
	{
		/// <summary>
		/// Bind the appropriate value into the given command.
		/// </summary>
		/// <param name = "command">The command into which the value should be bound.</param>
		/// <param name = "sqlQueryParametersList">The list of Sql query parameter in the exact sequence they are present in the query.</param>
		/// <param name = "queryParameters">The defined values for the current query execution.</param>
		/// <param name = "session">The session against which the current execution is occuring.</param>
		Task BindAsync(IDbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session);
		/// <summary>
		/// Bind the appropriate value into the given command.
		/// </summary>
		/// <param name = "command">The command into which the value should be bound.</param>
		/// <param name = "multiSqlQueryParametersList">The parameter-list of the whole query of the command.</param>
		/// <param name = "singleSqlParametersOffset">The offset from where start the list of <see cref = "IDataParameter"/> in the given <paramref name = "command"/> for the query where this <see cref = "IParameterSpecification"/> was used. </param>
		/// <param name = "sqlQueryParametersList">The list of Sql query parameter in the exact sequence they are present in the query where this <see cref = "IParameterSpecification"/> was used.</param>
		/// <param name = "queryParameters">The defined values for the query where this <see cref = "IParameterSpecification"/> was used.</param>
		/// <param name = "session">The session against which the current execution is occuring.</param>
		/// <remarks>
		/// Suppose the <paramref name = "command"/> is composed by two queries. The <paramref name = "singleSqlParametersOffset"/> for the first query is zero.
		/// If the first query in <paramref name = "command"/> has 12 parameters (size of its SqlType array) the offset to bind all <see cref = "IParameterSpecification"/>s of the second query in the
		/// <paramref name = "command"/> is 12 (for the first query we are using from 0 to 11).
		/// </remarks>
		Task BindAsync(IDbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session);
	}
}
#endif
