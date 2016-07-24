#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1171
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			// Firebird has issues with comments containing apostrophes
			return !(dialect is FirebirdDialect);
		}

		protected override Task ConfigureAsync(NHibernate.Cfg.Configuration configuration)
		{
			try
			{
				configuration.SetProperty(Environment.FormatSql, "false");
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task SupportSQLQueryWithCommentsAsync()
		{
			string sql = @"
SELECT id 
FROM tablea 
-- Comment with ' number 1 
WHERE Name = :name 
/* Comment with ' number 2 */ 
ORDER BY Name 
";
			using (ISession s = OpenSession())
			{
				var q = s.CreateSQLQuery(sql);
				q.SetString("name", "Evgeny Potashnik");
				await (q.ListAsync());
			}
		}

		[Test]
		public async Task ExecutedContainsCommentsAsync()
		{
			string sql = @"
SELECT id 
FROM tablea 
-- Comment with ' number 1 
WHERE Name = :name 
/* Comment with ' number 2 */ 
ORDER BY Name 
";
			using (var ls = new SqlLogSpy())
			{
				using (ISession s = OpenSession())
				{
					var q = s.CreateSQLQuery(sql);
					q.SetString("name", "Evgeny Potashnik");
					await (q.ListAsync());
				}

				string message = ls.GetWholeLog();
				Assert.That(message, Is.StringContaining("-- Comment with ' number 1"));
				Assert.That(message, Is.StringContaining("/* Comment with ' number 2 */"));
			}
		}
	}
}
#endif
