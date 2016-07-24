#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2789
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
					await (session.SaveAsync(new EntityWithAByteValue{ByteValue = null}));
					await (session.SaveAsync(new EntityWithAByteValue{ByteValue = 1}));
					await (session.SaveAsync(new EntityWithAByteValue{ByteValue = 2}));
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
		public void EqualityOperator()
		{
			using (var session = OpenSession())
			{
				var query = (
					from e in session.Query<EntityWithAByteValue>()where e.ByteValue == 1
					select e).ToList();
				Assert.AreEqual(1, query.Count);
			}
		}

		[Test]
		public void EqualityOperatorNull()
		{
			using (var session = OpenSession())
			{
				var query = (
					from e in session.Query<EntityWithAByteValue>()where e.ByteValue == null
					select e).ToList();
				Assert.AreEqual(1, query.Count);
			}
		}

		[Test]
		public void EqualityOperatorNotNull()
		{
			using (var session = OpenSession())
			{
				var query = (
					from e in session.Query<EntityWithAByteValue>()where e.ByteValue != null
					select e).ToList();
				Assert.AreEqual(2, query.Count);
			}
		}
	}
}
#endif
