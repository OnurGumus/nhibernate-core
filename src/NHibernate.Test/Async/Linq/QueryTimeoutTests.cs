#if NET_4_5
using System.Data.Common;
using System.Linq;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryTimeoutTestsAsync : LinqTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Environment.BatchStrategy, typeof (TimeoutCatchingNonBatchingBatcherFactory).AssemblyQualifiedName);
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class TimeoutCatchingNonBatchingBatcher : NonBatchingBatcher
		{
			// Is there an easier way to inspect the DbCommand instead of
			// creating a custom batcher?
			public static int LastCommandTimeout;
			public TimeoutCatchingNonBatchingBatcher(ConnectionManager connectionManager, IInterceptor interceptor): base (connectionManager, interceptor)
			{
			}

			public override async Task<DbDataReader> ExecuteReaderAsync(DbCommand cmd)
			{
				LastCommandTimeout = cmd.CommandTimeout;
				return await (base.ExecuteReaderAsync(cmd));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class TimeoutCatchingNonBatchingBatcherFactory : IBatcherFactory
		{
			public IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
			{
				return new TimeoutCatchingNonBatchingBatcher(connectionManager, interceptor);
			}
		}
	}
}
#endif
