#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1727
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		/* To the commiter
         * I'm using sql2005dialect
         * From what I've read there's been some diffucalties with this
         * dialect before when used parameter queries.
         * The first test (xxx_DoesNotWorkToday) passed in NH 2.0
         * The second test passes where I've just switched the order in the where clause
          */
		[Test]
		public async Task VerifyFilterAndInAndProperty_DoesNotWorkTodayAsync()
		{
			const string hql = @"select a from ClassA a 
                                    where a.Value in (:aValues)
                                        and a.Name=:name";
			ClassB b = new ClassB();
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(b));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				s.EnableFilter("bEquals").SetParameter("b", b.Id);
				await (s.CreateQuery(hql).SetString("name", "Sweden").SetParameterList("aValues", new[]{1, 3, 4}).ListAsync<ClassA>());
			}
		}

		[Test]
		public async Task VerifyFilterAndInAndProperty_WorksTodayAsync()
		{
			const string hql = @"select a from ClassA a 
                                    where a.Name=:name
                                        and a.Value in (:aValues)";
			ClassB b = new ClassB();
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(b));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				s.EnableFilter("bEquals").SetParameter("b", b.Id);
				await (s.CreateQuery(hql).SetString("name", "Sweden").SetParameterList("aValues", new[]{1, 3, 4}).ListAsync<ClassA>());
			}
		}
	}
}
#endif
