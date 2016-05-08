using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Dialect.Lock
{
	/// <summary> 
	/// A strategy abstraction for how locks are obtained in the underlying database.
	/// </summary>
	/// <remarks>
	/// All locking provided implementations assume the underlying database supports
	/// (and that the connection is in) at least read-committed transaction isolation.
	/// The most glaring exclusion to this is HSQLDB which only offers support for
	/// READ_UNCOMMITTED isolation.
	/// </remarks>
	/// <seealso cref = "NHibernate.Dialect.Dialect.GetLockingStrategy"/>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILockingStrategy
	{
		Task LockAsync(object id, object version, object obj, ISessionImplementor session);
	}
}