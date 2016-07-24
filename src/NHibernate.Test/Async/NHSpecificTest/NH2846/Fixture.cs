#if NET_4_5
using System.Linq;
using NHibernate.Driver;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2846
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return !(factory.ConnectionProvider.Driver is OracleManagedDataClientDriver);
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					// Add a test category
					var category = new Category{Id = 1, Title = "Cat 1"};
					await (session.SaveAsync(category));
					// Add a test post
					var post = new Post{Id = 1, Title = "Post 1", Category = category};
					await (session.SaveAsync(post));
					var comment1 = new Comment{Id = 1, Title = "Comment 1", Post = post};
					var comment2 = new Comment{Id = 2, Title = "Comment 2", Post = post};
					await (session.SaveAsync(comment1));
					await (session.SaveAsync(comment2));
					await (session.SaveAsync(post));
					// Flush the changes
					await (session.FlushAsync());
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Comment"));
					await (session.DeleteAsync("from Post"));
					await (session.DeleteAsync("from Category"));
					await (tran.CommitAsync());
				}
			}
		}

		[Test]
		public void FetchOnCountWorks()
		{
			using (var session = OpenSession())
			{
				var count = session.Query<Post>().Fetch(p => p.Category).FetchMany(p => p.Comments).Count();
				Assert.AreEqual(1, count);
			}
		}
	}
}
#endif
