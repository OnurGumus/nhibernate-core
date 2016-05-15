#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2404
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
