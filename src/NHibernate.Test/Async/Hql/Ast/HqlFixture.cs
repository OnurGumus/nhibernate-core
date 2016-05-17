#if NET_4_5
using System;
using System.Linq;
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Engine.Query;
using NHibernate.Hql;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HqlFixture : BaseFixture
	{
		[Test]
		public async Task CaseClauseInSelectAsync()
		{
			// NH-322
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.SaveAsync(new Animal{BodyWeight = 12, Description = "Polliwog"}));
					await (s.Transaction.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				var l = s.CreateQuery("select a.id, case when a.description = 'Polliwog' then 2 else 0 end from Animal a").List();
				var element = (IList)l[0];
				Assert.That(element[1], Is.EqualTo(2));
				// work with alias
				l = s.CreateQuery("select a.id, case when a.description = 'Polliwog' then 2 else 0 end as value from Animal a").List();
				element = (IList)l[0];
				Assert.That(element[1], Is.EqualTo(2));
			}

			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					s.CreateQuery("delete from Animal").ExecuteUpdate();
					await (s.Transaction.CommitAsync());
				}
		}

		[Test]
		public async Task MultipleParametersInCaseStatementAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.SaveAsync(new Animal{BodyWeight = 12, Description = "Polliwog"}));
					await (s.Transaction.CommitAsync());
				}

			try
			{
				using (ISession s = OpenSession())
				{
					var result = await (s.CreateQuery("select case when 'b' = ? then 2 when 'b' = ? then 1 else 0 end from Animal a").SetParameter(0, "a").SetParameter(1, "b").SetMaxResults(1).UniqueResultAsync());
					Assert.AreEqual(1, result);
				}
			}
			finally
			{
				using (ISession s = OpenSession())
					using (s.BeginTransaction())
					{
						s.CreateQuery("delete from Animal").ExecuteUpdate();
						await (s.Transaction.CommitAsync());
					}
			}
		}

		[Test]
		public async Task ParameterInCaseThenClauseAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.SaveAsync(new Animal{BodyWeight = 12, Description = "Polliwog"}));
					await (s.Transaction.CommitAsync());
				}

			try
			{
				using (ISession s = OpenSession())
				{
					var result = await (s.CreateQuery("select case when 2=2 then ? else 0 end from Animal a").SetParameter(0, 1).UniqueResultAsync());
					Assert.AreEqual(1, result);
				}
			}
			finally
			{
				using (ISession s = OpenSession())
					using (s.BeginTransaction())
					{
						s.CreateQuery("delete from Animal").ExecuteUpdate();
						await (s.Transaction.CommitAsync());
					}
			}
		}

		[Test]
		public async Task ParameterInCaseThenAndElseClausesWithCastAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.SaveAsync(new Animal{BodyWeight = 12, Description = "Polliwog"}));
					await (s.Transaction.CommitAsync());
				}

			try
			{
				using (ISession s = OpenSession())
				{
					var result = await (s.CreateQuery("select case when 2=2 then cast(? as integer) else ? end from Animal a").SetParameter(0, 1).SetParameter(1, 0).UniqueResultAsync());
					Assert.AreEqual(1, result);
				}
			}
			finally
			{
				using (ISession s = OpenSession())
					using (s.BeginTransaction())
					{
						s.CreateQuery("delete from Animal").ExecuteUpdate();
						await (s.Transaction.CommitAsync());
					}
			}
		}

		[Test]
		public async Task SubselectAdditionAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.SaveAsync(new Animal{BodyWeight = 12, Description = "Polliwog"}));
					await (s.Transaction.CommitAsync());
				}

			try
			{
				using (ISession s = OpenSession())
				{
					var result = await (s.CreateQuery("select count(a) from Animal a where (select count(a2) from Animal a2) + 1 > 1").UniqueResultAsync());
					Assert.AreEqual(1, result);
				}
			}
			finally
			{
				using (ISession s = OpenSession())
					using (s.BeginTransaction())
					{
						s.CreateQuery("delete from Animal").ExecuteUpdate();
						await (s.Transaction.CommitAsync());
					}
			}
		}

		[Test, Ignore("Not fixed yet.")]
		public async Task SumShouldReturnDoubleAsync()
		{
			// NH-1734
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.SaveAsync(new Human{IntValue = 11, BodyWeight = 12.5f, Description = "Polliwog"}));
					await (s.Transaction.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				var l = s.CreateQuery("select sum(a.intValue * a.bodyWeight) from Animal a group by a.id").List();
				Assert.That(l[0], Is.InstanceOf<Double>());
			}

			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					s.CreateQuery("delete from Animal").ExecuteUpdate();
					await (s.Transaction.CommitAsync());
				}
		}

		[Test]
		public async Task InsertIntoFromSelect_WithSelectClauseParametersAsync()
		{
			using (ISession s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					// arrange
					await (s.SaveAsync(new Animal()
					{Description = "cat1", BodyWeight = 2.1f}));
					await (s.SaveAsync(new Animal()
					{Description = "cat2", BodyWeight = 2.5f}));
					await (s.SaveAsync(new Animal()
					{Description = "cat3", BodyWeight = 2.7f}));
					// act
					s.CreateQuery("insert into Animal (description, bodyWeight) select a.description, :weight from Animal a where a.bodyWeight < :weight").SetParameter<float>("weight", 5.7f).ExecuteUpdate();
					// assert
					Assert.AreEqual(3, await (s.CreateCriteria<Animal>().SetProjection(Projections.RowCount()).Add(Restrictions.Gt("bodyWeight", 5.5f)).UniqueResultAsync<int>()));
					s.CreateQuery("delete from Animal").ExecuteUpdate();
					await (s.Transaction.CommitAsync());
				}
			}
		}

		[Test]
		public async Task UnaryMinusBeforeParenthesesHandledCorrectlyAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction txn = s.BeginTransaction())
				{
					await (s.SaveAsync(new Animal{Description = "cat1", BodyWeight = 1}));
					// NH-2290: Unary minus before parentheses wasn't handled correctly (this query returned 0).
					int actual = s.CreateQuery("select -(1+1) from Animal as h").List<int>().Single();
					Assert.That(actual, Is.EqualTo(-2));
					// This was the workaround, which of course should still work.
					int actualWorkaround = s.CreateQuery("select -1*(1+1) from Animal as h").List<int>().Single();
					Assert.That(actualWorkaround, Is.EqualTo(-2));
				}
		}
	}
}
#endif
