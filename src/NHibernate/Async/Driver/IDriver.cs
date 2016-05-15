#if NET_4_5
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Driver
{
	/// <summary>
	/// A strategy for describing how NHibernate should interact with the different .NET Data
	/// Providers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <c>IDriver</c> interface is not intended to be exposed to the application.
	/// Instead it is used internally by NHibernate to obtain connection objects, command objects, and
	/// to generate and prepare <see cref = "DbCommand">DbCommands</see>. Implementors should provide a
	/// public default constructor.
	/// </para>
	/// <para>
	/// This is the interface to implement, or you can inherit from <see cref = "DriverBase"/> 
	/// if you have an ADO.NET data provider that NHibernate does not have built in support for.
	/// To use the driver, NHibernate property <c>connection.driver_class</c> should be
	/// set to the assembly-qualified name of the driver class.
	/// </para>
	/// <code>
	/// key="connection.driver_class"
	/// value="FullyQualifiedClassName, AssemblyName"
	/// </code>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IDriver
	{
		/// <summary>
		/// Creates an uninitialized DbConnection object for the specific Driver
		/// </summary>
		Task<DbConnection> CreateConnectionAsync();
	}
}
#endif
