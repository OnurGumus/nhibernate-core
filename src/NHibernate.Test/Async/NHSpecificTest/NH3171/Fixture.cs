#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3171
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SqlShouldIncludeAliasAsJoinWhenRestrictingByCompositeKeyColumnAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					// Should not throw
					// The multi-part identifier "s1_.Name" could not be bound.
					await (s.CreateCriteria<Artist>("a").CreateAlias("a.Song", "s").Add(Restrictions.Eq("s.Name", "Miss Kiss Kiss Bang")).ListAsync<Artist>());
				}
		}
	}
}
#endif
