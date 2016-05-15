#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Proxy;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ReadOnly
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ReadOnlyVersionedNodes : AbstractReadOnlyTest
	{
		[Test]
		public async Task SetReadOnlyTrueAndFalseAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode node = new VersionedNode("node", "node");
			await (s.PersistAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			await (s.SetReadOnlyAsync(node, true));
			node.Name = "node-name";
			await (s.Transaction.CommitAsync());
			AssertUpdateCount(0);
			AssertInsertCount(0);
			// the changed name is still in node
			Assert.That(node.Name, Is.EqualTo("node-name"));
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			// the changed name is still in the session
			Assert.That(node.Name, Is.EqualTo("node-name"));
			await (s.RefreshAsync(node));
			// after refresh, the name reverts to the original value
			Assert.That(node.Name, Is.EqualTo("node"));
			node = await (s.GetAsync<VersionedNode>(node.Id));
			Assert.That(node.Name, Is.EqualTo("node"));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			Assert.That(node.Name, Is.EqualTo("node"));
			await (s.SetReadOnlyAsync(node, true));
			node.Name = "diff-node-name";
			await (s.FlushAsync());
			Assert.That(node.Name, Is.EqualTo("diff-node-name"));
			await (s.RefreshAsync(node));
			Assert.That(node.Name, Is.EqualTo("node"));
			await (s.SetReadOnlyAsync(node, false));
			node.Name = "diff-node-name";
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(0);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			Assert.That(node.Name, Is.EqualTo("diff-node-name"));
			Assert.That(node.Version, Is.EqualTo(2));
			await (s.SetReadOnlyAsync(node, true));
			await (s.DeleteAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public async Task UpdateSetReadOnlyTwiceAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode node = new VersionedNode("node", "node");
			await (s.PersistAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			node.Name = "node-name";
			await (s.SetReadOnlyAsync(node, true));
			await (s.SetReadOnlyAsync(node, true));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			Assert.That(node.Name, Is.EqualTo("node"));
			Assert.That(node.Version, Is.EqualTo(1));
			await (s.SetReadOnlyAsync(node, true));
			await (s.DeleteAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public async Task UpdateSetModifiableAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode node = new VersionedNode("node", "node");
			await (s.PersistAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			node.Name = "node-name";
			await (s.SetReadOnlyAsync(node, false));
			await (s.Transaction.CommitAsync());
			s.Close();
			s.Dispose();
			AssertUpdateCount(1);
			AssertInsertCount(0);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			Assert.That(node.Name, Is.EqualTo("node-name"));
			Assert.That(node.Version, Is.EqualTo(2));
			//s.SetReadOnly(node, true);
			await (s.DeleteAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		[Ignore("Failure expected")]
		public async Task UpdateSetReadOnlySetModifiableFailureExpectedAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode node = new VersionedNode("node", "node");
			await (s.PersistAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			node.Name = "node-name";
			await (s.SetReadOnlyAsync(node, true));
			await (s.SetReadOnlyAsync(node, false));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(0);
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			Assert.That(node.Name, Is.EqualTo("node-name"));
			Assert.That(node.Version, Is.EqualTo(2));
			await (s.DeleteAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		[Ignore("Failure expected")]
		public async Task SetReadOnlyUpdateSetModifiableFailureExpectedAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode node = new VersionedNode("node", "node");
			await (s.PersistAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			await (s.SetReadOnlyAsync(node, true));
			node.Name = "node-name";
			await (s.SetReadOnlyAsync(node, false));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(0);
			s = OpenSession();
			s.BeginTransaction();
			node = await (s.GetAsync<VersionedNode>(node.Id));
			Assert.That(node.Name, Is.EqualTo("node-name"));
			Assert.That(node.Version, Is.EqualTo(2));
			await (s.DeleteAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task AddNewChildToReadOnlyParentAsync()
		{
			ISession s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			VersionedNode parent = new VersionedNode("parent", "parent");
			await (s.PersistAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			VersionedNode parentManaged = await (s.GetAsync<VersionedNode>(parent.Id));
			await (s.SetReadOnlyAsync(parentManaged, true));
			parentManaged.Name = "new parent name";
			VersionedNode child = new VersionedNode("child", "child");
			parentManaged.AddChild(child);
			await (s.SaveAsync(parentManaged));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(1);
			s = OpenSession();
			s.CacheMode = CacheMode.Ignore;
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			Assert.That(parent.Name, Is.EqualTo("parent"));
			Assert.That(parent.Children.Count, Is.EqualTo(1));
			Assert.That(parent.Version, Is.EqualTo(2));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(child, Is.Not.Null);
			await (s.DeleteAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task UpdateParentWithNewChildCommitWithReadOnlyParentAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode parent = new VersionedNode("parent", "parent");
			await (s.PersistAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			parent.Name = "new parent name";
			VersionedNode child = new VersionedNode("child", "child");
			parent.AddChild(child);
			s = OpenSession();
			s.BeginTransaction();
			await (s.UpdateAsync(parent));
			await (s.SetReadOnlyAsync(parent, true));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(1);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(parent.Name, Is.EqualTo("parent"));
			Assert.That(parent.Children.Count, Is.EqualTo(1));
			Assert.That(parent.Version, Is.EqualTo(2));
			Assert.That(child.Parent, Is.SameAs(parent));
			Assert.That(parent.Children.First(), Is.SameAs(child));
			Assert.That(child.Version, Is.EqualTo(1));
			await (s.SetReadOnlyAsync(parent, true));
			await (s.SetReadOnlyAsync(child, true));
			await (s.DeleteAsync(parent));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public async Task MergeDetachedParentWithNewChildCommitWithReadOnlyParentAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode parent = new VersionedNode("parent", "parent");
			await (s.PersistAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			parent.Name = "new parent name";
			VersionedNode child = new VersionedNode("child", "child");
			parent.AddChild(child);
			s = OpenSession();
			s.BeginTransaction();
			parent = (VersionedNode)await (s.MergeAsync(parent));
			await (s.SetReadOnlyAsync(parent, true));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(1);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(parent.Name, Is.EqualTo("parent"));
			Assert.That(parent.Children.Count, Is.EqualTo(1));
			Assert.That(parent.Version, Is.EqualTo(2));
			Assert.That(child.Parent, Is.SameAs(parent));
			Assert.That(parent.Children.First(), Is.SameAs(child));
			Assert.That(child.Version, Is.EqualTo(1));
			await (s.SetReadOnlyAsync(parent, true));
			await (s.SetReadOnlyAsync(child, true));
			await (s.DeleteAsync(parent));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public async Task GetParentMakeReadOnlyThenMergeDetachedParentWithNewChildCAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode parent = new VersionedNode("parent", "parent");
			await (s.PersistAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			parent.Name = "new parent name";
			VersionedNode child = new VersionedNode("child", "child");
			parent.AddChild(child);
			s = OpenSession();
			s.BeginTransaction();
			VersionedNode parentManaged = await (s.GetAsync<VersionedNode>(parent.Id));
			await (s.SetReadOnlyAsync(parentManaged, true));
			VersionedNode parentMerged = (VersionedNode)await (s.MergeAsync(parent));
			Assert.That(parentManaged, Is.SameAs(parentMerged));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(1);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(parent.Name, Is.EqualTo("parent"));
			Assert.That(parent.Children.Count, Is.EqualTo(1));
			Assert.That(parent.Version, Is.EqualTo(2));
			Assert.That(child.Parent, Is.SameAs(parent));
			Assert.That(parent.Children.First(), Is.SameAs(child));
			Assert.That(child.Version, Is.EqualTo(1));
			await (s.DeleteAsync(parent));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public async Task MergeUnchangedDetachedParentChildrenAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode parent = new VersionedNode("parent", "parent");
			VersionedNode child = new VersionedNode("child", "child");
			parent.AddChild(child);
			await (s.PersistAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = (VersionedNode)await (s.MergeAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			VersionedNode parentGet = await (s.GetAsync<VersionedNode>(parent.Id));
			await (s.MergeAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			VersionedNode parentLoad = await (s.LoadAsync<VersionedNode>(parent.Id));
			await (s.MergeAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(parent.Name, Is.EqualTo("parent"));
			Assert.That(parent.Children.Count, Is.EqualTo(1));
			Assert.That(parent.Version, Is.EqualTo(1));
			Assert.That(child.Parent, Is.SameAs(parent));
			Assert.That(parent.Children.First(), Is.SameAs(child));
			Assert.That(child.Version, Is.EqualTo(1));
			await (s.DeleteAsync(parent));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public async Task AddNewParentToReadOnlyChildAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode child = new VersionedNode("child", "child");
			await (s.PersistAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			VersionedNode childManaged = await (s.GetAsync<VersionedNode>(child.Id));
			await (s.SetReadOnlyAsync(childManaged, true));
			childManaged.Name = "new child name";
			VersionedNode parent = new VersionedNode("parent", "parent");
			parent.AddChild(childManaged);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(1);
			s = OpenSession();
			s.BeginTransaction();
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(child.Name, Is.EqualTo("child"));
			Assert.That(child.Parent, Is.Null);
			Assert.That(child.Version, Is.EqualTo(1));
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			Assert.That(parent, Is.Not.Null);
			await (s.SetReadOnlyAsync(child, true));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(1);
		}

		[Test]
		public async Task UpdateChildWithNewParentCommitWithReadOnlyChildAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode child = new VersionedNode("child", "child");
			await (s.PersistAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			child.Name = "new child name";
			VersionedNode parent = new VersionedNode("parent", "parent");
			parent.AddChild(child);
			s = OpenSession();
			s.BeginTransaction();
			await (s.UpdateAsync(child));
			await (s.SetReadOnlyAsync(child, true));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(1);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(child.Name, Is.EqualTo("child"));
			Assert.That(child.Parent, Is.Null);
			Assert.That(child.Version, Is.EqualTo(1));
			Assert.That(parent, Is.Not.Null);
			Assert.That(parent.Children.Count, Is.EqualTo(0));
			Assert.That(parent.Version, Is.EqualTo(1));
			await (s.SetReadOnlyAsync(parent, true));
			await (s.SetReadOnlyAsync(child, true));
			await (s.DeleteAsync(parent));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public async Task MergeDetachedChildWithNewParentCommitWithReadOnlyChildAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode child = new VersionedNode("child", "child");
			await (s.PersistAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			child.Name = "new child name";
			VersionedNode parent = new VersionedNode("parent", "parent");
			parent.AddChild(child);
			s = OpenSession();
			s.BeginTransaction();
			child = (VersionedNode)await (s.MergeAsync(child));
			await (s.SetReadOnlyAsync(child, true));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0); // NH-specific: Hibernate issues a separate UPDATE for the version number
			AssertInsertCount(1);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(child.Name, Is.EqualTo("child"));
			Assert.That(child.Parent, Is.Null);
			Assert.That(child.Version, Is.EqualTo(1));
			Assert.That(parent, Is.Not.Null);
			Assert.That(parent.Children.Count, Is.EqualTo(0));
			Assert.That(parent.Version, Is.EqualTo(1));
			await (s.SetReadOnlyAsync(parent, true));
			await (s.SetReadOnlyAsync(child, true));
			await (s.DeleteAsync(parent));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}

		[Test]
		public async Task GetChildMakeReadOnlyThenMergeDetachedChildWithNewParentAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			VersionedNode child = new VersionedNode("child", "child");
			await (s.PersistAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			child.Name = "new child name";
			VersionedNode parent = new VersionedNode("parent", "parent");
			parent.AddChild(child);
			s = OpenSession();
			s.BeginTransaction();
			VersionedNode childManaged = await (s.GetAsync<VersionedNode>(child.Id));
			await (s.SetReadOnlyAsync(childManaged, true));
			VersionedNode childMerged = (VersionedNode)await (s.MergeAsync(child));
			Assert.That(childManaged, Is.SameAs(childMerged));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0); // NH-specific: Hibernate issues a separate UPDATE for the version number
			AssertInsertCount(1);
			ClearCounts();
			s = OpenSession();
			s.BeginTransaction();
			parent = await (s.GetAsync<VersionedNode>(parent.Id));
			child = await (s.GetAsync<VersionedNode>(child.Id));
			Assert.That(child.Name, Is.EqualTo("child"));
			Assert.That(child.Parent, Is.Null);
			Assert.That(child.Version, Is.EqualTo(1));
			Assert.That(parent, Is.Not.Null);
			Assert.That(parent.Children.Count, Is.EqualTo(0));
			Assert.That(parent.Version, Is.EqualTo(1)); // NH-specific: Hibernate incorrectly increments version number, NH does not
			await (s.SetReadOnlyAsync(parent, true));
			await (s.SetReadOnlyAsync(child, true));
			await (s.DeleteAsync(parent));
			await (s.DeleteAsync(child));
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertDeleteCount(2);
		}
	}
}
#endif
