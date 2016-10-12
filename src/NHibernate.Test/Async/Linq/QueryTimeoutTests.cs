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
	public partial class QueryTimeoutTestsAsync : LinqTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			base.Configure(configuration);
			configuration.SetProperty(Environment.BatchStrategy, typeof (TimeoutCatchingNonBatchingBatcherFactory).AssemblyQualifiedName);
		}

		[Test]
		public async Task CanSetTimeoutOnLinqQueriesAsync()
		{
			var result = await ((
				from e in db.Customers
				where e.CompanyName == "Corp"
				select e).Timeout(17).ToListAsync());
			Assert.That(TimeoutCatchingNonBatchingBatcher.LastCommandTimeout, Is.EqualTo(17));
		}

		[Test]
		public async Task CanSetTimeoutOnLinqPagingQueryAsync()
		{
			var result = await ((
				from e in db.Customers
				where e.CompanyName == "Corp"
				select e).Skip(5).Take(5).Timeout(17).ToListAsync());
			Assert.That(TimeoutCatchingNonBatchingBatcher.LastCommandTimeout, Is.EqualTo(17));
		}

		[Test]
		public async Task CanSetTimeoutBeforeSkipOnLinqOrderedPageQueryAsync()
		{
			var result = await ((
				from e in db.Customers
				orderby e.CompanyName
				select e).Timeout(17).Skip(5).Take(5).ToListAsync());
			Assert.That(TimeoutCatchingNonBatchingBatcher.LastCommandTimeout, Is.EqualTo(17));
		}

		[Test]
		public async Task CanSetTimeoutOnLinqGroupPageQueryAsync()
		{
			var subQuery = db.Customers.Where(e2 => e2.CompanyName.Contains("a")).Select(e2 => e2.CustomerId).Timeout(18); // This Timeout() should not cause trouble, and be ignored.
			var result = await ((
				from e in db.Customers
				where subQuery.Contains(e.CustomerId)group e by e.CompanyName into g
					select new
					{
					g.Key, Count = g.Count()}

			).Skip(5).Take(5).Timeout(17).ToListAsync());
			Assert.That(TimeoutCatchingNonBatchingBatcher.LastCommandTimeout, Is.EqualTo(17));
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
