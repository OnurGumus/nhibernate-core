#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Environment = NHibernate.Cfg.Environment;
using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Dialect
{
	/// <summary>
	/// An SQL dialect targeting Sybase Adaptive Server Enterprise (ASE) 15 and higher.
	/// </summary>
	/// <remarks>
	/// The dialect defaults the following configuration properties:
	/// <list type = "table">
	///	<listheader>
	///		<term>Property</term>
	///		<description>Default Value</description>
	///	</listheader>
	///	<item>
	///		<term>connection.driver_class</term>
	///		<description><see cref = "NHibernate.Driver.SybaseAseClientDriver"/></description>
	///	</item>
	/// </list>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SybaseASE15Dialect : Dialect
	{
		public override Task<DbDataReader> GetResultSetAsync(DbCommand statement)
		{
			return statement.ExecuteReaderAsync();
		}
	}
}
#endif
