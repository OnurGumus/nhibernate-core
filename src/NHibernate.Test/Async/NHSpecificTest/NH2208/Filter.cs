#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2208
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FilterAsync : BugTestCaseAsync
	{
		[Test, Ignore("Not fixed yet")]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
			{
				session.EnableFilter("myfilter");
				await (session.CreateQuery("from E1 e join fetch e.BO").ListAsync());
			}
		}
	}
}
#endif
