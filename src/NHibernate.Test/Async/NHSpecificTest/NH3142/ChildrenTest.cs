#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3142
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ChildrenTestAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return !(factory.ConnectionProvider.Driver is OracleManagedDataClientDriver);
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = OpenSession())
			{
				for (int h = 1; h < 3; h++)
				{
					for (int i = 1; i < 4; i++)
					{
						var parent = new DomainParent{Id1 = h, Id2 = i};
						await (session.SaveAsync(parent));
						var parentId = new DomainParentWithComponentId{Id = {Id1 = h, Id2 = i}};
						await (session.SaveAsync(parentId));
						for (int j = 1; j < 4; j++)
						{
							var child = new DomainChild{ParentId1 = h, ParentId2 = i};
							await (session.SaveAsync(child));
							var childId = new DomainChildWCId{ParentId1 = h, ParentId2 = i};
							await (session.SaveAsync(childId));
						}
					}
				}

				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
			{
				await (session.DeleteAsync("from DomainChild"));
				await (session.DeleteAsync("from DomainChildWCId"));
				await (session.DeleteAsync("from System.Object"));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task ChildrenCollectionOfAllParentsShouldContainsThreeElementsAsync()
		{
			using (var session = OpenSession())
			{
				var entities = await (session.CreateQuery("from DomainParent").ListAsync<DomainParent>());
				foreach (var parent in entities)
					Assert.AreEqual(3, parent.Children.Count);
			}
		}

		[Test]
		public async Task ChildrenCollectionOfAllParentsWithComponentIdShouldContainsThreeElementsAsync()
		{
			using (var session = OpenSession())
			{
				var entities = await (session.CreateQuery("from DomainParentWithComponentId").ListAsync<DomainParentWithComponentId>());
				foreach (var parent in entities)
					Assert.AreEqual(3, parent.Children.Count);
			}
		}
	}
}
#endif
