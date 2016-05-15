#if NET_4_5
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2214
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task PagedQueryWithDistinctAsync()
		{
			using (ISession session = OpenSession())
			{
				const int page = 2;
				const int rows = 2;
				var criteria = DetachedCriteria.For<DomainClass>("d").SetProjection(Projections.Distinct(Projections.ProjectionList().Add(Projections.Property("Name")))).SetFirstResult((page - 1) * rows).SetMaxResults(rows).AddOrder(Order.Asc("Name"));
				var query = criteria.GetExecutableCriteria(session);
				var result = await (query.ListAsync());
				Assert.AreEqual("Name2", result[0]);
				Assert.AreEqual("Name3", result[1]);
			}
		}

		[Test]
		public Task PagedQueryWithDistinctAndOrderingByNonProjectedColumnAsync()
		{
			try
			{
				PagedQueryWithDistinctAndOrderingByNonProjectedColumn();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
