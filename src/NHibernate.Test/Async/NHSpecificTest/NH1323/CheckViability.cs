#if NET_4_5
using System;
using NHibernate.Collection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1323
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CheckViability : BugTestCase
	{
		[Test]
		public async Task WhenReassociateCollectionUsingMergeThenReassingOwnerAsync()
		{
			using (var scenario = new FullInitializedRetrievedEntity(Sfi))
			{
				((IPersistentCollection)scenario.Entity.Children).Owner = null;
				((IPersistentCollection)scenario.Entity.Components).Owner = null;
				((IPersistentCollection)scenario.Entity.Elements).Owner = null;
				// When I reassociate the collections the Owner has value
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var merged = (MyClass)session.Merge(scenario.Entity);
						Assert.That(((IPersistentCollection)merged.Children).Owner, Is.Not.Null);
						Assert.That(((IPersistentCollection)merged.Components).Owner, Is.Not.Null);
						Assert.That(((IPersistentCollection)merged.Elements).Owner, Is.Not.Null);
						await (session.Transaction.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenReassociateCollectionUsingLockThenTheCommitNotThrowsAsync()
		{
			using (var scenario = new FullInitializedRetrievedEntity(Sfi))
			{
				((IPersistentCollection)scenario.Entity.Children).Owner = null;
				((IPersistentCollection)scenario.Entity.Components).Owner = null;
				((IPersistentCollection)scenario.Entity.Elements).Owner = null;
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						// When I reassociate the collections the Owner is null
						session.Lock(scenario.Entity, LockMode.None);
						// If I change something in each collection, there is no problems
						scenario.Entity.Children.Add(new MyChild{Parent = scenario.Entity});
						scenario.Entity.Components.Add(new MyComponent{Something = "something"});
						scenario.Entity.Elements.Add("somethingelse");
						await (session.Transaction.CommitAsync());
					}

				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var fresh = await (session.GetAsync<MyClass>(scenario.Entity.Id));
						Assert.That(fresh.Children, Has.Count.EqualTo(2));
						Assert.That(fresh.Components, Has.Count.EqualTo(2));
						Assert.That(fresh.Elements, Has.Count.EqualTo(2));
						await (session.Transaction.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenReassociateCollectionUsingUpdateThenTheCommitNotThrowsAsync()
		{
			using (var scenario = new FullInitializedRetrievedEntity(Sfi))
			{
				((IPersistentCollection)scenario.Entity.Children).Owner = null;
				((IPersistentCollection)scenario.Entity.Components).Owner = null;
				((IPersistentCollection)scenario.Entity.Elements).Owner = null;
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						scenario.Entity.Children.Add(new MyChild{Parent = scenario.Entity});
						scenario.Entity.Components.Add(new MyComponent{Something = "something"});
						scenario.Entity.Elements.Add("somethingelse");
						// When I reassociate the collections the Owner is null
						await (session.UpdateAsync(scenario.Entity));
						await (session.Transaction.CommitAsync());
					}

				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var fresh = await (session.GetAsync<MyClass>(scenario.Entity.Id));
						Assert.That(fresh.Children, Has.Count.EqualTo(2));
						Assert.That(fresh.Components, Has.Count.EqualTo(2));
						Assert.That(fresh.Elements, Has.Count.EqualTo(2));
						await (session.Transaction.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenReassociateCollectionUsingSaveOrUpdateThenTheCommitNotThrowsAsync()
		{
			using (var scenario = new FullInitializedRetrievedEntity(Sfi))
			{
				((IPersistentCollection)scenario.Entity.Children).Owner = null;
				((IPersistentCollection)scenario.Entity.Components).Owner = null;
				((IPersistentCollection)scenario.Entity.Elements).Owner = null;
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						scenario.Entity.Children.Add(new MyChild{Parent = scenario.Entity});
						scenario.Entity.Components.Add(new MyComponent{Something = "something"});
						scenario.Entity.Elements.Add("somethingelse");
						// When I reassociate the collections the Owner is null
						session.SaveOrUpdate(scenario.Entity);
						await (session.Transaction.CommitAsync());
					}

				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var fresh = await (session.GetAsync<MyClass>(scenario.Entity.Id));
						Assert.That(fresh.Children, Has.Count.EqualTo(2));
						Assert.That(fresh.Components, Has.Count.EqualTo(2));
						Assert.That(fresh.Elements, Has.Count.EqualTo(2));
						await (session.Transaction.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenReassociateCollectionUsingDeleteThenTheCommitNotThrowsAsync()
		{
			using (var scenario = new FullInitializedRetrievedEntity(Sfi))
			{
				((IPersistentCollection)scenario.Entity.Children).Owner = null;
				((IPersistentCollection)scenario.Entity.Components).Owner = null;
				((IPersistentCollection)scenario.Entity.Elements).Owner = null;
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						await (session.DeleteAsync(scenario.Entity));
						await (session.Transaction.CommitAsync());
					}

				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var fresh = await (session.GetAsync<MyClass>(scenario.Entity.Id));
						Assert.That(fresh, Is.Null);
						await (session.Transaction.CommitAsync());
					}
			}
		}
	}
}
#endif
