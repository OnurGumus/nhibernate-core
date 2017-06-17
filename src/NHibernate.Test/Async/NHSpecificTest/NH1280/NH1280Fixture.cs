﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1280
{
	using System.Threading.Tasks;
	[TestFixture]
	public class NH1280FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "NH1280"; }
		}

		protected override void OnSetUp()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Person e1 = new Person("Joe", 10, 9);
				Person e2 = new Person("Sally", 20, 8);
				Person e3 = new Person("Tim", 20, 7); //20
				Person e4 = new Person("Fred", 40, 40);
				Person e5 = new Person("Fred", 50, 50);
				s.Save(e1);
				s.Save(e2);
				s.Save(e3);
				s.Save(e4);
				s.Save(e5);
				tx.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (ISession session = OpenSession())
			using (ITransaction tx = session.BeginTransaction())
			{
				session.Delete("from Person");
				tx.Commit();
			}
			base.OnTearDown();
		}

		[Test]
		public async Task HavingUsingSqlFunctions_ConcatAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				IList list = await (s.CreateCriteria(typeof(Person))
					.SetProjection(
						Projections.ProjectionList()
							.Add(
								new SqlFunctionProjection(
									"concat",
									NHibernateUtil.String,
									Projections.GroupProperty("Name"),
									new ConstantProjection(" "),
									Projections.GroupProperty("Name")))
							.Add(
								Projections.Conditional(
									Restrictions.IsNotNull(Projections.GroupProperty("Id")), new ConstantProjection("yes"), new ConstantProjection("No"))))
					.Add(Restrictions.Eq(Projections.GroupProperty("Name"), "Fred"))
					.Add(Restrictions.Gt("Id", 2))
					.ListAsync());

				Assert.AreEqual(2, list.Count);
				Assert.AreEqual("Fred Fred", ((object[])list[0])[0]);
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task HavingOnGtCountAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				//Find the iq that two people share
				int iq = await (s.CreateCriteria(typeof(Person))
					.SetProjection(Projections.GroupProperty("IQ"))
					.Add(Restrictions.Gt(Projections.Count("IQ"), 1))
					.UniqueResultAsync<int>());

				Assert.AreEqual(20, iq);
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task HavingOnLtAverageAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				//Find the iq that two people share
				string name = await (s.CreateCriteria(typeof(Person))
					.SetProjection(Projections.GroupProperty("Name"))
					.Add(Restrictions.Lt(Projections.Avg("IQ"), 20))
					.UniqueResultAsync<string>());

				Assert.AreEqual("Joe", name);
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task HavingOnEqProjectionAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				//SELECT this_.Name as y0_ FROM Person this_ GROUP BY this_.Name HAVING this_.Name = @p0; @p0 = 'Joe'
				string name = await (s.CreateCriteria(typeof(Person))
					.SetProjection(Projections.GroupProperty("Name"))
					.Add(Restrictions.Eq(Projections.GroupProperty("Name"), "Joe"))
					.UniqueResultAsync<string>());

				Assert.AreEqual("Joe", name);
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task NonHavingOnEqPropertyAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				string name = await (s.CreateCriteria(typeof(Person))
					.SetProjection(Projections.GroupProperty("Name"))
					.Add(Restrictions.EqProperty("IQ", "ShoeSize"))
					.UniqueResultAsync<string>());

				Assert.AreEqual("Fred", name);
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task NotExpressionShouldNotAddCriteriaTwiceAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				IList list = await (s.CreateCriteria(typeof(Person))
					.Add(Restrictions.Not(Restrictions.Eq(Projections.Property("IQ"), 40)))
					.Add(Restrictions.Eq(Projections.Property("Name"), "Fred"))
					.ListAsync());

				Assert.AreEqual(1, list.Count);
				Assert.AreEqual("Fred", ((Person)list[0]).Name);
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task MultipleSubqueriesShouldStayInOrderAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				DetachedCriteria dc1 = DetachedCriteria.For(typeof(Person))
					.Add(Property.ForName("IQ").Eq(10))
					.SetProjection(Property.ForName("Name"));

				DetachedCriteria dc2 = DetachedCriteria.For(typeof(Person))
					.Add(Property.ForName("ShoeSize").Eq(7))
					.SetProjection(Projections.Property("Name"));

				IList list = await (s.CreateCriteria(typeof(Person), "p")
					.Add(Subqueries.PropertyEq("Name", dc1))
					.Add(Restrictions.Not(Subqueries.Eq("Sally", dc2)))
					.ListAsync());

				Assert.AreEqual(1, list.Count);
				Assert.AreEqual("Joe", ((Person)list[0]).Name);
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task NestedSubqueriesShouldStayInOrderAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				DetachedCriteria dc1 = DetachedCriteria.For(typeof(Person))
					.Add(Property.ForName("IQ").Eq(40))
					.SetProjection(Property.ForName("IQ"));

				DetachedCriteria dc2 = DetachedCriteria.For(typeof(Person))
					.Add(Subqueries.PropertyEq("ShoeSize", dc1))
					.SetProjection(
						new SqlFunctionProjection(
							"concat",
							NHibernateUtil.String,
							Projections.GroupProperty("Name"),
							new ConstantProjection(" "),
							Projections.GroupProperty("Name")));

				IList list = await (s.CreateCriteria(typeof(Person))
					.Add(Subqueries.Eq("Fred Fred", dc2))
					.ListAsync());

				Assert.AreEqual(5, list.Count); //yeah, it returns all five results. The key is that it didn't crash
				await (tx.CommitAsync());
			}
		}

		[Test]
		public async Task SubstringShouldUseAllParametersAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				IList list = await (s.CreateCriteria(typeof(Person))
					.SetProjection(
						new SqlFunctionProjection(
							"LEFT",
							NHibernateUtil.String,
							Projections.Property("Name"),
							new ConstantProjection(3)))
					.Add(Restrictions.Eq(
						new SqlFunctionProjection(
							"substring",
							NHibernateUtil.String,
							Projections.Property("Name"),
							new ConstantProjection(1),
							new ConstantProjection(2)),
						"Fr"))
					.ListAsync());

				Assert.AreEqual(2, list.Count);
				Assert.AreEqual("Fre", list[0]);
				await (tx.CommitAsync());
			}
		}

		[Test, Description("NH-2863")]
		public async Task HavingOnNotExpressionCountAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				//Find the iq that two people share
				int iq = await (s.CreateCriteria(typeof(Person))
					.SetProjection(Projections.GroupProperty("IQ"))
					.Add(Restrictions.Not(Restrictions.Le(Projections.Count("IQ"), 1)))
					.UniqueResultAsync<int>());

				Assert.AreEqual(20, iq);
				await (tx.CommitAsync());
			}
		}
	}
}