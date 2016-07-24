#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1027
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1027";
			}
		}

		private void AssertDialect()
		{
			if (!(Dialect is MsSql2005Dialect))
				Assert.Ignore("This test is specific for MsSql2005Dialect");
		}

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
