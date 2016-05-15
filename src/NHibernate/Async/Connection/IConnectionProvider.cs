#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Driver;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Connection
{
	/// <summary>
	/// A strategy for obtaining ADO.NET <see cref = "DbConnection"/>.
	/// </summary>
	/// <remarks>
	/// The <c>IConnectionProvider</c> interface is not intended to be exposed to the application.
	/// Instead it is used internally by NHibernate to obtain <see cref = "DbConnection"/>. 
	/// Implementors should provide a public default constructor.
	/// </remarks>
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

namespace NHibernate.Connection
{
	/// <summary>
	/// A strategy for obtaining ADO.NET <see cref = "DbConnection"/>.
	/// </summary>
	/// <remarks>
	/// The <c>IConnectionProvider</c> interface is not intended to be exposed to the application.
	/// Instead it is used internally by NHibernate to obtain <see cref = "DbConnection"/>. 
	/// Implementors should provide a public default constructor.
	/// </remarks>
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
