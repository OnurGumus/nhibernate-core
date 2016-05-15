#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3142
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ChildrenTest : BugTestCase
	{
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
