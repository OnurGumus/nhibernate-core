#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Event;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Hql
{
	/// <summary>
	/// Defines the contract of an HQL->SQL translator.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IQueryTranslator
	{
		// Not ported: 
		// Error message constants moved to the implementation of Classic.QueryTranslator (C# can't have fields in interface)
		//public const string ErrorCannotFetchWithIterate = "fetch may not be used with scroll() or iterate()";
		//public const string ErrorNamedParameterDoesNotAppear = "Named parameter does not appear in Query: ";
		//public const string ErrorCannotDetermineType = "Could not determine type of: ";
		//public const string ErrorCannotFormatLiteral = "Could not format constant value to SQL literal: ";
		/// <summary>
		/// Compile a "normal" query. This method may be called multiple times. Subsequent invocations are no-ops.
		/// </summary>
		/// <param name = "replacements">Defined query substitutions.</param>
		/// <param name = "shallow">Does this represent a shallow (scalar or entity-id) select?</param>
		/// <exception cref = "NHibernate.QueryException">There was a problem parsing the query string.</exception>
		/// <exception cref = "NHibernate.MappingException">There was a problem querying defined mappings.</exception>
		Task CompileAsync(IDictionary<string, string> replacements, bool shallow);
		/// <summary>
		/// Perform a list operation given the underlying query definition.
		/// </summary>
		/// <param name = "session">The session owning this query.</param>
		/// <param name = "queryParameters">The query bind parameters.</param>
		/// <returns>The query list results.</returns>
		/// <exception cref = "NHibernate.HibernateException"></exception>
		Task<IList> ListAsync(ISessionImplementor session, QueryParameters queryParameters);
		Task<IEnumerable> GetEnumerableAsync(QueryParameters queryParameters, IEventSource session);
		// Not ported:
		//IScrollableResults scroll(QueryParameters queryParameters, ISessionImplementor session);
		/// <summary>
		/// Perform a bulk update/delete operation given the underlying query definition.
		/// </summary>
		/// <param name = "queryParameters">The query bind parameters.</param>
		/// <param name = "session">The session owning this query.</param>
		/// <returns>The number of entities updated or deleted.</returns>
		/// <exception cref = "NHibernate.HibernateException"></exception>
		Task<int> ExecuteUpdateAsync(QueryParameters queryParameters, ISessionImplementor session);
	}
}
#endif
