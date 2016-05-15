#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3057
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CollectionQueryOnJoinedSubclassInheritedPropertyHqlAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var entities = await (session.CreateQuery("from AClass a where exists (from a.Bs b where b.InheritedProperty = 'B2')").ListAsync<AClass>());
					Assert.AreEqual(1, entities.Count);
					Assert.AreEqual(1, entities[0].Id);
				}
		}
	}
}
#endif
