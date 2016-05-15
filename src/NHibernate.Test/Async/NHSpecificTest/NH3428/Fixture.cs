#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3428
{
	using NHibernate.Criterion;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task QueryFailsWhenDistinctOrderedResultIsPagedPastPageOneAsync()
		{
			using (ISession session = this.OpenSession())
				using (session.BeginTransaction())
				{
					var criteria = session.CreateCriteria<Entity>();
					var projectionList = Projections.ProjectionList().Add(Projections.Property("Name"), "Name");
					criteria.SetProjection(Projections.Distinct(projectionList));
					criteria.SetFirstResult(1).SetMaxResults(1);
					criteria.AddOrder(Order.Asc("Name"));
					var result = await (criteria.ListAsync());
					Assert.AreEqual(1, result.Count);
					Assert.AreEqual("Sally", result[0]);
				}
		}
	}
}
#endif
