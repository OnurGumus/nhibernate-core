#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MultipleCollectionFetchTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractMultipleCollectionFetchFixture : TestCase
	{
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
