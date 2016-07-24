#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MultipleCollectionFetchTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractMultipleCollectionFetchFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected abstract void AddToCollection(ICollection<Person> collection, Person person);
		protected abstract ICollection<Person> CreateCollection();
		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync("from Person p where p.Parent is null"));
				await (s.FlushAsync());
			}
		}

		private Person CreateGrandparent()
		{
			Person parent = new Person();
			parent.Children = CreateCollection();
			for (int i = 0; i < 2; i++)
			{
				Person child = new Person();
				child.Parent = parent;
				AddToCollection(parent.Children, child);
				child.Children = CreateCollection();
				for (int j = 0; j < 3; j++)
				{
					Person grandChild = new Person();
					grandChild.Parent = child;
					AddToCollection(child.Children, grandChild);
				}
			}

			return parent;
		}

		protected virtual async Task RunLinearJoinFetchTestAsync(Person parent)
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(parent));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				Person p = (Person)await (s.CreateQuery("select p from Person p join fetch p.Children c join fetch c.Children gc").UniqueResultAsync());
				Assert.IsTrue(NHibernateUtil.IsInitialized(p.Children));
				Assert.AreEqual(2, p.Children.Count);
				foreach (Person child in p.Children)
				{
					Assert.IsTrue(NHibernateUtil.IsInitialized(child));
					Assert.IsTrue(NHibernateUtil.IsInitialized(child.Children));
					Assert.AreEqual(3, child.Children.Count);
				}
			}
		}

		// Tests "linear" join fetch, i.e. A join A.B join A.B.C
		[Test]
		public async Task MultipleCollectionsLinearJoinFetchAsync()
		{
			Person parent = CreateGrandparent();
			await (RunLinearJoinFetchTestAsync(parent));
		}

		private Person CreateParentAndFriend()
		{
			Person parent = new Person();
			parent.Children = CreateCollection();
			for (int i = 0; i < 2; i++)
			{
				Person child = new Person();
				child.Parent = parent;
				AddToCollection(parent.Children, child);
			}

			parent.Friends = CreateCollection();
			for (int i = 0; i < 3; i++)
			{
				Person friend = new Person();
				AddToCollection(parent.Friends, friend);
			}

			return parent;
		}

		protected virtual async Task RunNonLinearJoinFetchTestAsync(Person person)
		{
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(person));
				await (s.FlushAsync());
			}

			try
			{
				using (ISession s = OpenSession())
				{
					Person p = (Person)await (s.CreateQuery("select p from Person p join fetch p.Children join fetch p.Friends f").UniqueResultAsync());
					Assert.IsTrue(NHibernateUtil.IsInitialized(p.Children));
					Assert.AreEqual(2, p.Children.Count);
					foreach (Person child in p.Children)
					{
						Assert.IsTrue(NHibernateUtil.IsInitialized(child));
					}

					Assert.IsTrue(NHibernateUtil.IsInitialized(p.Friends));
					Assert.AreEqual(3, p.Friends.Count);
					foreach (Person friend in p.Friends)
					{
						Assert.IsTrue(NHibernateUtil.IsInitialized(friend));
					}
				}
			}
			finally
			{
				using (ISession s = OpenSession())
				{
					await (s.DeleteAsync("from Person p where p.Parent is null"));
					await (s.FlushAsync());
				}
			}
		}

		// Tests "non-linear" join fetch, i.e. A join A.B join A.C
		[Test]
		public async Task MultipleCollectionsNonLinearJoinFetchAsync()
		{
			Person person = CreateParentAndFriend();
			await (RunNonLinearJoinFetchTestAsync(person));
		}
	}
}
#endif
