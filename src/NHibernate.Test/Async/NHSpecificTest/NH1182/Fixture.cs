#if NET_4_5
using System.Collections.Generic;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1182
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.FormatSql, "false");
		}

		[Test]
		public async Task DeleteWithoutUpdateVersionAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new ObjectA{Bs = new List<ObjectB>{new ObjectB(), new ObjectB()}}));
					await (t.CommitAsync());
				}

			using (var ls = new SqlLogSpy())
			{
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						var a = await (s.CreateCriteria<ObjectA>().UniqueResultAsync<ObjectA>());
						await (s.DeleteAsync(a));
						await (t.CommitAsync());
					}

				string wholeLog = ls.GetWholeLog();
				Assert.That(wholeLog, Is.Not.StringContaining("UPDATE ObjectA"));
				Assert.That(wholeLog, Is.StringContaining("UPDATE ObjectB"), "should create orphans");
			}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from ObjectB").ExecuteUpdateAsync());
					await (s.CreateQuery("delete from ObjectA").ExecuteUpdateAsync());
					await (t.CommitAsync());
				}
		}
	}
}
#endif
