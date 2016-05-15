#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.DomainModel;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.GenericTest.Methods
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task CriteriaAsync()
		{
			using (ISession s2 = OpenSession())
				using (ITransaction t2 = s2.BeginTransaction())
				{
					IList<One> results2 = await (s2.CreateCriteria(typeof (One)).Add(Expression.Eq("X", 20)).ListAsync<One>());
					Assert.AreEqual(1, results2.Count);
					One one2 = results2[0];
					Assert.IsNotNull(one2, "Unable to load object");
					Assert.AreEqual(one.X, one2.X, "Load failed");
				}
		}

		[Test]
		public async Task QueryListAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					IList<One> results = await (s.CreateQuery("from One").ListAsync<One>());
					Assert.AreEqual(1, results.Count);
				}
		}

		[Test]
		public async Task QueryEnumerableAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					IEnumerable<One> results = await (s.CreateQuery("from One").EnumerableAsync<One>());
					IEnumerator<One> en = results.GetEnumerator();
					Assert.IsTrue(en.MoveNext());
					Assert.IsFalse(en.MoveNext());
				}
		}

		[Test]
		public async Task FilterAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					IList<Many> results = await ((await (s.CreateFilterAsync(one2.Manies, "where X = 10"))).ListAsync<Many>());
					Assert.AreEqual(1, results.Count);
					Assert.AreEqual(10, results[0].X);
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task FilterEnumerableAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					IEnumerable<Many> results = await ((await (s.CreateFilterAsync(one2.Manies, "where X = 10"))).EnumerableAsync<Many>());
					IEnumerator<Many> en = results.GetEnumerator();
					Assert.IsTrue(en.MoveNext());
					Assert.AreEqual(10, en.Current.X);
					Assert.IsFalse(en.MoveNext());
					await (t.CommitAsync());
				}
		}
	}
}
#endif
