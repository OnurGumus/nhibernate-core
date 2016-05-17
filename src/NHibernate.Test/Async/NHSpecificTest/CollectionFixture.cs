#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionFixture : TestCase
	{
		[Test]
		public async Task TestLoadParentFirstAsync()
		{
			int parentId = 0;
			using (ISession s1 = OpenSession())
				using (ITransaction t1 = s1.BeginTransaction())
				{
					// create a new
					LLParent parent = new LLParent();
					LLChildNoAdd child = new LLChildNoAdd();
					parent.ChildrenNoAdd.Add(child);
					child.Parent = parent;
					await (s1.SaveAsync(parent));
					parentId = (int)s1.GetIdentifier(parent);
					await (t1.CommitAsync());
				}

			// try to Load the object to make sure the save worked
			using (ISession s2 = OpenSession())
				using (ITransaction t2 = s2.BeginTransaction())
				{
					LLParent parent2 = (LLParent)s2.Load(typeof (LLParent), parentId);
					Assert.AreEqual(1, parent2.ChildrenNoAdd.Count);
				}
		}

		[Test]
		public async Task TestLoadChildFirstAsync()
		{
			int parentId = 0;
			int childId = 0;
			using (ISession s1 = OpenSession())
				using (ITransaction t1 = s1.BeginTransaction())
				{
					// create a new
					LLParent parent = new LLParent();
					LLChildNoAdd child = new LLChildNoAdd();
					parent.ChildrenNoAdd.Add(child);
					child.Parent = parent;
					await (s1.SaveAsync(parent));
					parentId = (int)s1.GetIdentifier(parent);
					childId = (int)s1.GetIdentifier(child);
					await (t1.CommitAsync());
				}

			// try to Load the object to make sure the save worked
			using (ISession s2 = OpenSession())
				using (ITransaction t2 = s2.BeginTransaction())
				{
					LLChildNoAdd child2 = (LLChildNoAdd)s2.Load(typeof (LLChildNoAdd), childId);
					Assert.AreEqual(parentId, (int)s2.GetIdentifier(child2.Parent));
				}
		}
	}
}
#endif
