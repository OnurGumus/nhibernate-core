#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1324
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseUniqueResultWithNullableType_ReturnNull_CriteriaAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person("a", null, 4);
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					int ? result = await (s.CreateCriteria(typeof (Person)).SetProjection(Projections.Property("IQ")).UniqueResultAsync<int ? >());
					Assert.IsNull(result);
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseUniqueResultWithNullableType_ReturnResult_CriteriaAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person("a", 4, 4);
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					int ? result = await (s.CreateCriteria(typeof (Person)).SetProjection(Projections.Property("IQ")).UniqueResultAsync<int ? >());
					Assert.AreEqual(4, result);
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseUniqueResultWithNullableType_ReturnNull_HQLAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person("a", null, 4);
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					int ? result = s.CreateQuery("select p.IQ from Person p").UniqueResult<int ? >();
					Assert.IsNull(result);
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseUniqueResultWithNullableType_ReturnResult_HQLAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = new Person("a", 4, 4);
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					int ? result = s.CreateQuery("select p.IQ from Person p").UniqueResult<int ? >();
					Assert.AreEqual(4, result);
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
