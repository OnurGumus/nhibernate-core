#if NET_4_5
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1408
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DetachedSubCriteriaTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			DetachedCriteria criteria = DetachedCriteria.For(typeof (DbResource));
			DetachedCriteria keyCriteria = criteria.CreateCriteria("keys");
			keyCriteria.Add(Restrictions.Eq("Key0", "2"));
			keyCriteria.Add(Restrictions.Eq("Key1", "en"));
			using (ISession session = OpenSession())
			{
				ICriteria icriteria = CriteriaTransformer.Clone(criteria).GetExecutableCriteria(session);
				icriteria.SetFirstResult(0);
				icriteria.SetMaxResults(1);
				await (icriteria.ListAsync<DbResource>());
			// should not throw when parse the criteria
			}
		}
	}
}
#endif
