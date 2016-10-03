#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2651
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = this.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var entity = new Model{Id = 1, SampleData = 1};
					await (session.SaveAsync(entity));
					var entity2 = new Model{Id = 2, SampleData = 2};
					await (session.SaveAsync(entity2));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = this.OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task TestConditionalProjectionWithConstantAndLikeExpressionAsync()
		{
			// Fails on Firebird since it's unable to determine the type of the
			// case expression from the parameters. See http://tracker.firebirdsql.org/browse/CORE-1821.
			// I don't want to mess up the test with cast statements that the DB really shouldn't need
			// (or the NHibernate dialect should add them just when needed).
			using (ISession session = this.OpenSession())
			{
				var projection = (Projections.Conditional(Restrictions.Eq("SampleData", 1), Projections.Constant("Foo"), Projections.Constant("Bar")));
				var likeExpression = new NHibernate.Criterion.LikeExpression(projection, "B", MatchMode.Start);
				var criteria1 = session.CreateCriteria<Model>().Add(Restrictions.Eq("Id", 1)).Add(likeExpression);
				var result1 = await (criteria1.UniqueResultAsync<Model>());
				var criteria2 = session.CreateCriteria<Model>().Add(Restrictions.Eq("Id", 2)).Add(likeExpression);
				var result2 = await (criteria2.UniqueResultAsync<Model>());
				Assert.IsNull(result1);
				Assert.IsNotNull(result2);
			}
		}
	}
}
#endif
