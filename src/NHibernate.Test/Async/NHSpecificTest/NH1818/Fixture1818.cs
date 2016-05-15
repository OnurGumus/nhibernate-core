#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1818
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture1818 : BugTestCase
	{
		[Test]
		[Description("Test HQL query on a property mapped with a formula.")]
		public async Task ComputedPropertyShouldRetrieveDataCorrectlyAsync()
		{
			using (var session = OpenSession())
			{
				var obj = await (session.CreateQuery("from DomainClass dc where dc.AlwaysTrue").UniqueResultAsync<DomainClass>());
				Assert.IsNotNull(obj);
			}
		}
	}
}
#endif
