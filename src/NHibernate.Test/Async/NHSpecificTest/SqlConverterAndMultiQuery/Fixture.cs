#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.SqlConverterAndMultiQuery
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private const string hqlQuery = "select a.Id from ClassA a";
		protected override void Configure(Configuration configuration)
		{
			configuration.DataBaseIntegration(x => x.ExceptionConverter<SqlConverter>());
		}

		[Test]
		public async Task NormalHqlShouldThrowUserExceptionAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					s.Connection.Close();
					Assert.ThrowsAsync<UnitTestException>(async () => await (s.CreateQuery(hqlQuery).ListAsync()));
				}
		}

		[Test]
		public async Task MultiHqlShouldThrowUserExceptionAsync()
		{
			var driver = sessions.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var multi = s.CreateMultiQuery();
					multi.Add(hqlQuery);
					s.Connection.Close();
					Assert.ThrowsAsync<UnitTestException>(async () => await (multi.ListAsync()));
				}
		}

		[Test]
		public async Task NormalCriteriaShouldThrowUserExceptionAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					s.Connection.Close();
					Assert.ThrowsAsync<UnitTestException>(async () => await (s.CreateCriteria(typeof (ClassA)).ListAsync()));
				}
		}

		[Test]
		public async Task MultiCriteriaShouldThrowUserExceptionAsync()
		{
			var driver = sessions.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var multi = s.CreateMultiCriteria();
					multi.Add(s.CreateCriteria(typeof (ClassA)));
					s.Connection.Close();
					Assert.ThrowsAsync<UnitTestException>(async () => await (multi.ListAsync()));
				}
		}
	}
}
#endif
