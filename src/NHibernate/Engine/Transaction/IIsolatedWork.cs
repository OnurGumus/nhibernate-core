using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Engine.Transaction
{
	/// <summary>
	/// Represents work that needs to be performed in a manner
	/// which isolates it from any current application unit of
	/// work transaction.
	/// </summary>
	public interface IIsolatedWork
	{
		/// <summary>
		/// Perform the actual work to be done.
		/// </summary>
		/// <param name="connection">The ADP connection to use.</param>
		/// <param name="transaction">The active transaction of the connection.</param>
		Task DoWork(DbConnection connection, DbTransaction transaction);
	}
}