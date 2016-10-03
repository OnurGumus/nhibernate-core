#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1400
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task DotInStringLiteralsConstantAsync()
		{
			using (ISession s = OpenSession())
			{
				Assert.DoesNotThrowAsync(async () => await (s.CreateQuery("from SimpleGeographicalAddress as dga where dga.Line2 = 'B1 P9, Scb, Ap. 18'").ListAsync()));
			}
		}
	}
}
#endif
