#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2692
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test, KnownBug("NH-2692")]
		public async Task QueryingChildrenComponentsHqlAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await (session.CreateQuery("select c from Parent as p join p.ChildComponents as c").ListAsync<ChildComponent>());
					Assert.That(result, Has.Count.EqualTo(1));
				}
		}
	}
}
#endif
