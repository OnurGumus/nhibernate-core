#if NET_4_5
using System.Data.Common;
using System.Linq;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryTimeoutTests : LinqTestCase
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class TimeoutCatchingNonBatchingBatcher : NonBatchingBatcher
		{
			public override async Task<DbDataReader> ExecuteReaderAsync(DbCommand cmd)
			{
				LastCommandTimeout = cmd.CommandTimeout;
				return await (base.ExecuteReaderAsync(cmd));
			}
		}
	}
}
#endif
