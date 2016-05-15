#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Driver
{
	/// <summary>
	/// NHibernate driver for the System.Data.SQLite data provider for .NET 2.0.
	/// </summary>
	/// <remarks>
	/// <p>
	/// In order to use this driver you must have the System.Data.SQLite.dll assembly available
	/// for NHibernate to load. This assembly includes the SQLite.dll or SQLite3.dll libraries.
	/// </p>    
	/// <p>
	/// You can get the System.Data.SQLite.dll assembly from http://sourceforge.net/projects/sqlite-dotnet2.
	/// </p>
	/// <p>
	/// Please check <a href = "http://www.sqlite.org/">http://www.sqlite.org/</a> for more information regarding SQLite.
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SQLite20Driver : ReflectionBasedDriver
	{
		public override async Task<DbConnection> CreateConnectionAsync()
		{
			DbConnection connection = (DbConnection)await (base.CreateConnectionAsync());
			connection.StateChange += Connection_StateChange;
			return connection;
		}

		private static async Task Connection_StateChangeAsync(object sender, StateChangeEventArgs e)
		{
			if ((e.OriginalState == ConnectionState.Broken || e.OriginalState == ConnectionState.Closed || e.OriginalState == ConnectionState.Connecting) && e.CurrentState == ConnectionState.Open)
			{
				DbConnection connection = (DbConnection)sender;
				using (DbCommand command = connection.CreateCommand())
				{
					// Activated foreign keys if supported by SQLite.  Unknown pragmas are ignored.
					command.CommandText = "PRAGMA foreign_keys = ON";
					await (command.ExecuteNonQueryAsync());
				}
			}
		}
	}
}
#endif
