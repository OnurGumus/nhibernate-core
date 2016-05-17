#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH892
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SelectWithWhereClauseAsync()
		{
			using (session = OpenSession())
			{
				User user1 = new User();
				user1.UserName = "User1";
				await (session.SaveAsync(user1));
				User user2 = new User();
				user2.UserName = "User2";
				await (session.SaveAsync(user2));
				BlogPost post = new BlogPost();
				post.Title = "Post 1";
				post.Poster = user1;
				await (session.SaveAsync(post));
				await (session.FlushAsync());
				session.Clear();
				User poster = (User)session.Get(typeof (User), user1.ID);
				string hql = "from BlogPost b where b.Poster = :poster";
				IList list = session.CreateQuery(hql).SetParameter("poster", poster).List();
				Assert.AreEqual(1, list.Count);
				BlogPost retrievedPost = (BlogPost)list[0];
				Assert.AreEqual(post.ID, retrievedPost.ID);
				Assert.AreEqual(user1.ID, retrievedPost.Poster.ID);
				await (session.DeleteAsync("from BlogPost"));
				await (session.DeleteAsync("from User"));
				await (session.FlushAsync());
				session.Close();
			}
		}
	}
}
#endif
