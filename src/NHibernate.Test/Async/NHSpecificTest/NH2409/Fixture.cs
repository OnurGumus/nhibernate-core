#if NET_4_5
using System;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2409
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BugAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var contest1 = new Contest{Id = 1};
					var contest2 = new Contest{Id = 2};
					var user = new User();
					var message = new Message{Contest = contest2};
					await (session.SaveAsync(contest1));
					await (session.SaveAsync(contest2));
					await (session.SaveAsync(user));
					await (session.SaveAsync(message));
					await (tx.CommitAsync());
				}

			using (var session = OpenSession())
			{
				var contest2 = await (session.CreateCriteria<Contest>().Add(Restrictions.IdEq(2)).UniqueResultAsync<Contest>());
				var user = (await (session.CreateCriteria<User>().ListAsync<User>())).Single();
				var msgs = await (session.CreateCriteria<Message>().Add(Restrictions.Eq("Contest", contest2)).CreateAlias("Readings", "mr", JoinType.LeftOuterJoin, Restrictions.Eq("mr.User", user)).ListAsync<Message>());
				Assert.AreEqual(1, msgs.Count, "We should be able to find our message despite any left outer joins");
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Contest"));
					await (session.DeleteAsync("from User"));
					await (session.DeleteAsync("from Message"));
					await (session.DeleteAsync("from MessageReading"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}
	}
}
#endif
