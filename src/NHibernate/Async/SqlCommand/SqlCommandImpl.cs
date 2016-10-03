#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.SqlCommand
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ISqlCommand
	{
		/// <summary>
		/// Bind the appropriate value into the given command.
		/// </summary>
		/// <param name = "command">The command into which the value should be bound.</param>
		/// <param name = "commandQueryParametersList">The parameter-list of the whole query of the command.</param>
		/// <param name = "singleSqlParametersOffset">The offset from where start the list of <see cref = "DbParameter"/>, in the given <paramref name = "command"/>, for the this <see cref = "SqlCommandImpl"/>. </param>
		/// <param name = "session">The session against which the current execution is occurring.</param>
		/// <remarks>
		/// Suppose the <paramref name = "command"/> is composed by two queries. The <paramref name = "singleSqlParametersOffset"/> for the first query is zero.
		/// If the first query in <paramref name = "command"/> has 12 parameters (size of its SqlType array) the offset to bind all <see cref = "IParameterSpecification"/>s, of the second query in the
		/// <paramref name = "command"/>, is 12 (for the first query we are using from 0 to 11).
		/// </remarks>
		Task BindAsync(DbCommand command, IList<Parameter> commandQueryParametersList, int singleSqlParametersOffset, ISessionImplementor session);
		/// <summary>
		/// Bind the appropriate value into the given command.
		/// </summary>
		/// <param name = "command">The command into which the value should be bound.</param>
		/// <param name = "session">The session against which the current execution is occurring.</param>
		/// <remarks>
		/// Use this method when the <paramref name = "command"/> contains just 'this' instance of <see cref = "ISqlCommand"/>.
		/// Use the overload <see cref = "Bind(DbCommand, IList{Parameter}, int, ISessionImplementor)"/> when the <paramref name = "command"/> contains more instances of <see cref = "ISqlCommand"/>.
		/// </remarks>
		Task BindAsync(DbCommand command, ISessionImplementor session);
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlCommandImpl : ISqlCommand
	{
		/// <summary>
		/// Bind the appropriate value into the given command.
		/// </summary>
		/// <param name = "command">The command into which the value should be bound.</param>
		/// <param name = "commandQueryParametersList">The parameter-list of the whole query of the command.</param>
		/// <param name = "singleSqlParametersOffset">The offset from where start the list of <see cref = "DbParameter"/>, in the given <paramref name = "command"/>, for the this <see cref = "SqlCommandImpl"/>. </param>
		/// <param name = "session">The session against which the current execution is occuring.</param>
		public async Task BindAsync(DbCommand command, IList<Parameter> commandQueryParametersList, int singleSqlParametersOffset, ISessionImplementor session)
		{
			foreach (IParameterSpecification parameterSpecification in Specifications)
			{
				await (parameterSpecification.BindAsync(command, commandQueryParametersList, singleSqlParametersOffset, SqlQueryParametersList, QueryParameters, session));
			}
		}

		/// <summary>
		/// Bind the appropriate value into the given command.
		/// </summary>
		/// <param name = "command">The command into which the value should be bound.</param>
		/// <param name = "session">The session against which the current execution is occuring.</param>
		/// <remarks>
		/// Use this method when the <paramref name = "command"/> contains just 'this' instance of <see cref = "ISqlCommand"/>.
		/// Use the overload <see cref = "Bind(DbCommand, IList{Parameter}, int, ISessionImplementor)"/> when the <paramref name = "command"/> contains more instances of <see cref = "ISqlCommand"/>.
		/// </remarks>
		public async Task BindAsync(DbCommand command, ISessionImplementor session)
		{
			foreach (IParameterSpecification parameterSpecification in Specifications)
			{
				await (parameterSpecification.BindAsync(command, SqlQueryParametersList, QueryParameters, session));
			}
		}
	}
}
#endif
