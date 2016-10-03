#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2951
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
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
		[Ignore("Not working.")]
		public async Task UpdateWithSubqueryToJoinedSubclassAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var c = new Customer{Name = "Bob"};
					await (session.SaveAsync(c));
					var i = new Invoice{Amount = 10};
					await (session.SaveAsync(i));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					// Using (select c.Id ...) works.
					string hql = "update Invoice i set i.Customer = (select c from Customer c where c.Name = 'Bob')";
					int result = await (session.CreateQuery(hql).ExecuteUpdateAsync());
					Assert.AreEqual(1, result);
				}
		}
	}
}
#endif
