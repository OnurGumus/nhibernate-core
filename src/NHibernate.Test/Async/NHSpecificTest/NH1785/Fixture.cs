#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1785
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task BugAsync()
		{
			try
			{
				Bug();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
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
