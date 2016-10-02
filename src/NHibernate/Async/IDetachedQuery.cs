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
#endif
