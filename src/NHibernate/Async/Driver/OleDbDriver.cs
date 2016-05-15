#if NET_4_5
using System;
using System.Data.Common;
using System.Data.OleDb;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Driver
{
	/// <summary>
	/// A NHibernate Driver for using the OleDb DataProvider
	/// </summary>
	/// <remarks>
	/// Always look for a native .NET DataProvider before using the OleDb DataProvider.
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OleDbDriver : DriverBase
	{
		public override Task<DbConnection> CreateConnectionAsync()
		{
			try
			{
				return Task.FromResult<DbConnection>(CreateConnection());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<DbConnection>(ex);
			}
		}
	}
}
#endif
