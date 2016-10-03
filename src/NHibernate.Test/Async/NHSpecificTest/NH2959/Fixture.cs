#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2959
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver.SupportsMultipleQueries;
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new DerivedEntity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new AnotherDerivedEntity{Name = "Sally"};
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task CanUsePolymorphicCriteriaInMultiCriteriaAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var results = await (session.CreateMultiCriteria().Add(session.CreateCriteria(typeof (BaseEntity))).ListAsync());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0], Has.Count.EqualTo(2));
				}
		}

		[Test]
		public async Task CanUsePolymorphicQueryInMultiQueryAsync()
		{
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var results = await (session.CreateMultiQuery().Add(session.CreateQuery("from " + typeof (BaseEntity).FullName)).ListAsync());
					Assert.That(results, Has.Count.EqualTo(1));
					Assert.That(results[0], Has.Count.EqualTo(2));
				}
		}
	}
}
#endif
