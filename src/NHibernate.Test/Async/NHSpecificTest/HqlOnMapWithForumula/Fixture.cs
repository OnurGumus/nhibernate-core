#if NET_4_5
using System;
using System.Text;
using NHibernate.Test.NHSpecificTest;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.HqlOnMapWithForumula
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "HqlOnMapWithForumula";
			}
		}

		[Test]
		public async Task TestBugAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.CreateQuery("from A a where 1 in elements(a.MyMaps)").ListAsync());
			}
		}
	}
}
#endif
