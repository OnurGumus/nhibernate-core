#if NET_4_5
using System.Collections.Generic;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Hql
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IFilterTranslator : IQueryTranslator
	{
		/// <summary> 
		/// Compile a filter. This method may be called multiple
		/// times. Subsequent invocations are no-ops.
		/// </summary>
		/// <param name = "collectionRole">the role name of the collection used as the basis for the filter.</param>
		/// <param name = "replacements">Defined query substitutions.</param>
		/// <param name = "shallow">Does this represent a shallow (scalar or entity-id) select?</param>
		Task CompileAsync(string collectionRole, IDictionary<string, string> replacements, bool shallow);
	}
}
#endif
