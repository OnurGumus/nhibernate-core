#if NET_4_5
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1098
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FilterParameterOrderFixture : BugTestCase
	{
		[Test]
		public async Task CriteriaParameterOrderAsync()
		{
			ISession session = OpenSession();
			session.EnableFilter("EnabledObjects").SetParameter("Enabled", true);
			DetachedCriteria detached = DetachedCriteria.For(typeof (B), "b");
			detached.Add(Restrictions.LtProperty("a.ValueA", "b.ValueB")).Add(Restrictions.Gt("ValueB", 5)).SetProjection(Projections.Property("ValueB"));
			ICriteria crit = session.CreateCriteria(typeof (A), "a");
			crit.Add(Restrictions.Lt("ValueA", 6)).Add(Subqueries.Exists(detached));
			//
			// Query:
			// {select a0_.id as id0_, a0_.val_a as val2_0_, a0_.enabled as enabled0_ 
			//       from table_a a0_ 
			//       where a0_.enabled = ? and ((a0_.val_a<? )and
			//          (exists(select b1_.val_b 
			//                   from table_b b1_ 
			//                   where b1_.enabled = ? and 
			//                       ((a0_.val_a<b1_.val_b )and(b1_.val_b>? )))))}
			// 
			// Parameter:
			// 1) "this_.enabled = :EnabledObjects.Enabled" [filter #1]
			// 2) "this_.val_a < (?)" [positional #1]
			// 3) "this_0_.enabled = :EnabledObjects.Enabled" [filter #2]
			// 4) "this_0_.val_b > (?)" [positional #2]
			// 
			// => OK, parameter are in correct order: filter #1, pos #1, filter #2, pos #2
			//
			IList<A> result = await (crit.ListAsync<A>());
			Assert.AreEqual(1, result.Count);
		}

		[Test]
		public async Task QueryWithNamedParametersAsync()
		{
			ISession session = OpenSession();
			session.EnableFilter("EnabledObjects").SetParameter("Enabled", true);
			var sql = new StringBuilder();
			sql.Append("from A as a where a.ValueA < :ValA");
			sql.Append(" and exists (select b.ValueB from B as b where ");
			sql.Append(" a.ValueA < b.ValueB and b.ValueB > :ValB)");
			IQuery query = session.CreateQuery(sql.ToString());
			query.SetParameter("ValA", 6);
			query.SetParameter("ValB", 5);
			//
			// Query:
			// {select a0_.id as id0_, a0_.val_a as val2_0_, a0_.enabled as enabled0_ 
			//     from table_a a0_ 
			//     where a0_.enabled = ? and ((a0_.val_a<? )and
			//        (exists(select b1_.val_b 
			//            from table_b b1_ 
			//            where b1_.enabled = ? and ((a0_.val_a<b1_.val_b )and(b1_.val_b>? )))))}
			// 
			// Parameter:
			// 1) "this_.enabled = :EnabledObjects.Enabled" [filter #1]
			// 2) "this_.val_a < (?)" [named parameter #1]
			// 3) "this_0_.enabled = :EnabledObjects.Enabled" [filter #2]
			// 4) "this_0_.val_b > (?)" [named parameter #2]
			// 
			// => ERROR, parameters are in wrong order: filter #1, filter #2, named #1, named #2
			//
			IList<A> result = await (query.ListAsync<A>());
			Assert.AreEqual(1, result.Count);
		}

		[Test]
		public async Task QueryWithPositionalParameterAsync()
		{
			ISession session = OpenSession();
			session.EnableFilter("EnabledObjects").SetParameter("Enabled", true);
			var sql = new StringBuilder();
			sql.Append("from A as a where a.ValueA < ?");
			sql.Append(" and exists (select b.ValueB from B as b where ");
			sql.Append(" a.ValueA < b.ValueB and b.ValueB > ?)");
			IQuery query = session.CreateQuery(sql.ToString());
			query.SetInt32(0, 6);
			query.SetInt32(1, 5);
			//
			// Query:
			// {select a0_.id as id0_, a0_.val_a as val2_0_, a0_.enabled as enabled0_ 
			//     from table_a a0_ 
			//     where a0_.enabled = ? and ((a0_.val_a<? )and
			//        (exists(select b1_.val_b 
			//                  from table_b b1_ 
			//                  where b1_.enabled = ? and ((a0_.val_a<b1_.val_b )and(b1_.val_b>? )))))}
			// 
			// Parameter:
			// 1) "this_.enabled = :EnabledObjects.Enabled" [filter #1]
			// 2) "this_.val_a < (?)" [positional parameter #1]
			// 3) "this_0_.enabled = :EnabledObjects.Enabled" [filter #2]
			// 4) "this_0_.val_b > (?)" [positional parameter #2]
			// 
			// => OK, parameters are in correct order: filter #1, pos 12, filter #2, pos #2
			//
			IList<A> result = await (query.ListAsync<A>());
			Assert.AreEqual(1, result.Count);
		}

		[Test]
		public async Task QueryMapElementsAsync()
		{
			IQuery query = OpenSession().CreateQuery("from A a where a.C[:ValC] = :Text");
			query.SetInt32("ValC", 1);
			query.SetString("Text", "Text1");
			// Query:
			// {select a0_.id as id0_, a0_.val_a as val2_0_, a0_.enabled as enabled0_ 
			//         from table_a a0_, table_c c1_ 
			//         where (c1_.text = (?) and a0_.id=c1_.val_a and c1_.val_c = (?) ); }
			// Parameter:
			// 1) "c1_.text = (?)" [named parameter #2 Text]
			// 2) "c1_.val_c = (?)" [named parameter #1 ValC]
			//
			// => ERROR, parameters are in wrong order: named ValC, named Text
			var a = await (query.UniqueResultAsync<A>());
			Assert.AreEqual(a.C[1], "Text1");
		}
	}
}
#endif
