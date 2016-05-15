#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2074
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanQueryOnPropertyUsingUnicodeTokenAsync()
		{
			using (var s = OpenSession())
			{
				await (s.CreateQuery("from Person").ListAsync());
			}
		}
	}
}
#endif
