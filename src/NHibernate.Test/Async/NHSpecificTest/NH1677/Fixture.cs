#if NET_4_5
using System.Collections;
using System.Data;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1677
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityModeMapCriteria : BugTestCase
	{
		[Test]
		public async Task EntityModeMapFailsWithCriteriaAsync()
		{
			using (ISessionFactory sf = cfg.BuildSessionFactory())
			{
				using (ISession s = sf.OpenSession())
				{
					IQuery query = s.CreateQuery(string.Format("from {0}", Entity1Name));
					IList entity1List = await (query.ListAsync());
					Assert.AreEqual(NumberOfRecordPerEntity, entity1List.Count); // OK, Count == 10
				}

				using (ISession s = sf.OpenSession())
				{
					ICriteria entity1Criteria = s.CreateCriteria(Entity1Name);
					IList entity1List = await (entity1Criteria.ListAsync());
					Assert.AreEqual(NumberOfRecordPerEntity, entity1List.Count); // KO !!! Count == 20 !!!
				}
			}
		}
	}
}
#endif
