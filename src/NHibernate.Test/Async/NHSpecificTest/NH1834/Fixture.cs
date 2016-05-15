#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1834
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task OneToManyPropertyWithFormulaNodeShouldWorkLikeFormulaAttribAsync()
		{
			using (ISession session = base.OpenSession())
			{
				session.Clear();
				var b = await (session.GetAsync<B>(1));
				Assert.IsNotNull(b.A2);
				Assert.IsNotNull(b.A);
				Assert.That(b.A.Id == b.A2.Id);
			}
		}
	}
}
#endif
