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
