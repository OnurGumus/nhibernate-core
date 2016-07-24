#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1905
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task QueryAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.CreateQuery("select d from Det d left join d.Mas m where (SELECT count(e) FROM d.Mas.Els e WHERE e.Descr='e1')>0").ListAsync());
			}
		}
	}
}
#endif
