using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Util;


namespace NHibernate.Connection
{
	/// <summary>
	/// A ConnectionProvider that uses an IDriver to create connections.
	/// </summary>
	public class DriverConnectionProvider : ConnectionProvider
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(DriverConnectionProvider));

		/// <summary>
		/// Closes and Disposes of the <see cref="IDbConnection"/>.
		/// </summary>
		/// <param name="conn">The <see cref="IDbConnection"/> to clean up.</param>
		public override void CloseConnection(IDbConnection conn)
		{
			base.CloseConnection(conn);
			conn.Dispose();
		}

		/// <summary>
		/// Gets a new open <see cref="IDbConnection"/> through 
		/// the <see cref="NHibernate.Driver.IDriver"/>.
		/// </summary>
		/// <returns>
		/// An Open <see cref="IDbConnection"/>.
		/// </returns>
		/// <exception cref="Exception">
		/// If there is any problem creating or opening the <see cref="IDbConnection"/>.
		/// </exception>
		public override DbConnection GetConnection()
		{
			return GetConnectionAsync().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		/// <summary>
		/// Gets a new open <see cref="IDbConnection"/> through 
		/// the <see cref="NHibernate.Driver.IDriver"/>.
		/// </summary>
		/// <returns>
		/// An Open <see cref="IDbConnection"/>.
		/// </returns>
		/// <exception cref="Exception">
		/// If there is any problem creating or opening the <see cref="IDbConnection"/>.
		/// </exception>
		public override async Task<DbConnection> GetConnectionAsync()
		{
			log.Debug("Obtaining IDbConnection from Driver");
			DbConnection conn = Driver.CreateConnection();
			try
			{
				conn.ConnectionString = ConnectionString;
				await conn.OpenAsync().ConfigureAwait(false);
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