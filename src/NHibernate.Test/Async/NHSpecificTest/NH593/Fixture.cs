#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH593
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BugAsync()
		{
			using (ISession session = OpenSession())
			{
				User user = new User("test");
				user.UserId = 10;
				Assert.ThrowsAsync<QueryException>(async () => await (session.CreateCriteria(typeof (Blog)).Add(Expression.In("Users", new User[]{user})).ListAsync()));
			}
		}
	}
}
#endif
