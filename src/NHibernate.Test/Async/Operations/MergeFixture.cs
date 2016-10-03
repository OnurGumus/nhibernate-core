#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Operations
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MergeFixtureAsync : AbstractOperationTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is Dialect.FirebirdDialect); // Firebird has no CommandTimeout, and locks up during the tear-down of this fixture
		}

		protected override Task OnTearDownAsync()
		{
			return CleanupAsync();
		}

		private async Task CleanupAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from NumberedNode where parent is not null"));
					await (s.DeleteAsync("from NumberedNode"));
					await (s.DeleteAsync("from Node where parent is not null"));
					await (s.DeleteAsync("from Node"));
					await (s.DeleteAsync("from VersionedEntity where parent is not null"));
					await (s.DeleteAsync("from VersionedEntity"));
					await (s.DeleteAsync("from TimestampedEntity"));
					await (s.DeleteAsync("from Competitor"));
					await (s.DeleteAsync("from Competition"));
					await (s.DeleteAsync("from Employer"));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task DeleteAndMergeAsync()
		{
			using (ISession s = OpenSession())
			{
				s.BeginTransaction();
				var jboss = new Employer();
				await (s.PersistAsync(jboss));
				await (s.Transaction.CommitAsync());
				s.Clear();
				s.BeginTransaction();
				var otherJboss = await (s.GetAsync<Employer>(jboss.Id));
				await (s.DeleteAsync(otherJboss));
				await (s.Transaction.CommitAsync());
				s.Clear();
				jboss.Vers = 1;
				s.BeginTransaction();
				s.Merge(jboss);
				await (s.Transaction.CommitAsync());
			}
		}

		[Test]
		public async Task MergeBidiForeignKeyOneToOneAsync()
		{
			Person p;
			Address a;
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					p = new Person{Name = "steve"};
					a = new Address{StreetAddress = "123 Main", City = "Austin", Country = "US", Resident = p};
					await (s.PersistAsync(a));
					await (s.PersistAsync(p));
					await (tx.CommitAsync());
				}
			}

			ClearCounts();
			p.Address.StreetAddress = "321 Main";
			using (ISession s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					p = (Person)s.Merge(p);
					await (s.Transaction.CommitAsync());
				}
			}

			AssertInsertCount(0);
			AssertUpdateCount(0); // no cascade
			AssertDeleteCount(0);
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(a));
					await (s.DeleteAsync(p));
					await (tx.CommitAsync());
				}
			}
		}

		[Test, Ignore("Need some more investigation about id sync.")]
		public async Task MergeBidiPrimayKeyOneToOneAsync()
		{
			Person p;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					p = new Person{Name = "steve"};
					new PersonalDetails{SomePersonalDetail = "I have big feet", Person = p};
					await (s.PersistAsync(p));
					await (tx.CommitAsync());
				}

			ClearCounts();
			p.Details.SomePersonalDetail = p.Details.SomePersonalDetail + " and big hands too";
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					p = (Person)s.Merge(p);
					await (tx.CommitAsync());
				}

			AssertInsertCount(0);
			AssertUpdateCount(1);
			AssertDeleteCount(0);
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(p));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task MergeDeepTreeAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			var root = new Node{Name = "root"};
			var child = new Node{Name = "child"};
			var grandchild = new Node{Name = "grandchild"};
			root.AddChild(child);
			child.AddChild(grandchild);
			s.Merge(root);
			await (tx.CommitAsync());
			s.Close();
			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			grandchild.Description = "the grand child";
			var grandchild2 = new Node{Name = "grandchild2"};
			child.AddChild(grandchild2);
			s = OpenSession();
			tx = s.BeginTransaction();
			s.Merge(root);
			await (tx.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(1);
			ClearCounts();
			var child2 = new Node{Name = "child2"};
			var grandchild3 = new Node{Name = "grandchild3"};
			child2.AddChild(grandchild3);
			root.AddChild(child2);
			s = OpenSession();
			tx = s.BeginTransaction();
			s.Merge(root);
			await (tx.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			s = OpenSession();
			tx = s.BeginTransaction();
			await (s.DeleteAsync(grandchild));
			await (s.DeleteAsync(grandchild2));
			await (s.DeleteAsync(grandchild3));
			await (s.DeleteAsync(child));
			await (s.DeleteAsync(child2));
			await (s.DeleteAsync(root));
			await (tx.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MergeDeepTreeWithGeneratedIdAsync()
		{
			ClearCounts();
			NumberedNode root;
			NumberedNode child;
			NumberedNode grandchild;
			using (ISession s = OpenSession())
			{
				ITransaction tx = s.BeginTransaction();
				root = new NumberedNode("root");
				child = new NumberedNode("child");
				grandchild = new NumberedNode("grandchild");
				root.AddChild(child);
				child.AddChild(grandchild);
				root = (NumberedNode)s.Merge(root);
				await (tx.CommitAsync());
			}

			AssertInsertCount(3);
			AssertUpdateCount(0);
			ClearCounts();
			IEnumerator<NumberedNode> rit = root.Children.GetEnumerator();
			rit.MoveNext();
			child = rit.Current;
			IEnumerator<NumberedNode> cit = child.Children.GetEnumerator();
			cit.MoveNext();
			grandchild = cit.Current;
			grandchild.Description = "the grand child";
			var grandchild2 = new NumberedNode("grandchild2");
			child.AddChild(grandchild2);
			using (ISession s = OpenSession())
			{
				ITransaction tx = s.BeginTransaction();
				root = (NumberedNode)s.Merge(root);
				await (tx.CommitAsync());
			}

			AssertInsertCount(1);
			AssertUpdateCount(1);
			ClearCounts();
			sessions.Evict(typeof (NumberedNode));
			var child2 = new NumberedNode("child2");
			var grandchild3 = new NumberedNode("grandchild3");
			child2.AddChild(grandchild3);
			root.AddChild(child2);
			using (ISession s = OpenSession())
			{
				ITransaction tx = s.BeginTransaction();
				root = (NumberedNode)s.Merge(root);
				await (tx.CommitAsync());
			}

			AssertInsertCount(2);
			AssertUpdateCount(0);
			ClearCounts();
			using (ISession s = OpenSession())
			{
				ITransaction tx = s.BeginTransaction();
				await (s.DeleteAsync("from NumberedNode where name like 'grand%'"));
				await (s.DeleteAsync("from NumberedNode where name like 'child%'"));
				await (s.DeleteAsync("from NumberedNode"));
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task MergeManagedAsync()
		{
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			var root = new NumberedNode("root");
			await (s.PersistAsync(root));
			await (tx.CommitAsync());
			ClearCounts();
			tx = s.BeginTransaction();
			var child = new NumberedNode("child");
			root.AddChild(child);
			Assert.That(s.Merge(root), Is.SameAs(root));
			IEnumerator<NumberedNode> rit = root.Children.GetEnumerator();
			rit.MoveNext();
			NumberedNode mergedChild = rit.Current;
			Assert.That(mergedChild, Is.Not.SameAs(child));
			Assert.That(await (s.ContainsAsync(mergedChild)));
			Assert.That(!await (s.ContainsAsync(child)));
			Assert.That(root.Children.Count, Is.EqualTo(1));
			Assert.That(root.Children.Contains(mergedChild));
			//assertNotSame( mergedChild, s.Merge(child) ); //yucky :(
			await (tx.CommitAsync());
			AssertInsertCount(1);
			AssertUpdateCount(0);
			Assert.That(root.Children.Count, Is.EqualTo(1));
			Assert.That(root.Children.Contains(mergedChild));
			tx = s.BeginTransaction();
			Assert.That(await (s.CreateCriteria(typeof (NumberedNode)).SetProjection(Projections.RowCount()).UniqueResultAsync()), Is.EqualTo(2));
			tx.Rollback();
			s.Close();
		}

		[Test]
		public async Task MergeManyToManyWithCollectionDeferenceAsync()
		{
			// setup base data...
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			var competition = new Competition();
			competition.Competitors.Add(new Competitor{Name = "Name"});
			competition.Competitors.Add(new Competitor());
			competition.Competitors.Add(new Competitor());
			await (s.PersistAsync(competition));
			await (tx.CommitAsync());
			s.Close();
			// the competition graph is now detached:
			//   1) create a new List reference to represent the competitors
			s = OpenSession();
			tx = s.BeginTransaction();
			var newComp = new List<Competitor>();
			Competitor originalCompetitor = competition.Competitors[0];
			originalCompetitor.Name = "Name2";
			newComp.Add(originalCompetitor);
			newComp.Add(new Competitor());
			//   2) set that new List reference unto the Competition reference
			competition.Competitors = newComp;
			//   3) attempt the merge
			var competition2 = (Competition)s.Merge(competition);
			await (tx.CommitAsync());
			s.Close();
			Assert.That(!(competition == competition2));
			Assert.That(!(competition.Competitors == competition2.Competitors));
			Assert.That(competition2.Competitors.Count, Is.EqualTo(2));
			s = OpenSession();
			tx = s.BeginTransaction();
			competition = await (s.GetAsync<Competition>(competition.Id));
			Assert.That(competition.Competitors.Count, Is.EqualTo(2));
			await (s.DeleteAsync(competition));
			await (tx.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task MergeStaleVersionFailsAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			var entity = new VersionedEntity{Id = "entity", Name = "entity"};
			await (s.PersistAsync(entity));
			await (s.Transaction.CommitAsync());
			s.Close();
			// make the detached 'entity' reference stale...
			s = OpenSession();
			s.BeginTransaction();
			var entity2 = await (s.GetAsync<VersionedEntity>(entity.Id));
			entity2.Name = "entity-name";
			await (s.Transaction.CommitAsync());
			s.Close();
			// now try to reattch it
			s = OpenSession();
			s.BeginTransaction();
			try
			{
				s.Merge(entity);
				await (s.Transaction.CommitAsync());
				Assert.Fail("was expecting staleness error");
			}
			catch (StaleObjectStateException)
			{
			// expected outcome...
			}
			finally
			{
				s.Transaction.Rollback();
				s.Close();
				await (CleanupAsync());
			}
		}

		[Test]
		public async Task MergeTreeAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			var root = new Node{Name = "root"};
			var child = new Node{Name = "child"};
			root.AddChild(child);
			await (s.PersistAsync(root));
			await (tx.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			ClearCounts();
			root.Description = "The root node";
			child.Description = "The child node";
			var secondChild = new Node{Name = "second child"};
			root.AddChild(secondChild);
			s = OpenSession();
			tx = s.BeginTransaction();
			s.Merge(root);
			await (tx.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(2);
		}

		[Test]
		public async Task MergeTreeWithGeneratedIdAsync()
		{
			ClearCounts();
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			var root = new NumberedNode("root");
			var child = new NumberedNode("child");
			root.AddChild(child);
			await (s.PersistAsync(root));
			await (tx.CommitAsync());
			s.Close();
			AssertInsertCount(2);
			ClearCounts();
			root.Description = "The root node";
			child.Description = "The child node";
			var secondChild = new NumberedNode("second child");
			root.AddChild(secondChild);
			s = OpenSession();
			tx = s.BeginTransaction();
			s.Merge(root);
			await (tx.CommitAsync());
			s.Close();
			AssertInsertCount(1);
			AssertUpdateCount(2);
		}

		[Test]
		public async Task NoExtraUpdatesOnMergeAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			var node = new Node{Name = "test"};
			await (s.PersistAsync(node));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			// node is now detached, but we have made no changes.  so attempt to merge it
			// into this new session; this should cause no updates...
			s = OpenSession();
			s.BeginTransaction();
			node = (Node)s.Merge(node);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			///////////////////////////////////////////////////////////////////////
			// as a control measure, now update the node while it is detached and
			// make sure we get an update as a result...
			node.Description = "new description";
			s = OpenSession();
			s.BeginTransaction();
			node = (Node)s.Merge(node);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(0);
		///////////////////////////////////////////////////////////////////////
		}

		[Test]
		public async Task NoExtraUpdatesOnMergeVersionedAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			var entity = new VersionedEntity{Id = "entity", Name = "entity"};
			await (s.PersistAsync(entity));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			// entity is now detached, but we have made no changes.  so attempt to merge it
			// into this new session; this should cause no updates...
			s = OpenSession();
			s.BeginTransaction();
			var mergedEntity = (VersionedEntity)s.Merge(entity);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			Assert.That(entity.Version, Is.EqualTo(mergedEntity.Version), "unexpected version increment");
			///////////////////////////////////////////////////////////////////////
			// as a control measure, now update the node while it is detached and
			// make sure we get an update as a result...
			entity.Name = "new name";
			s = OpenSession();
			s.BeginTransaction();
			entity = (VersionedEntity)s.Merge(entity);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(0);
		///////////////////////////////////////////////////////////////////////
		}

		[Test]
		public async Task NoExtraUpdatesOnMergeVersionedWithCollectionAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			var parent = new VersionedEntity{Id = "parent", Name = "parent"};
			var child = new VersionedEntity{Id = "child", Name = "child"};
			parent.Children.Add(child);
			child.Parent = parent;
			await (s.PersistAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			// parent is now detached, but we have made no changes.  so attempt to merge it
			// into this new session; this should cause no updates...
			s = OpenSession();
			s.BeginTransaction();
			var mergedParent = (VersionedEntity)s.Merge(parent);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			Assert.That(parent.Version, Is.EqualTo(mergedParent.Version), "unexpected parent version increment");
			IEnumerator<VersionedEntity> it = mergedParent.Children.GetEnumerator();
			it.MoveNext();
			VersionedEntity mergedChild = it.Current;
			Assert.That(child.Version, Is.EqualTo(mergedChild.Version), "unexpected child version increment");
			///////////////////////////////////////////////////////////////////////
			// as a control measure, now update the node while it is detached and
			// make sure we get an update as a result...
			mergedParent.Name = "new name";
			mergedParent.Children.Add(new VersionedEntity{Id = "child2", Name = "new child"});
			s = OpenSession();
			s.BeginTransaction();
			parent = (VersionedEntity)s.Merge(mergedParent);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(1);
		///////////////////////////////////////////////////////////////////////
		}

		[Test]
		public async Task NoExtraUpdatesOnMergeWithCollectionAsync()
		{
			ISession s = OpenSession();
			s.BeginTransaction();
			var parent = new Node{Name = "parent"};
			var child = new Node{Name = "child"};
			parent.Children.Add(child);
			child.Parent = parent;
			await (s.PersistAsync(parent));
			await (s.Transaction.CommitAsync());
			s.Close();
			ClearCounts();
			// parent is now detached, but we have made no changes.  so attempt to merge it
			// into this new session; this should cause no updates...
			s = OpenSession();
			s.BeginTransaction();
			parent = (Node)s.Merge(parent);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(0);
			AssertInsertCount(0);
			///////////////////////////////////////////////////////////////////////
			// as a control measure, now update the node while it is detached and
			// make sure we get an update as a result...
			IEnumerator<Node> it = parent.Children.GetEnumerator();
			it.MoveNext();
			it.Current.Description = "child's new description";
			parent.Children.Add(new Node{Name = "second child"});
			s = OpenSession();
			s.BeginTransaction();
			parent = (Node)s.Merge(parent);
			await (s.Transaction.CommitAsync());
			s.Close();
			AssertUpdateCount(1);
			AssertInsertCount(1);
		///////////////////////////////////////////////////////////////////////
		}

		[Test]
		public async Task PersistThenMergeInSameTxnWithTimestampAsync()
		{
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			var entity = new TimestampedEntity{Id = "test", Name = "test"};
			await (s.PersistAsync(entity));
			s.Merge(new TimestampedEntity{Id = "test", Name = "test-2"});
			try
			{
				// control operation...
				await (s.SaveOrUpdateAsync(new TimestampedEntity{Id = "test", Name = "test-3"}));
				Assert.Fail("saveOrUpdate() should fail here");
			}
			catch (NonUniqueObjectException)
			{
			// expected behavior
			}

			await (tx.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task PersistThenMergeInSameTxnWithVersionAsync()
		{
			ISession s = OpenSession();
			ITransaction tx = s.BeginTransaction();
			var entity = new VersionedEntity{Id = "test", Name = "test"};
			await (s.PersistAsync(entity));
			s.Merge(new VersionedEntity{Id = "test", Name = "test-2"});
			try
			{
				// control operation...
				await (s.SaveOrUpdateAsync(new VersionedEntity{Id = "test", Name = "test-3"}));
				Assert.Fail("saveOrUpdate() should fail here");
			}
			catch (NonUniqueObjectException)
			{
			// expected behavior
			}

			await (tx.CommitAsync());
			s.Close();
		}

		[Test]
		public async Task RecursiveMergeTransientAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var jboss = new Employer();
					var gavin = new Employee();
					jboss.Employees = new List<Employee>{gavin};
					s.Merge(jboss);
					await (s.FlushAsync());
					jboss = await (s.CreateQuery("from Employer e join fetch e.Employees").UniqueResultAsync<Employer>());
					Assert.That(NHibernateUtil.IsInitialized(jboss.Employees));
					Assert.That(jboss.Employees.Count, Is.EqualTo(1));
					s.Clear();
					IEnumerator<Employee> it = jboss.Employees.GetEnumerator();
					it.MoveNext();
					s.Merge(it.Current);
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
