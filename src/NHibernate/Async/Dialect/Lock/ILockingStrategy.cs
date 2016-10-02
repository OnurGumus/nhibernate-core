#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Dialect.Lock
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILockingStrategy
	{
		/// <summary> 
		/// Acquire an appropriate type of lock on the underlying data that will
		/// endure until the end of the current transaction.
		/// </summary>
		/// <param name = "id">The id of the row to be locked </param>
		/// <param name = "version">The current version (or null if not versioned) </param>
		/// <param name = "obj">The object logically being locked (currently not used) </param>
		/// <param name = "session">The session from which the lock request originated </param>
		Task LockAsync(object id, object version, object obj, ISessionImplementor session);
	}
}
#endif
