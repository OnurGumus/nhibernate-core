#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2317
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
	}
}
#endif
