#if NET_4_5
using System;
using System.Collections;
using NHibernate.Transform;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IMultiQuery
	{
		/// <summary>
		/// Get all the results
		/// </summary>
		/// <remarks>
		/// The result is a IList of IList.
		/// </remarks>
		Task<IList> ListAsync();
		/// <summary>
		/// Bind an instance of a mapped persistent class to a named parameter.
		/// </summary>
		/// <param name = "name">The name of the parameter</param>
		/// <param name = "val">A non-null instance of a persistent class</param>
		/// <returns>The instance for method chain.</returns>
		Task<IMultiQuery> SetEntityAsync(string name, object val);
		/// <summary>
		/// Returns the result of one of the query based on the key
		/// </summary>
		/// <param name = "key">The key</param>
		/// <returns>The instance for method chain.</returns>
		Task<object> GetResultAsync(string key);
	}
}
#endif
