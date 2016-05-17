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
		public async Task FilterAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					One one2 = (One)await (s.CreateQuery("from One").UniqueResultAsync());
					IList<Many> results = s.CreateFilter(one2.Manies, "where X = 10").List<Many>();
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
					IEnumerable<Many> results = s.CreateFilter(one2.Manies, "where X = 10").Enumerable<Many>();
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
