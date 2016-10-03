#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2317
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = sessions.OpenStatelessSession())
				using (var tx = session.BeginTransaction())
				{
					foreach (var artistName in new[]{"Foo", "Bar", "Baz", "Soz", "Tiz", "Fez"})
					{
						await (session.InsertAsync(new Artist{Name = artistName}));
					}

					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task QueryShouldWorkAsync()
		{
			using (var session = sessions.OpenSession())
				using (session.BeginTransaction())
				{
					// The HQL : "select a.id from Artist a where a in (from Artist take 3)"
					// shows how should look the HQL tree in the case where Skip/Take are not the last sentences.
					// When the query has no where-clauses the the HQL can be reduced to: "select a.id from Artist a take 3)"
					var expected = await (session.CreateQuery("select a.id from Artist a take 3").ListAsync<int>());
					var actual = session.Query<Artist>().Take(3).Select(a => a.Id).ToArray();
					Assert.That(actual, Is.EquivalentTo(expected));
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = sessions.OpenStatelessSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.CreateQuery("delete Artist").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}
	}
}
#endif
