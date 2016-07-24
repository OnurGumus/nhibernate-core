#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2404
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
					await (session.SaveAsync(new TestEntity{Id = 1, Name = "Test Entity"}));
					await (session.SaveAsync(new TestEntity{Id = 2, Name = "Test Entity"}));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public void ProjectionsShouldWorkWithLinqProviderAndFutures()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query1 = (
						from entity in session.Query<TestEntity>()select new TestEntityDto{EntityId = entity.Id, EntityName = entity.Name}).ToList();
					Assert.AreEqual(2, query1.Count());
					var query2 = (
						from entity in session.Query<TestEntity>()select new TestEntityDto{EntityId = entity.Id, EntityName = entity.Name}).ToFuture();
					Assert.AreEqual(2, query2.Count());
				}
		}

		[Test]
		public async Task ProjectionsShouldWorkWithHqlAndFuturesAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var query1 = await (session.CreateQuery("select e.Id as EntityId, e.Name as EntityName from TestEntity e").SetResultTransformer(Transformers.AliasToBean(typeof (TestEntityDto))).ListAsync<TestEntityDto>());
					Assert.AreEqual(2, query1.Count());
					var query2 = await (session.CreateQuery("select e.Id as EntityId, e.Name as EntityName from TestEntity e").SetResultTransformer(Transformers.AliasToBean(typeof (TestEntityDto))).FutureAsync<TestEntityDto>());
					Assert.AreEqual(2, query2.Count());
				}
		}
	}
}
#endif
