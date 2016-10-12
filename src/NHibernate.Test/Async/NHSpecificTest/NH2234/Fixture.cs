#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2234
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CanQueryViaLinqAsync()
		{
			using (var s = OpenSession())
			{
				var qry =
					from item in s.Query<SomethingLinq>()where item.Relation == MyUserTypes.Value1
					select item;
				await (qry.ToListAsync());
				Assert.That(async () => await (qry.ToListAsync()), Throws.Nothing);
			}
		}
	}
}
#endif
