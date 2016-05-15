#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.EntityNameAndInheritance
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task DoesNotCrashAsync()
		{
			using (var s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					Assert.IsNotNull(await (s.GetAsync(entityName, id)));
				}
			}
		}
	}
}
#endif
