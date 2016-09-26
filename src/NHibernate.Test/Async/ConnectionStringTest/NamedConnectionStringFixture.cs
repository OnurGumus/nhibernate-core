#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Connection;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.ConnectionStringTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MockConnectionProvider : ConnectionProvider
	{
		/// <summary>
		/// Get an open <see cref = "DbConnection"/>.
		/// </summary>
		/// <returns>An open <see cref = "DbConnection"/>.</returns>
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
