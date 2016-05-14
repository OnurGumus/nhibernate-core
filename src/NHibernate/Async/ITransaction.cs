#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Transaction;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// Allows the application to define units of work, while maintaining abstraction from the
	/// underlying transaction implementation
	/// </summary>
	/// <remarks>
	/// A transaction is associated with a <c>ISession</c> and is usually instantiated by a call to
	/// <c>ISession.BeginTransaction()</c>. A single session might span multiple transactions since 
	/// the notion of a session (a conversation between the application and the datastore) is of
	/// coarser granularity than the notion of a transaction. However, it is intended that there be
	/// at most one uncommitted <c>ITransaction</c> associated with a particular <c>ISession</c>
	/// at a time. Implementors are not intended to be threadsafe.
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ITransaction : IDisposable
	{
		/// <summary>
		/// Flush the associated <c>ISession</c> and end the unit of work.
		/// </summary>
		/// <remarks>
		/// This method will commit the underlying transaction if and only if the transaction
		/// was initiated by this object.
		/// </remarks>
		Task CommitAsync();
	}
}
#endif
