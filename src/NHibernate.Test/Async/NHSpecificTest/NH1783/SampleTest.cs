#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1783
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task DatePropertyShouldBeStoredWithoutTimePartAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var entity = new DomainClass{Id = 1, BirthDate = new DateTime(1950, 2, 13, 3, 12, 10)};
					await (session.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
			{
				// upload the result using DateTime type to verify it does not have the time-part.
				var l = await (session.CreateSQLQuery("SELECT BirthDate AS bd FROM DomainClass").AddScalar("bd", NHibernateUtil.DateTime).ListAsync());
				var actual = (DateTime)l[0];
				var expected = new DateTime(1950, 2, 13);
				Assert.That(actual, Is.EqualTo(expected));
			}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from DomainClass").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
