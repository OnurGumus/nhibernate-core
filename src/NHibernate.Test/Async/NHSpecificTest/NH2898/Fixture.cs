#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Properties;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2898
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
