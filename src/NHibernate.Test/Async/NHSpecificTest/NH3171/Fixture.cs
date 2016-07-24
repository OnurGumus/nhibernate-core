#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3171
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var entrant = new Artist{Name = "Alex Swings Oscar Sings", Song = new Song{Name = "Miss Kiss Kiss Bang"}, Country = new Country{Name = "Germany"}};
					await (session.SaveAsync(entrant.Country));
					await (session.SaveAsync(entrant.Song));
					await (session.SaveAsync(entrant));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Artist"));
					await (s.DeleteAsync("from Song"));
					await (s.DeleteAsync("from Country"));
					await (t.CommitAsync());
				}
		}

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
