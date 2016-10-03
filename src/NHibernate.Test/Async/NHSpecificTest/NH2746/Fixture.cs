#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2746
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestQueryAsync()
		{
			using (ISession session = OpenSession())
			{
				DetachedCriteria page = DetachedCriteria.For<T1>().SetFirstResult(3).SetMaxResults(7).AddOrder(NHibernate.Criterion.Order.Asc(Projections.Id())).SetProjection(Projections.Id());
				ICriteria crit = session.CreateCriteria<T1>().Add(Subqueries.PropertyIn("id", page)).SetResultTransformer(new DistinctRootEntityResultTransformer()).SetFetchMode("Children", NHibernate.FetchMode.Join);
				session.EnableFilter("nameFilter").SetParameter("name", "Another child");
				Assert.That(async () => await (crit.ListAsync<T1>()), Throws.Nothing);
			}
		}
	}
}
#endif
