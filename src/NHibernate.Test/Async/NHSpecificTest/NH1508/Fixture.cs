#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1508
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task DoesntThrowExceptionWhenHqlQueryIsGivenAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var sqlQuery = session.CreateQuery("from Document");
					var q = session.CreateMultiQuery().Add(sqlQuery);
					await (q.ListAsync());
				}
		}
	}
}
#endif
