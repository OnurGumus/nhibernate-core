#if NET_4_5
using System.Data.Common;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Engine.Transaction
{
	/// <summary>
	/// Represents work that needs to be performed in a manner
	/// which isolates it from any current application unit of
	/// work transaction.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IIsolatedWork
	{
		/// <summary>
		/// Perform the actual work to be done.
		/// </summary>
		/// <param name = "connection">The ADP connection to use.</param>
		/// <param name = "transaction">The active transaction of the connection.</param>
		Task DoWorkAsync(DbConnection connection, DbTransaction transaction);
	}
}
#endif
