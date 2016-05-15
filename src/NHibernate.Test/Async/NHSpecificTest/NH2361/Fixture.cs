#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2361
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task WhenDeleteMultiTableHierarchyThenNotThrowsAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.CreateQuery("delete from Animal").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
