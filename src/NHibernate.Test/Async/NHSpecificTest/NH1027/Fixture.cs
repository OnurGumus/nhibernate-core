#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1027
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanMakeCriteriaQueryAcrossBothAssociationsAsync()
		{
			AssertDialect();
			using (ISession s = OpenSession())
			{
				ICriteria criteria = s.CreateCriteria(typeof (Item));
				criteria.CreateCriteria("Ships", "s", JoinType.InnerJoin).Add(Expression.Eq("s.Id", 15));
				criteria.CreateCriteria("Containers", "c", JoinType.LeftOuterJoin).Add(Expression.Eq("c.Id", 15));
				criteria.SetMaxResults(2);
				await (criteria.ListAsync());
			}
		}
	}
}
#endif
