#if NET_4_5
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2214
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect || dialect is MsSql2008Dialect;
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.SaveAsync(new DomainClass{Id = 1, Name = "Name"}));
				await (session.SaveAsync(new DomainClass{Id = 2, Name = "Name"}));
				await (session.SaveAsync(new DomainClass{Id = 3, Name = "Name1"}));
				await (session.SaveAsync(new DomainClass{Id = 4, Name = "Name1"}));
				await (session.SaveAsync(new DomainClass{Id = 5, Name = "Name2"}));
				await (session.SaveAsync(new DomainClass{Id = 6, Name = "Name2"}));
				await (session.SaveAsync(new DomainClass{Id = 7, Name = "Name3"}));
				await (session.SaveAsync(new DomainClass{Id = 8, Name = "Name3"}));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from DomainClass"));
				await (session.FlushAsync());
			}
		}

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
		public async Task PagedQueryWithDistinctAndOrderingByNonProjectedColumnAsync()
		{
			using (ISession session = OpenSession())
			{
				const int page = 2;
				const int rows = 2;
				var criteria = DetachedCriteria.For<DomainClass>("d").SetProjection(Projections.Distinct(Projections.ProjectionList().Add(Projections.Property("Name")))).SetFirstResult((page - 1) * rows).SetMaxResults(rows).AddOrder(Order.Asc("Id"));
				var query = criteria.GetExecutableCriteria(session);
				Assert.ThrowsAsync<HibernateException>(async () => await (query.ListAsync()));
			}
		}
	}
}
#endif
