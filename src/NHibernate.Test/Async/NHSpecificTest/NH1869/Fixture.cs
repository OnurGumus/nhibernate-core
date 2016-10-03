#if NET_4_5
using System.Collections;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1869
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private Keyword _keyword;
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver.SupportsMultipleQueries;
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = sessions.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from NodeKeyword"));
					await (session.DeleteAsync("from Keyword"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task TestAsync()
		{
			using (var session = sessions.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					_keyword = new Keyword();
					await (session.SaveAsync(_keyword));
					var nodeKeyword = new NodeKeyword();
					nodeKeyword.NodeId = 1;
					nodeKeyword.Keyword = _keyword;
					await (session.SaveAsync(nodeKeyword));
					await (transaction.CommitAsync());
				}

			using (var session = sessions.OpenSession())
			{
				//If uncomment the line below the test will pass
				//GetResult(session);
				var result = await (GetResultAsync(session));
				Assert.That(result, Has.Count.EqualTo(2));
				Assert.That(result[0], Has.Count.EqualTo(1));
				Assert.That(result[1], Has.Count.EqualTo(1));
			}
		}

		private async Task<IList> GetResultAsync(ISession session)
		{
			var query1 = session.CreateQuery("from NodeKeyword nk");
			var query2 = session.CreateQuery("from NodeKeyword nk");
			var multi = session.CreateMultiQuery();
			multi.Add(query1).Add(query2);
			return await (multi.ListAsync());
		}
	}
}
#endif
