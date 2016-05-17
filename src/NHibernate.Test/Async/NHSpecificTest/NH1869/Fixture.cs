#if NET_4_5
using System.Collections;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1869
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
				var result = GetResult(session);
				Assert.That(result, Has.Count.EqualTo(2));
				Assert.That(result[0], Has.Count.EqualTo(1));
				Assert.That(result[1], Has.Count.EqualTo(1));
			}
		}
	}
}
#endif
