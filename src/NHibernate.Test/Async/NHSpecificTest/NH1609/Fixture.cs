#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1609
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestAsync()
		{
			using (var session = sessions.OpenSession())
				using (session.BeginTransaction())
				{
					EntityA a1 = await (CreateEntityAAsync(session));
					EntityA a2 = await (CreateEntityAAsync(session));
					EntityC c = await (CreateEntityCAsync(session));
					EntityB b = await (CreateEntityBAsync(session, a1, c));
					// make sure the created entities are no longer in the session
					session.Clear();
					var multi = session.CreateMultiCriteria();
					// the first query is a simple select by id on EntityA
					multi.Add(session.CreateCriteria(typeof (EntityA)).Add(Restrictions.Eq("Id", a1.Id)));
					// the second query is also a simple select by id on EntityB
					multi.Add(session.CreateCriteria(typeof (EntityA)).Add(Restrictions.Eq("Id", a2.Id)));
					// the final query selects the first element (using SetFirstResult and SetMaxResults) for each EntityB where B.A.Id = a1.Id and B.C.Id = c.Id
					// the problem is that the paged query uses parameters @p0 and @p1 instead of @p2 and @p3
					multi.Add(session.CreateCriteria(typeof (EntityB)).Add(Restrictions.Eq("A.Id", a1.Id)).Add(Restrictions.Eq("C.Id", c.Id)).SetFirstResult(0).SetMaxResults(1));
					IList results = await (multi.ListAsync());
					Assert.AreEqual(1, ((IList)results[0]).Count);
					Assert.AreEqual(1, ((IList)results[1]).Count);
					Assert.AreEqual(1, ((IList)results[2]).Count);
				}
		}

		private async Task<EntityA> CreateEntityAAsync(ISession session)
		{
			var a = new EntityA();
			await (session.SaveAsync(a));
			await (session.FlushAsync());
			return a;
		}

		private async Task<EntityC> CreateEntityCAsync(ISession session)
		{
			var c = new EntityC();
			await (session.SaveAsync(c));
			await (session.FlushAsync());
			return c;
		}

		private async Task<EntityB> CreateEntityBAsync(ISession session, EntityA a, EntityC c)
		{
			var b = new EntityB{A = a, C = c};
			await (session.SaveAsync(b));
			await (session.FlushAsync());
			return b;
		}
	}
}
#endif
