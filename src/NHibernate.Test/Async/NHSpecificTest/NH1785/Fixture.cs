#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1785
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BugAsync()
		{
			using (var session = OpenSession())
			{
				ICriteria criteria = session.CreateCriteria(typeof (Entity1));
				criteria.CreateAlias("Entities2", "ent2", JoinType.InnerJoin);
				criteria.CreateAlias("ent2.Id.Entity3", "ent3", JoinType.InnerJoin);
				criteria.CreateAlias("ent3.Entity4", "ent4", JoinType.InnerJoin);
				criteria.Add(Restrictions.Eq("ent4.Id", Guid.NewGuid()));
				Assert.DoesNotThrowAsync(async () => await (criteria.ListAsync<Entity1>()));
			}
		}

		[Test]
		public async Task ShouldNotContainJoinWhereNotRequiredAsync()
		{
			using (var session = OpenSession())
			{
				using (var ls = new SqlLogSpy())
				{
					ICriteria criteria = session.CreateCriteria(typeof (Entity1));
					criteria.CreateAlias("Entities2", "ent2", JoinType.InnerJoin);
					await (criteria.ListAsync<Entity1>());
					var sql = ls.GetWholeLog();
					var rx = new Regex(@"\bjoin\b");
					Assert.That(rx.Matches(sql).Count, Is.EqualTo(1));
				}
			}
		}
	}
}
#endif
