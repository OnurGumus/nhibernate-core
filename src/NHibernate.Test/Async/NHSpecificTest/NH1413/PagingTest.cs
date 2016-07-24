#if NET_4_5
using System;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1413
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PagingTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BugAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction t = session.BeginTransaction())
				{
					await (session.PersistAsync(new Foo("Foo1", DateTime.Today.AddDays(5))));
					await (session.PersistAsync(new Foo("Foo2", DateTime.Today.AddDays(1))));
					await (session.PersistAsync(new Foo("Foo3", DateTime.Today.AddDays(3))));
					await (t.CommitAsync());
				}

			DetachedCriteria criteria = DetachedCriteria.For(typeof (Foo));
			criteria.Add(Restrictions.Like("Name", "Foo", MatchMode.Start));
			criteria.AddOrder(Order.Desc("Name"));
			criteria.AddOrder(Order.Asc("BirthDate"));
			using (ISession session = OpenSession())
			{
				ICriteria icriteria = criteria.GetExecutableCriteria(session);
				icriteria.SetFirstResult(0);
				icriteria.SetMaxResults(2);
				Assert.That(2, Is.EqualTo((await (icriteria.ListAsync<Foo>())).Count));
			}

			using (ISession session = OpenSession())
				using (ITransaction t = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Foo"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
