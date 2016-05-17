#if NET_4_5
using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using NHibernate;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2760
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ShouldBeAbleToSelectUserGroupAndOrderByUserCountAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var query =
						from ug in session.Query<UserGroup>()orderby ug.Users.Count()select ug;
					var queryResults = query.ToList();
					Assert.AreEqual(2, queryResults.Count);
					Assert.AreEqual(2, queryResults[0].Id);
					Assert.AreEqual(1, queryResults[1].Id);
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ShouldBeAbleToSelectUserGroupWhereUserCountAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var query =
						from ug in session.Query<UserGroup>()where ug.Users.Count() > 1
						select ug;
					var queryResults = query.ToList();
					Assert.AreEqual(1, queryResults.Count);
					Assert.AreEqual(1, queryResults[0].Id);
					Assert.AreEqual(2, queryResults[0].Users.Count());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ShouldBeAbleToSelectUserGroupAndSelectUserIdUserCountAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var query =
						from ug in session.Query<UserGroup>()select new
						{
						id = ug.Id, count = ug.Users.Count(), }

					;
					var queryResults = query.ToList();
					Assert.AreEqual(2, queryResults.Count);
					Assert.AreEqual(1, queryResults[0].id);
					Assert.AreEqual(2, queryResults[0].count);
					Assert.AreEqual(2, queryResults[1].id);
					Assert.AreEqual(1, queryResults[1].count);
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ShouldBeAbleToSelectUserGroupAndOrderByUserCountWithHqlAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var query = session.CreateQuery("select ug from UserGroup ug order by size(ug.Users)");
					var queryResults = query.List<UserGroup>();
					Assert.AreEqual(2, queryResults.Count);
					Assert.AreEqual(2, queryResults[0].Id);
					Assert.AreEqual(1, queryResults[1].Id);
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
