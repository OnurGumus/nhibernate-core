#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2898
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			cfg.Properties[Environment.CacheProvider] = typeof (BinaryFormatterCacheProvider).AssemblyQualifiedName;
			cfg.Properties[Environment.UseQueryCache] = "true";
			sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					for (var i = 0; i < 5; i++)
					{
						var obj = new ItemWithLazyProperty{Id = i + 1, Name = "Name #" + i, Description = "Description #" + i, };
						await (session.SaveAsync(obj));
					}

					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
			{
				await (session.DeleteAsync("from ItemWithLazyProperty"));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task SecondLevelCacheWithCriteriaQueriesAsync()
		{
			using (var session = OpenSession())
			{
				var list = await (session.CreateCriteria(typeof (ItemWithLazyProperty)).Add(Restrictions.Gt("Id", 2)).SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
				using (var cmd = session.Connection.CreateCommand())
				{
					cmd.CommandText = "DELETE FROM ItemWithLazyProperty";
					await (cmd.ExecuteNonQueryAsync());
				}
			}

			using (var session = OpenSession())
			{
				//should bring from cache
				var list = await (session.CreateCriteria(typeof (ItemWithLazyProperty)).Add(Restrictions.Gt("Id", 2)).SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
			}
		}

		[Test]
		public async Task SecondLevelCacheWithHqlQueriesAsync()
		{
			using (var session = OpenSession())
			{
				var list = await (session.CreateQuery("from ItemWithLazyProperty i where i.Id > 2").SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
				using (var cmd = session.Connection.CreateCommand())
				{
					cmd.CommandText = "DELETE FROM ItemWithLazyProperty";
					await (cmd.ExecuteNonQueryAsync());
				}
			}

			using (var session = OpenSession())
			{
				//should bring from cache
				var list = await (session.CreateQuery("from ItemWithLazyProperty i where i.Id > 2").SetCacheable(true).ListAsync());
				Assert.AreEqual(3, list.Count);
			}
		}
	}
}
#endif
