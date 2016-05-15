#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2469
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ShouldNotThrowSqlExceptionAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var criteria = session.CreateCriteria(typeof (Entity2), "e2").CreateAlias("e2.Entity1", "e1").Add(Restrictions.Eq("e1.Foo", 0));
					Assert.AreEqual(0, (await (criteria.ListAsync<Entity2>())).Count);
				}
		}
	}
}
#endif
