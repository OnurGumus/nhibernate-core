#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Driver;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Connection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IConnectionProvider : IDisposable
	{
		/// <summary>
		/// Get an open <see cref = "DbConnection"/>.
		/// </summary>
		/// <returns>An open <see cref = "DbConnection"/>.</returns>
		Task<DbConnection> GetConnectionAsync();
	}
}
#endif
