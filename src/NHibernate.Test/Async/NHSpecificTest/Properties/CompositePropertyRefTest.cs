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
					var l = s.CreateQuery("from Person").List(); //pull address references for cache
					Assert.AreEqual(l.Count, 2);
					Assert.IsTrue(l.Contains(p) && l.Contains(p2));
				}
			}
		}
	}
}
#endif
