#if NET_4_5
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Connection
{
	/// <summary>
	/// A ConnectionProvider that uses an IDriver to create connections.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DriverConnectionProvider : ConnectionProvider
	{
		/// <summary>
		/// Gets a new open <see cref = "DbConnection"/> through 
		/// the <see cref = "NHibernate.Driver.IDriver"/>.
		/// </summary>
		/// <returns>
		/// An Open <see cref = "DbConnection"/>.
		/// </returns>
		/// <exception cref = "Exception">
		/// If there is any problem creating or opening the <see cref = "DbConnection"/>.
		/// </exception>
		public override async Task<DbConnection> GetConnectionAsync()
		{
			log.Debug("Obtaining DbConnection from Driver");
			DbConnection conn = await (Driver.CreateConnectionAsync());
			try
			{
				conn.ConnectionString = ConnectionString;
				await (conn.OpenAsync());
			}
			catch (Exception)
			{
				conn.Dispose();
				throw;
			}

			return conn;
		}
	}
}
#endif
