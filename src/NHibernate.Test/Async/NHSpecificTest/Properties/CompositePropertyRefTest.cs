#if NET_4_5
using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Properties
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CompositePropertyRefTest : BugTestCase
	{
		[Test]
		public async Task MappingOuterJoinAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var p = await (s.GetAsync<Person>(p_id)); //get address reference by outer join
					var p2 = await (s.GetAsync<Person>(p2_id)); //get null address reference by outer join
					Assert.IsNull(p2.Address);
					Assert.IsNotNull(p.Address);
					var l = await (s.CreateQuery("from Person").ListAsync()); //pull address references for cache
					Assert.AreEqual(l.Count, 2);
					Assert.IsTrue(l.Contains(p) && l.Contains(p2));
				}
			}
		}

		[Test]
		public async Task AddressBySequentialSelectAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var l = await (s.CreateQuery("from Person p order by p.Name").ListAsync<Person>());
					Assert.AreEqual(l.Count, 2);
					Assert.IsNull(l[0].Address);
					Assert.IsNotNull(l[1].Address);
				}
			}
		}

		[Test]
		public async Task AddressOuterJoinAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var l = await (s.CreateQuery("from Person p left join fetch p.Address a order by a.Country").ListAsync<Person>());
					Assert.AreEqual(l.Count, 2);
					if (l[0].Name.Equals("Max"))
					{
						Assert.IsNull(l[0].Address);
						Assert.IsNotNull(l[1].Address);
					}
					else
					{
						Assert.IsNull(l[1].Address);
						Assert.IsNotNull(l[0].Address);
					}
				}
			}
		}

		[Test]
		public async Task AccountsOuterJoinAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var l = await (s.CreateQuery("from Person p left join p.Accounts").ListAsync());
					for (var i = 0; i < 2; i++)
					{
						var row = (object[])l[i];
						var px = (Person)row[0];
						var accounts = px.Accounts;
						Assert.IsFalse(NHibernateUtil.IsInitialized(accounts));
						Assert.IsTrue(px.Accounts.Count > 0 || row[1] == null);
					}
				}
			}
		}

		[Test]
		public async Task AccountsOuterJoinVerifyInitializationAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var l = await (s.CreateQuery("from Person p left join fetch p.Accounts a order by p.Name").ListAsync<Person>());
					var p0 = l[0];
					Assert.IsTrue(NHibernateUtil.IsInitialized(p0.Accounts));
					Assert.AreEqual(p0.Accounts.Count, 1);
					Assert.AreSame(p0.Accounts.First().User, p0);
					var p1 = l[1];
					Assert.IsTrue(NHibernateUtil.IsInitialized(p1.Accounts));
					Assert.AreEqual(p1.Accounts.Count, 0);
				}
			}
		}
	}
}
#endif
