#if NET_4_5
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Driver
{
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
