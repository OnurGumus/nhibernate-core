#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Proxy
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILazyInitializer
	{
		/// <summary>
		/// Perform an ImmediateLoad of the actual object for the Proxy.
		/// </summary>
		/// <exception cref = "HibernateException">
		/// Thrown when the Proxy has no Session or the Session is closed or disconnected.
		/// </exception>
		Task InitializeAsync();
		/// <summary>
		/// Return the underlying persistent object, initializing if necessary.
		/// </summary>
		/// <returns>The persistent object this proxy is proxying.</returns>
		Task<object> GetImplementationAsync();
		/// <summary>
		/// Associate the proxy with the given session.
		///
		/// Care should be given to make certain that the proxy is added to the session's persistence context as well
		/// to maintain the symmetry of the association.  That must be done separately as this method simply sets an
		/// internal reference.  We do also check that if there is already an associated session that the proxy
		/// reference was removed from that previous session's persistence context.
		/// </summary>
		/// <param name = "s">The session</param>
		Task SetSessionAsync(ISessionImplementor s);
	}
}
#endif
