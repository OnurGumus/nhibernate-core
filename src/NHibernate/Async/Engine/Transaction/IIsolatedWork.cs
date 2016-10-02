#if NET_4_5
using System.Data.Common;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Engine.Transaction
{
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
