#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Connection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UserSuppliedConnectionProvider : ConnectionProvider
	{
		/// <summary>
		/// Throws an <see cref = "InvalidOperationException"/> if this method is called
		/// because the user is responsible for creating <see cref = "DbConnection"/>s.
		/// </summary>
		/// <returns>
		/// No value is returned because an <see cref = "InvalidOperationException"/> is thrown.
		/// </returns>
		/// <exception cref = "InvalidOperationException">
		/// Thrown when this method is called.  User is responsible for creating
		/// <see cref = "DbConnection"/>s.
		/// </exception>
		public override Task<DbConnection> GetConnectionAsync()
		{
			try
			{
				return Task.FromResult<DbConnection>(GetConnection());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<DbConnection>(ex);
			}
		}
	}
}
#endif
