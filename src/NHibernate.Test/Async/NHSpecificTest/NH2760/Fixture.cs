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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var group1 = new UserGroup()
					{Id = 1, Name = "User Group 1"};
					var group2 = new UserGroup()
					{Id = 2, Name = "User Group 2"};
					var user1 = new User()
					{Id = 1, Name = "User 1"};
					var user2 = new User()
					{Id = 2, Name = "User 2"};
					user1.UserGroups.Add(group1);
					user1.UserGroups.Add(group2);
					user2.UserGroups.Add(group1);
					await (session.SaveAsync(group1));
					await (session.SaveAsync(group2));
					await (session.SaveAsync(user1));
					await (session.SaveAsync(user2));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = base.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

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
					var queryResults = await (query.ListAsync<UserGroup>());
					Assert.AreEqual(2, queryResults.Count);
					Assert.AreEqual(2, queryResults[0].Id);
					Assert.AreEqual(1, queryResults[1].Id);
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
