#if NET_4_5
using NUnit.Framework;
using NHibernate.Criterion;
using NHibernate.Dialect;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3567
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH3567Tests : BugTestCase
	{
		[Test]
		public async Task TestFlushModeAutoAsync()
		{
			using (ISession session = this.OpenSession())
			{
				session.FlushMode = FlushMode.Auto;
				using (var transaction = session.BeginTransaction())
				{
					var post = await (session.QueryOver<Post>().Where(x => x.Content == "Post 1").SingleOrDefaultAsync());
					post.Content = "1";
					var comments = await (session.QueryOver<Comment>().JoinQueryOver(x => x.Post).Where(x => x.Content == "1").ListAsync());
					Assert.That(comments.Count, Is.EqualTo(2), "Query over returned something different than 2");
					post.Content = "I";
					var subquery = DetachedCriteria.For(typeof (Post)).Add(Restrictions.Eq("Content", "I")).SetProjection(Projections.Id());
					var numberOfComments = (await (session.CreateCriteria(typeof (Comment)).Add(Subqueries.PropertyIn("Post.Id", subquery)).ListAsync())).Count;
					Assert.That(numberOfComments, Is.EqualTo(2), "Query with sub-query returned an invalid number of rows.");
					var site = await (session.GetAsync<Site>(1));
					site.Name = "Site 3";
					subquery = DetachedCriteria.For(typeof (Post)).SetProjection(Projections.Id()).CreateCriteria("Site").Add(Restrictions.Eq("Name", "Site 3"));
					numberOfComments = (await (session.CreateCriteria(typeof (Comment)).Add(Subqueries.PropertyIn("Post.Id", subquery)).ListAsync())).Count;
					Assert.That(numberOfComments, Is.EqualTo(2), "Query with sub-query returned an invalid number of rows.");
					transaction.Rollback();
				}
			}
		}
	}
}
#endif
