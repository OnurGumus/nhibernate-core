#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2000
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		// In this version of nHibernate, GetEnabledFilter throws an exception
		// instead returning nothing like in previous versions.
		[Test]
		public void TestSessionGetEnableFilter()
		{
			using (ISession session = OpenSession())
			{
				IFilter filter = session.GetEnabledFilter("TestFilter");
			}
		}
	}
}
#endif
