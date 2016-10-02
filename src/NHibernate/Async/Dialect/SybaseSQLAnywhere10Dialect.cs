#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.SqlCommand;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Dialect
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SybaseSQLAnywhere10Dialect : Dialect
	{
		public override async Task<DbDataReader> GetResultSetAsync(DbCommand statement)
		{
			DbDataReader rdr = await (statement.ExecuteReaderAsync());
			return rdr;
		}
	}
}
#endif
