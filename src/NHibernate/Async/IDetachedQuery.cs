using System;
using System.Collections;
using NHibernate.Transform;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// Interface  to create queries in "detached mode" where the NHibernate session is not available.
	/// All methods have the same semantics as the corresponding methods of the <see cref = "IQuery"/> interface.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IDetachedQuery
	{
		/// <summary>
		/// Bind an instance of a mapped persistent class to an indexed parameter.
		/// </summary>
		/// <param name = "position">Position of the parameter in the query string, numbered from <c>0</c></param>
		/// <param name = "val">A non-null instance of a persistent class</param>
		Task<IDetachedQuery> SetEntityAsync(int position, object val);
		/// <summary>
		/// Bind an instance of a mapped persistent class to a named parameter.
		/// </summary>
		/// <param name = "name">The name of the parameter</param>
		/// <param name = "val">A non-null instance of a persistent class</param>
		Task<IDetachedQuery> SetEntityAsync(string name, object val);
	}
}