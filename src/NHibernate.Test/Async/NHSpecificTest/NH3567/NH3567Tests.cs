#if NET_4_5
using NUnit.Framework;
using NHibernate.Criterion;
using NHibernate.Dialect;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3567
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH3567TestsAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = this.OpenSession())
			{
				session.BeginTransaction();
				var id = 0;
				var site1 = new Site{Id = ++id, Name = "Site 1"};
				var site2 = new Site{Id = ++id, Name = "Site 1"};
				await (session.SaveAsync(site1));
				await (session.SaveAsync(site2));
				var p1 = new Post{Id = ++id, Content = "Post 1", Site = site1};
				var p2 = new Post{Id = ++id, Content = "Post 2", Site = site2};
				await (session.SaveAsync(p1));
				await (session.SaveAsync(p2));
				await (session.SaveAsync(new Comment{Id = ++id, Content = "Comment 1.1", Post = p1}));
				await (session.SaveAsync(new Comment{Id = ++id, Content = "Comment 1.2", Post = p1}));
				await (session.SaveAsync(new Comment{Id = ++id, Content = "Comment 2.1", Post = p2}));
				await (session.SaveAsync(new Comment{Id = ++id, Content = "Comment 2.2", Post = p2}));
				await (session.FlushAsync());
				await (session.Transaction.CommitAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = this.OpenSession())
			{
				await (session.DeleteAsync("from Comment"));
				await (session.DeleteAsync("from Post"));
				await (session.DeleteAsync("from Site"));
				await (session.FlushAsync());
			}
		}

		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect as MsSql2005Dialect != null;
		}

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
