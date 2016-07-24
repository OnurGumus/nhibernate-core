#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3428
{
	using NHibernate.Criterion;

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Dialect.MsSql2005Dialect;
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally"};
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

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
