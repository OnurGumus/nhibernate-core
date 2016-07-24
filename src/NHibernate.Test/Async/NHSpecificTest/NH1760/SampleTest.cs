#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1760
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		private async Task FillDbAsync()
		{
			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var customer = new Customer{Name = "Alkampfer"};
					await (session.SaveAsync(customer));
					var testClass = new TestClass{Id = new TestClassId{Customer = customer, SomeInt = 42}, Value = "TESTVALUE"};
					await (session.SaveAsync(testClass));
					await (tx.CommitAsync());
				}
		}

		private async Task CleanupAsync()
		{
			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from TestClass").ExecuteUpdateAsync());
					await (session.CreateQuery("delete from Customer").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task CanUseCriteriaAsync()
		{
			await (FillDbAsync());
			int hqlCount;
			int criteriaCount;
			using (ISession session = OpenSession())
			{
				IList<TestClass> retvalue = await (session.CreateQuery("Select tc from TestClass tc join tc.Id.Customer cu where cu.Name = :name").SetString("name", "Alkampfer").ListAsync<TestClass>());
				hqlCount = retvalue.Count;
			}

			using (ISession session = OpenSession())
			{
				ICriteria c = session.CreateCriteria(typeof (TestClass)).CreateAlias("Id.Customer", "IdCust").Add(Restrictions.Eq("IdCust.Name", "Alkampfer"));
				IList<TestClass> retvalue = await (c.ListAsync<TestClass>());
				criteriaCount = retvalue.Count;
			}

			Assert.That(criteriaCount, Is.EqualTo(1));
			Assert.That(criteriaCount, Is.EqualTo(hqlCount));
			await (CleanupAsync());
		}

		[Test]
		public async Task TheJoinShouldBeOptionalAsync()
		{
			await (FillDbAsync());
			int criteriaCount;
			using (ISession session = OpenSession())
			{
				using (var ls = new SqlLogSpy())
				{
					ICriteria c = session.CreateCriteria(typeof (TestClass));
					IList<TestClass> retvalue = await (c.ListAsync<TestClass>());
					Assert.That(ls.GetWholeLog(), Is.Not.StringContaining("join"));
					criteriaCount = retvalue.Count;
				}
			}

			Assert.That(criteriaCount, Is.EqualTo(1));
			await (CleanupAsync());
		}
	}
}
#endif
