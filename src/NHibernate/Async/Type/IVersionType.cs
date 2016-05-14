#if NET_4_5
using System.Collections;
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Type
{
	/// <summary>
	/// An <see cref = "IType"/> that may be used to version data.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IVersionType : IType
	{
		/// <summary>
		/// When implemented by a class, increments the version.
		/// </summary>
		/// <param name = "current">The current version</param>
		/// <param name = "session">The current session, if available.</param>
		/// <returns>an instance of the <see cref = "IType"/> that has been incremented.</returns>
		Task<object> NextAsync(object current, ISessionImplementor session);
		/// <summary>
		/// When implemented by a class, gets an initial version.
		/// </summary>
		/// <param name = "session">The current session, if available.</param>
		/// <returns>An instance of the type.</returns>
		Task<object> SeedAsync(ISessionImplementor session);
	}
}
#endif
