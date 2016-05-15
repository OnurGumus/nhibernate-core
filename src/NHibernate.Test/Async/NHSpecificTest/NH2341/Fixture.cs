#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2341
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task WhenSaveInstanceOfConcreteInheritedThenNotThrowsAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var entity = new ConcreteB();
					Assert.That(() => session.Save(entity), Throws.Nothing);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
