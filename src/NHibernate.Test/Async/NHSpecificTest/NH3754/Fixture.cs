#if NET_4_5
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3754
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH3754";
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class TestEntity
		{
			public string Name
			{
				get;
				set;
			}
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			cfg.Properties[Environment.CacheProvider] = typeof (HashtableCacheProvider).AssemblyQualifiedName;
			cfg.Properties[Environment.UseQueryCache] = "true";
		}

		[Test]
		public async Task SecondLevelCacheWithResultTransformerAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction t = session.BeginTransaction())
				{
					User user = new User();
					user.Name = "Test";
					user.Id = 1;
					await (session.SaveAsync(user));
					await (session.FlushAsync());
					session.Clear();
					var list = await (session.CreateCriteria<User>().SetProjection(Projections.Property<User>(x => x.Name).As("Name")).SetResultTransformer(new AliasToBeanResultTransformer(typeof (TestEntity))).SetCacheable(false).ListAsync<TestEntity>());
					Assert.AreEqual(1, list.Count);
					Assert.AreEqual("Test", list[0].Name);
					await (session.DeleteAsync("from User"));
					await (t.CommitAsync());
				}
			}
		}
	}
}
#endif
