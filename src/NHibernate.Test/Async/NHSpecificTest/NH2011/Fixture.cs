#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2011
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(new Country{CountryCode = "SE"}));
					await (tx.CommitAsync());
				}
			}

			var newOrder = new Order();
			newOrder.GroupComponent = new GroupComponent();
			newOrder.GroupComponent.Countries = new List<Country>();
			newOrder.GroupComponent.Countries.Add(new Country{CountryCode = "SE"});
			Order mergedCopy;
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					mergedCopy = (Order)session.Merge(newOrder);
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				var order = await (session.GetAsync<Order>(mergedCopy.Id));
				Assert.That(order.GroupComponent.Countries.Count, Is.EqualTo(1));
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Order"));
					await (session.DeleteAsync("from Country"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
