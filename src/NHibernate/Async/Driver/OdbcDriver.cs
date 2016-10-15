#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using NHibernate.SqlTypes;
using NHibernate.Util;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OdbcDriver : DriverBase
	{
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
