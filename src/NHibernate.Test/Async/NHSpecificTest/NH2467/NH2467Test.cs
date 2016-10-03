#if NET_4_5
using System.Linq;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2467
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH2467TestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = OpenSession())
			{
				var entity = new DomainClass{Id = 1, Data = "Test"};
				await (session.SaveAsync(entity));
				await (session.FlushAsync());
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect.SupportsLimit;
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object"));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task ShouldNotThrowOnFuturePagingAsync()
		{
			using (var session = OpenSession())
			{
				var contentQuery = session.CreateCriteria<DomainClass>().Add(Restrictions.Eq("Data", "Test"));
				contentQuery.SetMaxResults(2);
				contentQuery.SetFirstResult(0);
				var content = await (contentQuery.FutureAsync<DomainClass>());
				var countQuery = session.CreateCriteria<DomainClass>().Add(Restrictions.Eq("Data", "Test"));
				countQuery.SetProjection(Projections.RowCount());
				var count = await (countQuery.FutureValueAsync<int>());
				// triggers batch operation, should not throw
				var result = content.ToList();
			}
		}

		[Test]
		public async Task ShouldNotThrowOnReversedFuturePagingAsync()
		{
			using (var session = OpenSession())
			{
				var countQuery = session.CreateCriteria<DomainClass>().Add(Restrictions.Eq("Data", "Test"));
				countQuery.SetProjection(Projections.RowCount());
				var count = await (countQuery.FutureValueAsync<int>());
				var contentQuery = session.CreateCriteria<DomainClass>().Add(Restrictions.Eq("Data", "Test"));
				contentQuery.SetMaxResults(2);
				contentQuery.SetFirstResult(0);
				var content = await (contentQuery.FutureAsync<DomainClass>());
				// triggers batch operation, should not throw
				var result = content.ToList();
			}
		}

		[Test]
		public async Task ShouldNotThrowOnFuturePagingUsingHqlAsync()
		{
			using (var session = OpenSession())
			{
				var contentQuery = session.CreateQuery("from DomainClass as d where d.Data = ?");
				contentQuery.SetString(0, "Test");
				contentQuery.SetMaxResults(2);
				contentQuery.SetFirstResult(0);
				var content = await (contentQuery.FutureAsync<DomainClass>());
				var countQuery = session.CreateQuery("select count(d) from DomainClass as d where d.Data = ?");
				countQuery.SetString(0, "Test");
				var count = await (countQuery.FutureValueAsync<long>());
				Assert.AreEqual(1, content.ToList().Count);
				Assert.AreEqual(1, count.Value);
			}
		}

		[Test]
		public async Task ShouldNotThrowOnReversedFuturePagingUsingHqlAsync()
		{
			using (var session = OpenSession())
			{
				var contentQuery = session.CreateQuery("from DomainClass as d where d.Data = ?");
				contentQuery.SetString(0, "Test");
				contentQuery.SetMaxResults(2);
				contentQuery.SetFirstResult(0);
				var content = await (contentQuery.FutureAsync<DomainClass>());
				var countQuery = session.CreateQuery("select count(d) from DomainClass as d where d.Data = ?");
				countQuery.SetString(0, "Test");
				var count = await (countQuery.FutureValueAsync<long>());
				Assert.AreEqual(1, content.ToList().Count);
				Assert.AreEqual(1, count.Value);
			}
		}
	}
}
#endif
