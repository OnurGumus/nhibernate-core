#if NET_4_5
using NHibernate.Test.Events.Collections.Association.Bidirectional.ManyToMany;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Events.Collections.Association
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractAssociationCollectionEventFixture : AbstractCollectionEventFixture
	{
		[Test]
		public async Task DeleteParentButNotChildAsync()
		{
			CollectionListeners listeners = new CollectionListeners(sessions);
			IParentWithCollection parent = await (CreateParentWithOneChildAsync("parent", "child"));
			ChildEntity child = (ChildEntity)GetFirstChild(parent.Children);
			listeners.Clear();
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			parent = (IParentWithCollection)await (s.GetAsync(parent.GetType(), parent.Id));
			child = (ChildEntity)await (s.GetAsync(child.GetType(), child.Id));
			parent.RemoveChild(child);
			await (s.DeleteAsync(parent));
			await (tx.CommitAsync());
			s.Close();
			int index = 0;
			CheckResult(listeners, listeners.InitializeCollection, parent, index++);
			if (child is ChildWithBidirectionalManyToMany)
			{
				CheckResult(listeners, listeners.InitializeCollection, (ChildWithBidirectionalManyToMany)child, index++);
			}

			CheckResult(listeners, listeners.PreCollectionRemove, parent, index++);
			CheckResult(listeners, listeners.PostCollectionRemove, parent, index++);
			if (child is ChildWithBidirectionalManyToMany)
			{
				CheckResult(listeners, listeners.PreCollectionUpdate, (ChildWithBidirectionalManyToMany)child, index++);
				CheckResult(listeners, listeners.PostCollectionUpdate, (ChildWithBidirectionalManyToMany)child, index++);
			}

			CheckNumberOfResults(listeners, index);
		}
	}
}
#endif
