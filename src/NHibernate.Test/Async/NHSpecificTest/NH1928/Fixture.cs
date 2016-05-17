#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1928
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SqlCommentAtBeginningOfLineAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var query = session.CreateSQLQuery(@"
select 1
from 
    Customer 
where
-- this is a comment
    Name = 'Joe'
    and Age > 50
");
					Assert.DoesNotThrow(() => query.List());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task SqlCommentAtBeginningOfLastLineAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var query = session.CreateSQLQuery(@"
select 1
from 
    Customer 
where
    Name = 'Joe'
    and Age > 50
-- this is a comment");
					Assert.DoesNotThrow(() => query.List());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task SqlCommentAfterBeginningOfLineAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var query = session.CreateSQLQuery(@"
select 1
from 
    Customer 
where
 -- this is a comment
    Name = 'Joe'
    and Age > 50
");
					Assert.DoesNotThrow(() => query.List());
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
