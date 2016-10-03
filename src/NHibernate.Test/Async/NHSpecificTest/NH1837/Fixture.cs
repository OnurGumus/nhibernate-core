#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1837
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			sessions.Statistics.IsStatisticsEnabled = true;
			using (ISession session = this.OpenSession())
				using (ITransaction tran = session.BeginTransaction())
				{
					var customer = new Customer{Name = "Fabio Maulo"};
					var order = new Order{Date = DateTime.Now, Customer = customer};
					customer.Orders.Add(order);
					await (session.SaveAsync(customer));
					await (session.SaveAsync(order));
					await (tran.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = this.OpenSession())
				using (ITransaction tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Order"));
					await (session.DeleteAsync("from Customer"));
					await (tran.CommitAsync());
				}
		}

		[Test]
		public async Task ExecutesOneQueryWithUniqueResultWithChildCriteriaNonGenericAsync()
		{
			sessions.Statistics.Clear();
			using (ISession session = this.OpenSession())
			{
				var criteria = session.CreateCriteria(typeof (Order), "o");
				await (criteria.CreateCriteria("o.Customer", "c").Add(Restrictions.Eq("c.Id", 1)).SetProjection(Projections.RowCount()).UniqueResultAsync());
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task ExecutesOneQueryWithUniqueResultWithChildCriteriaGenericAsync()
		{
			sessions.Statistics.Clear();
			using (ISession session = this.OpenSession())
			{
				await (session.CreateCriteria(typeof (Order), "o").CreateCriteria("o.Customer", "c").Add(Restrictions.Eq("c.Id", 1)).SetProjection(Projections.RowCount()).UniqueResultAsync<int>());
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task ExecutesOneQueryWithUniqueResultWithCriteriaNonGenericAsync()
		{
			sessions.Statistics.Clear();
			using (ISession session = this.OpenSession())
			{
				await (session.CreateCriteria(typeof (Order), "o").SetProjection(Projections.RowCount()).UniqueResultAsync());
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task ExecutesOneQueryWithUniqueResultWithCriteriaGenericAsync()
		{
			sessions.Statistics.Clear();
			using (ISession session = this.OpenSession())
			{
				await (session.CreateCriteria(typeof (Order), "o").SetProjection(Projections.RowCount()).UniqueResultAsync<int>());
				Assert.That(sessions.Statistics.QueryExecutionCount, Is.EqualTo(1));
			}
		}
	}
}
#endif
