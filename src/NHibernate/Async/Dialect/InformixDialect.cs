#if NET_4_5
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Dialect.Function;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.Util;
using System.Threading.Tasks;
using System;

//using NHibernate.Dialect.Schema;
namespace NHibernate.Dialect
{
	/// <summary>
	/// Summary description for InformixDialect.
	/// This dialect is intended to work with IDS version 7.31 
	/// However I can test only version 10.00 as I have only this version at work
	/// </summary>
	/// <remarks>
	/// The InformixDialect defaults the following configuration properties:
	/// <list type = "table">
	///		<listheader>
	///			<term>ConnectionDriver</term>
	///			<description>NHibernate.Driver.OdbcDriver</description>
	///			<term>PrepareSql</term>
	///			<description>true</description>
	///		</listheader>
	///		<item>
	///			<term>connection.driver_class</term>
	///			<description><see cref = "NHibernate.Driver.OdbcDriver"/></description>
	///		</item>
	/// </list>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InformixDialect : Dialect
	{
		public override Task<DbDataReader> GetResultSetAsync(DbCommand statement)
		{
			return statement.ExecuteReaderAsync(CommandBehavior.SingleResult);
		}
	}
}
#endif
