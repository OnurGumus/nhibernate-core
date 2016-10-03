#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1119
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1119";
			}
		}

		[Test]
		public async Task SelectMinFromEmptyTableAsync()
		{
			using (ISession s = OpenSession())
			{
				DateTime dt = await (s.CreateQuery("select max(tc.DateTimeProperty) from TestClass tc").UniqueResultAsync<DateTime>());
				Assert.AreEqual(default (DateTime), dt);
				DateTime? dtn = await (s.CreateQuery("select max(tc.DateTimeProperty) from TestClass tc").UniqueResultAsync<DateTime? >());
				Assert.IsFalse(dtn.HasValue);
			}
		}
	}
}
#endif
