#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2960
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
