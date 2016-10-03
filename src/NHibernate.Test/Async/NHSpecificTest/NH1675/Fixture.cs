#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1675
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task ShouldWorkUsingDistinctAndLimitsAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					for (int i = 0; i < 5; i++)
					{
						await (s.SaveAsync(new Person{FirstName = "Name" + i}));
					}

					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				var q = s.CreateQuery("select distinct p from Person p").SetFirstResult(0).SetMaxResults(10);
				Assert.That((await (q.ListAsync())).Count, Is.EqualTo(5));
			}

			// clean up
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
