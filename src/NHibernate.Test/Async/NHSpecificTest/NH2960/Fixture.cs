#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2960
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "F100"};
					await (session.SaveAsync("FooCode", e1));
					var e2 = new Entity{Name = "B100"};
					await (session.SaveAsync("BarCode", e2));
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
		public async Task QueryWithExplicitEntityNameOnlyReturnsEntitiesOfSameTypeWithMatchingEntityNameAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.CreateQuery("from BarCode").ListAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task CriteriaQueryWithExplicitEntityNameOnlyReturnsEntitiesOfSameTypeWithMatchingEntityNameAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.CreateCriteria("BarCode").ListAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task QueryWithImplicitEntityNameReturnsAllEntitiesOfSameTypeAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.CreateQuery("from " + typeof (Entity).FullName).ListAsync());
					Assert.AreEqual(2, result.Count);
				}
		}

		[Test]
		public async Task CriteriaQueryWithImplicitEntityNameReturnsAllEntitiesOfSameTypeAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.CreateCriteria(typeof (Entity)).ListAsync());
					Assert.AreEqual(2, result.Count);
				}
		}
	}
}
#endif
