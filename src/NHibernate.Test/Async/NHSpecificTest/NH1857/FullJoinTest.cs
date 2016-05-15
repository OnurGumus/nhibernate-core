#if NET_4_5
using System;
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1857
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FullJoinTest : BugTestCase
	{
		[Test]
		public async Task TestFullJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var q = session.CreateQuery("from Employee as e full join e.Department");
					var result = await (q.ListAsync());
					Assert.AreEqual(3, result.Count);
				}
		}
	}
}
#endif
