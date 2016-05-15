#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Connection;
using System.Threading.Tasks;

namespace NHibernate.Test
{
	/// <summary>
	/// This connection provider keeps a list of all open connections,
	/// it is used when testing to check that tests clean up after themselves.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DebugConnectionProvider : DriverConnectionProvider
	{
		public override async Task<DbConnection> GetConnectionAsync()
		{
			try
			{
				DbConnection connection = await (base.GetConnectionAsync());
				connections.Add(connection);
				return connection;
			}
			catch (Exception e)
			{
				throw new HibernateException("Could not open connection to: " + ConnectionString, e);
			}
		}
	}
}
#endif
