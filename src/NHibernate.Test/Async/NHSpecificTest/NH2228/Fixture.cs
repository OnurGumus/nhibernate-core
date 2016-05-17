#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Cfg.Loquacious;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2228
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task WhenStaleObjectStateThenMessageContainsEntityAsync()
		{
			using (var scenario = new ParentWithTwoChildrenScenario(Sfi))
			{
				using (var client1 = OpenSession())
				{
					var parentFromClient1 = await (client1.GetAsync<Parent>(scenario.ParentId));
					NHibernateUtil.Initialize(parentFromClient1.Children);
					var firstChildId = parentFromClient1.Children[0].Id;
					await (DeleteChildUsingAnotherSessionAsync(firstChildId));
					using (var tx1 = client1.BeginTransaction())
					{
						parentFromClient1.Children[0].Description = "Modified info";
						var expectedException = Assert.Throws<StaleObjectStateException>(async () => await (tx1.CommitAsync()));
						Assert.That(expectedException.EntityName, Is.EqualTo(typeof (Child).FullName));
						Assert.That(expectedException.Identifier, Is.EqualTo(firstChildId));
					}
				}
			}
		}

		private async Task DeleteChildUsingAnotherSessionAsync(int childIdToDelete)
		{
			using (var client2 = Sfi.OpenStatelessSession())
				using (var tx2 = client2.BeginTransaction())
				{
					client2.CreateQuery("delete from Child c where c.Id = :pChildId").SetInt32("pChildId", childIdToDelete).ExecuteUpdate();
					await (tx2.CommitAsync());
				}
		}
	}
}
#endif
