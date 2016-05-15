#if NET_4_5
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using NHibernate.AdoNet;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Driver
{
	/// <summary>
	/// A NHibernate Driver for using the SqlClient DataProvider
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlClientDriver : DriverBase, IEmbeddedBatcherFactoryProvider
	{
		/// <summary>
		/// Creates an uninitialized <see cref = "DbConnection"/> object for
		/// the SqlClientDriver.
		/// </summary>
		/// <value>An unitialized <see cref = "System.Data.SqlClient.SqlConnection"/> object.</value>
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
