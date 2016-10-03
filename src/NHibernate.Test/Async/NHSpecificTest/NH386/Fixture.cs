#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH386
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH386";
			}
		}

		[Test]
		public async Task QueryAsync()
		{
			using (ISession s = OpenSession())
			{
				await (s.CreateQuery("from _Parent _p left join _p._Children _c where _c._Id > 0").ListAsync());
			}
		}
	}
}
#endif
