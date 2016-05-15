#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1688
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task UsingExpressionAsync()
		{
			await (TestActionAsync(criteria => criteria.Add(Restrictions.Eq("alias.BooleanData", true))));
		}

		[Test]
		public async Task UsingExpressionProjectionAsync()
		{
			await (TestActionAsync(criteria => criteria.Add(Restrictions.Eq(Projections.Property("alias.BooleanData"), true))));
		}

		[Test]
		public async Task UsingExpressionFunctionProjectionAsync()
		{
			await (TestActionAsync(criteria => criteria.Add(Restrictions.Eq(Projections.Conditional(Restrictions.Eq(Projections.Property("alias.BooleanData"), true), Projections.Property("alias.BooleanData"), Projections.Constant(false)), false))));
		}

		public async Task TestActionAsync(System.Action<DetachedCriteria> action)
		{
			using (ISession session = OpenSession())
			{
				DetachedCriteria criteria = DetachedCriteria.For<NH1679.DomainClass>("alias");
				action.Invoke(criteria);
				IList l = await (criteria.GetExecutableCriteria(session).ListAsync());
				Assert.AreNotEqual(l, null);
			}
		}
	}
}
#endif
