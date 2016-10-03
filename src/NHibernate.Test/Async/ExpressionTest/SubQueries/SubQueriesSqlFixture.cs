#if NET_4_5
using System;
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest.SubQueries
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SubQueriesSqlFixtureAsync : TestCaseAsync
	{
		private Post post2;
		private Post post1;
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"ExpressionTest.SubQueries.Mappings.hbm.xml"};
			}
		}

		protected override async Task OnSetUpAsync()
		{
			// Create some objects
			using (ISession session = OpenSession())
			{
				Category category = new Category("NHibernate");
				User author = new User("Josh");
				User commenter = new User("Ayende");
				Blog blog = new Blog("bar");
				blog.Users.Add(author);
				author.Blogs.Add(blog);
				post1 = new Post("p1");
				this.post1.Blog = blog;
				this.post1.Categories.Add(category);
				post2 = new Post("p2");
				this.post2.Blog = blog;
				Comment comment = new Comment("foo");
				comment.Commenter = commenter;
				comment.Post = post1;
				comment.IndexInPost = 0;
				post1.Comments.Add(comment);
				await (session.SaveAsync(category));
				await (session.SaveAsync(author));
				await (session.SaveAsync(commenter));
				await (session.SaveAsync(blog));
				await (session.SaveAsync(this.post1));
				await (session.SaveAsync(this.post2));
				await (session.SaveAsync(comment));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.DeleteAsync("from Comment"));
				await (s.DeleteAsync("from Post"));
				await (s.DeleteAsync("from Blog"));
				await (s.DeleteAsync("from User"));
				await (s.DeleteAsync("from Category"));
				await (s.FlushAsync());
			}
		}

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
