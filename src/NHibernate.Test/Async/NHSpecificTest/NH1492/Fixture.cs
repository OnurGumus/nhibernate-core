#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1492
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task RetrieveEntitiesAsync()
		{
			Entity eDel = new Entity(1, "DeletedEntity");
			eDel.Deleted = "Y";
			Entity eGood = new Entity(2, "GoodEntity");
			eGood.Childs.Add(new ChildEntity(eGood, "GoodEntityChild"));
			// Make "Deleted" entity persistent
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(eDel));
					await (s.SaveAsync(eGood));
					await (t.CommitAsync());
				}

			// Retrive (check if the entity was well persisted)
			IList<ChildEntity> childs;
			using (ISession s = OpenSession())
			{
				s.EnableFilter("excludeDeletedRows").SetParameter("deleted", "Y");
				IQuery q = s.CreateQuery("FROM ChildEntity c WHERE c.Parent.Code = :parentCode").SetParameter("parentCode", 2);
				childs = await (q.ListAsync<ChildEntity>());
			}

			Assert.AreEqual(1, childs.Count);
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Entity"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
