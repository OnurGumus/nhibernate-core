#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1394
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Person e1 = new Person("Joe", 10, 9);
					Person e2 = new Person("Sally", 100, 8);
					Person e3 = new Person("Tim", 20, 7); //20
					Person e4 = new Person("Fred", 40, 40);
					Person e5 = new Person("Mike", 50, 50);
					await (s.SaveAsync(e1));
					await (s.SaveAsync(e2));
					await (s.SaveAsync(e3));
					await (s.SaveAsync(e4));
					await (s.SaveAsync(e5));
					Pet p0 = new Pet("Fido", "Dog", 25, e1);
					Pet p1 = new Pet("Biff", "Dog", 9, e1);
					Pet p2 = new Pet("Pasha", "Dog", 25, e2);
					Pet p3 = new Pet("Lord", "Dog", 10, e2);
					Pet p4 = new Pet("Max", "Dog", 25, e3);
					Pet p5 = new Pet("Min", "Dog", 8, e3);
					await (s.SaveAsync(p0));
					await (s.SaveAsync(p1));
					await (s.SaveAsync(p2));
					await (s.SaveAsync(p3));
					await (s.SaveAsync(p4));
					await (s.SaveAsync(p5));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanOrderByPropertyProjectionAsync()
		{
			using (ISession s = OpenSession())
			{
				ICriteria c = s.CreateCriteria(typeof (Person)).AddOrder(Order.Desc(Projections.Property("IQ")));
				IList<Person> list = await (c.ListAsync<Person>());
				for (int i = 0; i < list.Count - 1; i++)
				{
					Assert.IsTrue(list[i].IQ >= list[i + 1].IQ);
				}
			}
		}

		[Test]
		public async Task CanOrderBySubqueryProjectionAsync()
		{
			using (ISession s = OpenSession())
			{
				using (new SqlLogSpy())
				{
					DetachedCriteria dc = DetachedCriteria.For<Person>("sub");
					dc.CreateCriteria("Pets", "pets").SetProjection(Projections.Min("pets.Weight")).Add(Restrictions.EqProperty("this.Id", "sub.Id"));
					ICriteria c = s.CreateCriteria(typeof (Person)).AddOrder(Order.Asc(Projections.SubQuery(dc)));
					Console.WriteLine("list()");
					IList<Person> list = await (c.ListAsync<Person>());
					int nullRelationOffSet = 2;
					if (Dialect is Oracle8iDialect || Dialect is PostgreSQLDialect)
					{
						// Oracle order NULL Last (ASC)
						nullRelationOffSet = 0;
					}

					Assert.AreEqual(list[nullRelationOffSet].Name, "Tim");
					Assert.AreEqual(list[nullRelationOffSet + 1].Name, "Joe");
					Assert.AreEqual(list[nullRelationOffSet + 2].Name, "Sally");
				}
			}
		}

		[Test]
		public async Task CanOrderBySubqueryProjectionDescAsync()
		{
			using (ISession s = OpenSession())
			{
				DetachedCriteria dc = DetachedCriteria.For<Person>("sub");
				dc.CreateCriteria("Pets", "pets").SetProjection(Projections.Min("pets.Weight")).Add(Restrictions.EqProperty("this.Id", "sub.Id"));
				ICriteria c = s.CreateCriteria(typeof (Person)).AddOrder(Order.Desc(Projections.SubQuery(dc)));
				IList<Person> list = await (c.ListAsync<Person>());
				int nullRelationOffSet = 0;
				if (Dialect is Oracle8iDialect || Dialect is PostgreSQLDialect)
				{
					// Oracle order NULL First (DESC)
					nullRelationOffSet = 2;
				}

				Assert.AreEqual(list[nullRelationOffSet + 2].Name, "Tim");
				Assert.AreEqual(list[nullRelationOffSet + 1].Name, "Joe");
				Assert.AreEqual(list[nullRelationOffSet].Name, "Sally");
			}
		}

		[Test]
		public async Task CanOrderBySqlProjectionAscAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).AddOrder(Order.Asc(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList<Person> list = await (c.ListAsync<Person>());
				for (int i = 0; i < list.Count - 1; i++)
				{
					Assert.IsTrue(list[i].IQ + list[i].ShoeSize <= list[i + 1].IQ + list[i + 1].ShoeSize);
				}
			}
		}

		[Test]
		public async Task CanOrderBySqlProjectionDescAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).AddOrder(Order.Desc(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList<Person> list = await (c.ListAsync<Person>());
				for (int i = 0; i < list.Count - 1; i++)
				{
					Assert.IsTrue(list[i].IQ + list[i].ShoeSize >= list[i + 1].IQ + list[i + 1].ShoeSize);
				}
			}
		}
	}
}
#endif
