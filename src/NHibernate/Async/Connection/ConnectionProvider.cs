#if NET_4_5
using System;
using System.Collections;
using System.Configuration;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.Util;
using Environment = NHibernate.Cfg.Environment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Connection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class ConnectionProvider : IConnectionProvider
	{
		/// <summary>
		/// Get an open <see cref = "DbConnection"/>.
		/// </summary>
		/// <returns>An open <see cref = "DbConnection"/>.</returns>
		public abstract Task<DbConnection> GetConnectionAsync();
	}
}
#endif
