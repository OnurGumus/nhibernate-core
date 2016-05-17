#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OrderByTest : BaseFixture
	{
		[Test]
		public async Task TestOrderByNoSelectAliasRefAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					CheckTestOrderByResults(s.CreateQuery("select name, address from Zoo order by name, address").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address from Zoo z order by z.name, z.address").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z2.name, z2.address from Zoo z2 where z2.name in ( select name from Zoo ) order by z2.name, z2.address").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					// using ASC
					CheckTestOrderByResults(s.CreateQuery("select name, address from Zoo order by name ASC, address ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address from Zoo z order by z.name ASC, z.address ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z2.name, z2.address from Zoo z2 where z2.name in ( select name from Zoo ) order by z2.name ASC, z2.address ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					// ordered by address, name:
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address from Zoo z order by z.address, z.name").List(), data.Zoo3, data.Zoo4, data.Zoo2, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select name, address from Zoo order by address, name").List(), data.Zoo3, data.Zoo4, data.Zoo2, data.Zoo1, null);
					// ordered by address:
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					// unordered:
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address from Zoo z order by z.address").List(), data.Zoo3, data.Zoo4, null, null, data.ZoosWithSameAddress);
					CheckTestOrderByResults(s.CreateQuery("select name, address from Zoo order by address").List(), data.Zoo3, data.Zoo4, null, null, data.ZoosWithSameAddress);
					// ordered by name:
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					// unordered:
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address from Zoo z order by z.name").List(), data.Zoo2, data.Zoo4, null, null, data.ZoosWithSameName);
					CheckTestOrderByResults(s.CreateQuery("select name, address from Zoo order by name").List(), data.Zoo2, data.Zoo4, null, null, data.ZoosWithSameName);
					await (txn.CommitAsync());
				}
		}

		[Test, KnownBug("HHH-5574")]
		public async Task TestOrderByComponentDescNoSelectAliasRefFailureExpectedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address from Zoo z order by z.address DESC, z.name DESC").List(), data.Zoo1, data.Zoo2, data.Zoo4, data.Zoo3, null);
					CheckTestOrderByResults(s.CreateQuery("select name, address from Zoo order by address DESC, name DESC").List(), data.Zoo1, data.Zoo2, data.Zoo4, data.Zoo3, null);
					await (txn.CommitAsync());
				}
		}

		[Test]
		public async Task TestOrderBySelectAliasRefAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					CheckTestOrderByResults(s.CreateQuery("select z2.name as zname, z2.address as zooAddress from Zoo z2 where z2.name in ( select name from Zoo ) order by zname, zooAddress").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name as name, z.address as address from Zoo z order by name, address").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name as zooName, z.address as zooAddress from Zoo z order by zooName, zooAddress").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address as name from Zoo z order by z.name, name").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address as name from Zoo z order by z.name, name").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					// using ASC
					CheckTestOrderByResults(s.CreateQuery("select z2.name as zname, z2.address as zooAddress from Zoo z2 where z2.name in ( select name from Zoo ) order by zname ASC, zooAddress ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name as name, z.address as address from Zoo z order by name ASC, address ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name as zooName, z.address as zooAddress from Zoo z order by zooName ASC, zooAddress ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address as name from Zoo z order by z.name ASC, name ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address as name from Zoo z order by z.name ASC, name ASC").List(), data.Zoo2, data.Zoo4, data.Zoo3, data.Zoo1, null);
					// ordered by address, name:
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					CheckTestOrderByResults(s.CreateQuery("select z.name as address, z.address as name from Zoo z order by name, address").List(), data.Zoo3, data.Zoo4, data.Zoo2, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address as name from Zoo z order by name, z.name").List(), data.Zoo3, data.Zoo4, data.Zoo2, data.Zoo1, null);
					// using ASC
					CheckTestOrderByResults(s.CreateQuery("select z.name as address, z.address as name from Zoo z order by name ASC, address ASC").List(), data.Zoo3, data.Zoo4, data.Zoo2, data.Zoo1, null);
					CheckTestOrderByResults(s.CreateQuery("select z.name, z.address as name from Zoo z order by name ASC, z.name ASC").List(), data.Zoo3, data.Zoo4, data.Zoo2, data.Zoo1, null);
					// ordered by address:
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					// unordered:
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					CheckTestOrderByResults(s.CreateQuery("select z.name as zooName, z.address as zooAddress from Zoo z order by zooAddress").List(), data.Zoo3, data.Zoo4, null, null, data.ZoosWithSameAddress);
					CheckTestOrderByResults(s.CreateQuery("select z.name as zooName, z.address as name from Zoo z order by name").List(), data.Zoo3, data.Zoo4, null, null, data.ZoosWithSameAddress);
					// ordered by name:
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					// unordered:
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					CheckTestOrderByResults(s.CreateQuery("select z.name as zooName, z.address as zooAddress from Zoo z order by zooName").List(), data.Zoo2, data.Zoo4, null, null, data.ZoosWithSameName);
					CheckTestOrderByResults(s.CreateQuery("select z.name as address, z.address as name from Zoo z order by address").List(), data.Zoo2, data.Zoo4, null, null, data.ZoosWithSameName);
					await (txn.CommitAsync());
				}
		}

		[Test, KnownBug("HHH-5574")]
		public async Task TestOrderByComponentDescSelectAliasRefFailureExpectedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					CheckTestOrderByResults(s.CreateQuery("select z.name as zooName, z.address as zooAddress from Zoo z order by zooAddress DESC, zooName DESC").List(), data.Zoo1, data.Zoo2, data.Zoo4, data.Zoo3, null);
					await (txn.CommitAsync());
				}
		}

		[Test]
		public async Task TestOrderByEntityWithFetchJoinedCollectionAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					// ordered by address desc, name desc:
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					// using DESC
					var list = s.CreateQuery("from Zoo z join fetch z.mammals").List();
					await (txn.CommitAsync());
				}
		}

		[Test]
		public async Task TestOrderBySelectNewArgAliasRefAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					// ordered by name, address:
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					var list = s.CreateQuery("select new Zoo(z.name as zname, z.address as zaddress) from Zoo z order by zname, zaddress").List();
					Assert.AreEqual(4, list.Count);
					Assert.AreEqual(data.Zoo2, list[0]);
					Assert.AreEqual(data.Zoo4, list[1]);
					Assert.AreEqual(data.Zoo3, list[2]);
					Assert.AreEqual(data.Zoo1, list[3]);
					// ordered by address, name:
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					list = s.CreateQuery("select new Zoo( z.name as zname, z.address as zaddress) from Zoo z order by zaddress, zname").List();
					Assert.AreEqual(4, list.Count);
					Assert.AreEqual(data.Zoo3, list[0]);
					Assert.AreEqual(data.Zoo4, list[1]);
					Assert.AreEqual(data.Zoo2, list[2]);
					Assert.AreEqual(data.Zoo1, list[3]);
					await (txn.CommitAsync());
				}
		}

		[Test]
		public async Task TestOrderBySelectNewMapArgAliasRefAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					// ordered by name, address:
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					var list = s.CreateQuery("select new map( z.name as zname, z.address as zaddress ) from Zoo z left join z.mammals m order by zname, zaddress").List();
					// NHibernate different behaviour hashtable does not maintain identity 
					Assert.AreEqual(5, list.Count);
					Assert.AreEqual(data.Zoo2.Name, ((Hashtable)list[0])["zname"]);
					Assert.AreEqual(data.Zoo2.Address, ((Hashtable)list[0])["zaddress"]);
					Assert.AreEqual(data.Zoo4.Name, ((Hashtable)list[1])["zname"]);
					Assert.AreEqual(data.Zoo4.Address, ((Hashtable)list[1])["zaddress"]);
					Assert.AreEqual(data.Zoo3.Name, ((Hashtable)list[2])["zname"]);
					Assert.AreEqual(data.Zoo3.Address, ((Hashtable)list[2])["zaddress"]);
					Assert.AreEqual(data.Zoo1.Name, ((Hashtable)list[3])["zname"]);
					Assert.AreEqual(data.Zoo1.Address, ((Hashtable)list[3])["zaddress"]);
					// ordered by address, name:
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					list = s.CreateQuery("select new map( z.name as zname, z.address as zaddress ) from Zoo z left join z.mammals m order by zaddress, zname").List();
					Assert.AreEqual(5, list.Count);
					Assert.AreEqual(data.Zoo3.Name, ((Hashtable)list[0])["zname"]);
					Assert.AreEqual(data.Zoo3.Address, ((Hashtable)list[0])["zaddress"]);
					Assert.AreEqual(data.Zoo4.Name, ((Hashtable)list[1])["zname"]);
					Assert.AreEqual(data.Zoo4.Address, ((Hashtable)list[1])["zaddress"]);
					Assert.AreEqual(data.Zoo2.Name, ((Hashtable)list[2])["zname"]);
					Assert.AreEqual(data.Zoo2.Address, ((Hashtable)list[2])["zaddress"]);
					Assert.AreEqual(data.Zoo1.Name, ((Hashtable)list[3])["zname"]);
					Assert.AreEqual(data.Zoo1.Address, ((Hashtable)list[3])["zaddress"]);
					await (txn.CommitAsync());
				}
		}

		[Test]
		public async Task TestOrderByAggregatedArgAliasRefAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					// ordered by name, address:
					//   zoo2  A Zoo       1313 Mockingbird Lane, Anywhere, IL USA
					//   zoo4  Duh Zoo     1312 Mockingbird Lane, Nowhere, IL USA
					//   zoo3  Zoo         1312 Mockingbird Lane, Anywhere, IL USA
					//   zoo1  Zoo         1313 Mockingbird Lane, Anywhere, IL USA
					var list = s.CreateQuery("select z.name as zname, count(*) as cnt from Zoo z group by z.name order by cnt desc, zname").List();
					Assert.AreEqual(3, list.Count);
					Assert.AreEqual(data.Zoo3.Name, ((Object[])list[0])[0]);
					Assert.AreEqual(2L, ((Object[])list[0])[1]);
					Assert.AreEqual(data.Zoo2.Name, ((Object[])list[1])[0]);
					Assert.AreEqual(1L, ((Object[])list[1])[1]);
					Assert.AreEqual(data.Zoo4.Name, ((Object[])list[2])[0]);
					Assert.AreEqual(1L, ((Object[])list[2])[1]);
					await (txn.CommitAsync());
				}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TestData
		{
			public async Task PrepareAsync()
			{
				using (ISession session = tc.OpenNewSession())
					using (ITransaction txn = session.BeginTransaction())
					{
						_stateProvince = new StateProvince{Name = "IL"};
						_zoo1 = new Zoo{Name = "Zoo", Address = new Address{Street = "1313 Mockingbird Lane", City = "Anywhere", StateProvince = StateProvince, Country = "USA"}, Mammals = new Dictionary<string, Mammal>()};
						_zooMammal1 = new Mammal{Description = "zooMammal1", Zoo = Zoo1};
						Zoo1.Mammals.Add("type1", ZooMammal1);
						_zooMammal2 = new Mammal{Description = "zooMammal2", Zoo = Zoo1};
						Zoo1.Mammals.Add("type2", ZooMammal2);
						_zoo2 = new Zoo{Name = "A Zoo", Address = new Address{Street = "1313 Mockingbird Lane", City = "Anywhere", StateProvince = StateProvince, Country = "USA"}};
						_zoo3 = new Zoo{Name = "Zoo", Address = new Address{Street = "1312 Mockingbird Lane", City = "Anywhere", StateProvince = StateProvince, Country = "USA"}};
						_zoo4 = new Zoo{Name = "Duh Zoo", Address = new Address{Street = "1312 Mockingbird Lane", City = "Nowhere", StateProvince = StateProvince, Country = "USA"}};
						await (session.SaveAsync(StateProvince));
						await (session.SaveAsync(ZooMammal1));
						await (session.SaveAsync(ZooMammal2));
						await (session.SaveAsync(Zoo1));
						await (session.SaveAsync(Zoo2));
						await (session.SaveAsync(Zoo3));
						await (session.SaveAsync(Zoo4));
						await (txn.CommitAsync());
					}

				_zoosWithSameName = new HashSet<Zoo>();
				ZoosWithSameName.Add(Zoo1);
				ZoosWithSameName.Add(Zoo3);
				_zoosWithSameAddress = new HashSet<Zoo>();
				ZoosWithSameAddress.Add(Zoo1);
				ZoosWithSameAddress.Add(Zoo2);
			}

			public async Task CleanupAsync()
			{
				using (ISession session = tc.OpenNewSession())
					using (ITransaction txn = session.BeginTransaction())
					{
						await (session.DeleteAsync(Zoo1));
						await (session.DeleteAsync(Zoo2));
						await (session.DeleteAsync(Zoo3));
						await (session.DeleteAsync(Zoo4));
						await (session.DeleteAsync(ZooMammal1));
						await (session.DeleteAsync(ZooMammal2));
						await (session.DeleteAsync(StateProvince));
						await (txn.CommitAsync());
					}
			}
		}
	}
}
#endif
