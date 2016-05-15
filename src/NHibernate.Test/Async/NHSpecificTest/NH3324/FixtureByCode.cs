#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3324
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureByCode : TestCaseMappingByCode
	{
		[Test]
		public async Task LeftOuterJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					const string hql = "FROM Entity e LEFT OUTER JOIN FETCH e.Children";
					var query = session.CreateQuery(hql);
					var result = await (query.ListAsync()); // does work
					Assert.That(result, Has.Count.GreaterThan(0));
				}
		}

		[Test]
		public async Task LeftOuterJoinSetMaxResultsAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					const string hql = "FROM Entity e LEFT OUTER JOIN FETCH e.Children";
					var query = session.CreateQuery(hql);
					query.SetMaxResults(5);
					var result = await (query.ListAsync()); // does not work
					Assert.That(result, Has.Count.GreaterThan(0));
				}
		}
	}
}
#endif
