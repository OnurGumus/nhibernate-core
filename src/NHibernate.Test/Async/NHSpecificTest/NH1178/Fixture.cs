#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1178
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ExcludeNullsAndZeroesAsync()
		{
			using (ISession s = OpenSession())
			{
				Example example = Example.Create(new Foo(1000, "mono")).ExcludeZeroes().ExcludeNulls();
				IList results = await (s.CreateCriteria(typeof (Foo)).Add(example).ListAsync());
				Assert.AreEqual(1, results.Count);
			}

			using (ISession s = OpenSession())
			{
				Example example = Example.Create(new Foo(1000, "mono")).ExcludeNulls().ExcludeZeroes();
				IList results = await (s.CreateCriteria(typeof (Foo)).Add(example).ListAsync());
				Assert.AreEqual(1, results.Count);
			}
		}
	}
}
#endif
