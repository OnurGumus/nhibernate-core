#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2812
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var entity = new EntityWithAByteValue{ByteValue = 1};
					await (session.SaveAsync(entity));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from EntityWithAByteValue"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task PerformingAQueryOnAByteColumnShouldNotThrowEqualityOperatorAsync()
		{
			using (var session = sessions.OpenSession())
			{
				var query = await ((
					from e in session.Query<EntityWithAByteValue>()where e.ByteValue == 1
					select e).ToListAsync());
				// this should not fail if fixed
				Assert.AreEqual(1, query.Count);
			}
		}

		[Test]
		public async Task PerformingAQueryOnAByteColumnShouldNotThrowEqualsAsync()
		{
			using (var session = sessions.OpenSession())
			{
				var query = await ((
					from e in session.Query<EntityWithAByteValue>()where e.ByteValue.Equals(1)select e).ToListAsync());
				Assert.AreEqual(1, query.Count);
			}
		}
	}
}
#endif
