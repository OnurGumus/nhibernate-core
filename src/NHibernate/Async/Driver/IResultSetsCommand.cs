#if NET_4_5
using System.Data.Common;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IResultSetsCommand
	{
		Task<DbDataReader> GetReaderAsync(int ? commandTimeout);
	}
}
#endif
