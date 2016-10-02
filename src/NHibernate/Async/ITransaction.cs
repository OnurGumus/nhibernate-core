#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Transaction;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
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
