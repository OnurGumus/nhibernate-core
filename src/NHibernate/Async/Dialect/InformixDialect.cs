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
using Exception = System.Exception;

//using NHibernate.Dialect.Schema;
namespace NHibernate.Dialect
{
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
