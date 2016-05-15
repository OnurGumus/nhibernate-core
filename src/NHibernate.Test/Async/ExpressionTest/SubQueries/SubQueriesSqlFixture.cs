#if NET_4_5
using System;
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest.SubQueries
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SubQueriesSqlFixture : TestCase
	{
		[Test]
		public async Task CanQueryBlogByItsPostsAsync()
		{
			DetachedCriteria dc = DetachedCriteria.For(typeof (Post), "posts").SetProjection(Property.ForName("id")).Add(Expression.Eq("id", post1.PostId)).Add(Property.ForName("posts.Blog.id").EqProperty("blog.id"));
			using (ISession s = sessions.OpenSession())
			{
				IList list = await (s.CreateCriteria(typeof (Blog), "blog").Add(Subqueries.Exists(dc)).ListAsync());
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task ComplexSubQuery_QueryingByGrandChildrenAsync()
		{
			DetachedCriteria comment = DetachedCriteria.For(typeof (Comment), "comment").SetProjection(Property.ForName("id")).Add(Property.ForName("Post.id").EqProperty("post.id")).Add(Expression.Eq("Text", "foo"));
			using (ISession s = OpenSession())
			{
				DetachedCriteria dc = DetachedCriteria.For(typeof (Blog)).CreateCriteria("Posts", "post").Add(Subqueries.Exists(comment));
				IList list = await (dc.GetExecutableCriteria(s).ListAsync());
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
