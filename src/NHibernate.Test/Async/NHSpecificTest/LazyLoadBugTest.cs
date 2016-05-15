#if NET_4_5
using System;
using System.Collections;
using NHibernate.DomainModel.NHSpecific;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyLoadBugTest : TestCase
	{
		[Test]
		public async Task TestLazyLoadAsync()
		{
			int parentId = 0;
			using (ISession s1 = OpenSession())
				using (ITransaction t1 = s1.BeginTransaction())
				{
					// create a new
					LLParent parent = new LLParent();
					LLChild child = new LLChild();
					child.Parent = parent;
					await (s1.SaveAsync(parent));
					parentId = (int)s1.GetIdentifier(parent);
					await (t1.CommitAsync());
				}

			try
			{
				// try to Load the object to get the exception
				using (ISession s2 = OpenSession())
					using (ITransaction t2 = s2.BeginTransaction())
					{
						LLParent parent2 = (LLParent)await (s2.LoadAsync(typeof (LLParent), parentId));
						// this should throw the exception - the property setter access is not mapped correctly.
						// Because it maintains logic to maintain the collection during the property set it should
						// tell NHibernate to skip the setter and access the field.  If it doesn't, then throw
						// a LazyInitializationException.
						Assert.Throws<LazyInitializationException>(() =>
						{
							int count = parent2.Children.Count;
						}

						);
					}
			}
			finally
			{
				await (ExecuteStatementAsync("delete from LLChild"));
				await (ExecuteStatementAsync("delete from LLParent"));
			}
		}

		[Test]
		public async Task TestLazyLoadNoAddAsync()
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
					LLParent parent2 = (LLParent)await (s2.LoadAsync(typeof (LLParent), parentId));
					Assert.AreEqual(1, parent2.ChildrenNoAdd.Count);
				}

			using (ISession session = sessions.OpenSession())
			{
				await (session.DeleteAsync("from LLParent"));
				await (session.FlushAsync());
			}
		}
	}
}
#endif
