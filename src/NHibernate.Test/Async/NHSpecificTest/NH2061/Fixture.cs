#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2061
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task merge_with_many_to_many_inside_component_that_is_nullAsync()
		{
			// Order with null GroupComponent
			Order newOrder = new Order();
			newOrder.GroupComponent = null;
			Order mergedCopy = null;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					mergedCopy = (Order)session.Merge(newOrder);
					await (tx.CommitAsync());
				}

			Assert.That(mergedCopy, Is.Not.Null);
			Assert.That(mergedCopy.GroupComponent, Is.Null);
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Order"));
					await (session.DeleteAsync("from Country"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}
	}
}
#endif
