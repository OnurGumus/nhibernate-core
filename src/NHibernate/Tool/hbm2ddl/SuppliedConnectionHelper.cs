using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Tool.hbm2ddl
{
	/// <summary>
	/// A <seealso cref="IConnectionHelper"/> implementation based on an explicitly supplied
	/// connection.
	/// </summary>
	public class SuppliedConnectionHelper : IConnectionHelper
	{
		private DbConnection connection;

		public SuppliedConnectionHelper(DbConnection connection)
		{
			this.connection = connection;
		}

		public void Prepare()
		{
		}

		public Task PrepareAsync()
		{
			return TaskHelper.CompletedTask;
		}

		public DbConnection Connection
		{
			get { return connection; }
		}

		public void Release()
		{
			connection = null;
		}
	}
}
