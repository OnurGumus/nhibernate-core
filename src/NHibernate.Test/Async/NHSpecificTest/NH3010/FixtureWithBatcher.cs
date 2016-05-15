#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3010
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureWithBatcher : BugTestCase
	{
		// Test case from NH-2527
		[Test]
		public async Task DisposedCommandShouldNotBeReusedAfterRemoveAtAndInsertAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var parent = await (session.CreateCriteria<Parent>().UniqueResultAsync<Parent>());
					Child childOne = parent.Childs[0];
					var childTwo = new Child();
					parent.Childs.Add(childTwo);
					Child childToMove = parent.Childs[1];
					parent.Childs.RemoveAt(1);
					parent.Childs.Insert(0, childToMove);
					await (Assert.DoesNotThrow(tx.CommitAsync));
					Assert.AreEqual(childTwo.Id, parent.Childs[0].Id);
					Assert.AreEqual(childOne.Id, parent.Childs[1].Id);
				}
		}

		// Test case from NH-1477
		[Test]
		public async Task DisposedCommandShouldNotBeReusedAfterClearAndAddAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var parent = await (session.CreateCriteria<Parent>().UniqueResultAsync<Parent>());
					parent.Childs.Clear();
					var childOne = new Child();
					parent.Childs.Add(childOne);
					var childTwo = new Child();
					parent.Childs.Add(childTwo);
					await (Assert.DoesNotThrow(tx.CommitAsync));
					Assert.AreEqual(childOne.Id, parent.Childs[0].Id);
					Assert.AreEqual(childTwo.Id, parent.Childs[1].Id);
				}
		}
	}
}
#endif
