#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH508
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			User friend1 = new User("friend1");
			User friend2 = new User("friend2");
			User friend3 = new User("friend3");
			// create a new user with 3 friends
			User user = new User();
			user.Login = "admin";
			user.FriendList.Add(friend2);
			user.FriendList.Add(friend1);
			user.FriendList.Add(friend3);
			object userId = null;
			using (ISession session = sessions.OpenSession())
				using (ITransaction tran = session.BeginTransaction())
				{
					await (session.SaveAsync(friend1));
					await (session.SaveAsync(friend2));
					await (session.SaveAsync(friend3));
					userId = await (session.SaveAsync(user));
					await (tran.CommitAsync());
				}

			// reload the user and remove one of the 3 friends
			using (ISession session = sessions.OpenSession())
				using (ITransaction tran = session.BeginTransaction())
				{
					User reloadedFriend = (User)await (session.LoadAsync(typeof (User), friend1.UserId));
					User reloadedUser = (User)await (session.LoadAsync(typeof (User), userId));
					reloadedUser.FriendList.Remove(reloadedFriend);
					await (tran.CommitAsync());
				}

			using (ISession session = sessions.OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					User admin = (User)await (session.GetAsync(typeof (User), userId));
					Assert.IsFalse(admin.FriendList.Contains(friend1));
					Assert.IsTrue(admin.FriendList.Contains(friend2));
					Assert.IsTrue(admin.FriendList.Contains(friend3));
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from User"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
