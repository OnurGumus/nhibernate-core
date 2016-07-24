#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1359
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1359";
			}
		}

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
					Person e2 = new Person("Sally", 20, 8);
					Person e3 = new Person("Tim", 20, 7); //20
					Person e4 = new Person("Fred", 40, 40);
					Person e5 = new Person("Fred", 50, 50);
					await (s.SaveAsync(e1));
					await (s.SaveAsync(e2));
					await (s.SaveAsync(e3));
					await (s.SaveAsync(e4));
					await (s.SaveAsync(e5));
					Pet p = new Pet("Fido", "Dog", 25, e1);
					Pet p2 = new Pet("Biff", "Dog", 10, e1);
					await (s.SaveAsync(p));
					await (s.SaveAsync(p2));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanSetSubQueryProjectionFromDetachedCriteriaWithCountProjectionAsync()
		{
			using (ISession s = OpenSession())
			{
				// This query doesn't make sense at all
				DetachedCriteria dc = DetachedCriteria.For<Person>().SetProjection(Projections.Count("Id"));
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.SubQuery(dc)).Add(Expression.Eq("Name", "Fred"));
				IList list = await (c.ListAsync());
				Assert.AreEqual(2, list.Count);
				foreach (object item in list)
				{
					Assert.AreEqual(5, item);
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class HeaviestPet
		{
			public string Name;
			public double Weight;
		}

		[Test]
		public async Task CanSubqueryRelatedObjectsNotInMainQueryAsync()
		{
			using (ISession s = OpenSession())
			{
				DetachedCriteria dc = DetachedCriteria.For<Person>().CreateCriteria("Pets", "pets").SetProjection(Projections.Max("pets.Weight"));
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.ProjectionList().Add(Projections.SubQuery(dc), "Weight").Add(Projections.Property("Name"), "Name")).Add(Restrictions.Eq("Name", "Joe"));
				c.SetResultTransformer(Transformers.AliasToBean(typeof (HeaviestPet)));
				IList<HeaviestPet> list = await (c.ListAsync<HeaviestPet>());
				Assert.AreEqual(1, list.Count);
				foreach (HeaviestPet pet in list)
				{
					Assert.AreEqual("Joe", pet.Name);
					Assert.AreEqual(25, pet.Weight);
				}
			}
		}

		[Test]
		public async Task CanGetSelectSubqueryWithSpecifiedParameterAsync()
		{
			using (ISession s = OpenSession())
			{
				DetachedCriteria dc = DetachedCriteria.For<Person>().Add(Restrictions.Eq("Name", "Joe")).SetProjection(Projections.Max("Name"));
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.ProjectionList().Add(Projections.SubQuery(dc), "Name")).Add(Restrictions.Eq("Name", "Joe"));
				c.SetResultTransformer(Transformers.AliasToBean(typeof (HeaviestPet)));
				IList<HeaviestPet> list = await (c.ListAsync<HeaviestPet>());
				Assert.AreEqual(1, list.Count);
				foreach (HeaviestPet pet in list)
				{
					Assert.AreEqual("Joe", pet.Name);
				}
			}
		}

		[Test]
		public async Task CanPageAndSortResultsWithParametersAndFiltersAsync()
		{
			using (ISession s = OpenSession())
			{
				s.EnableFilter("ExampleFilter").SetParameter("WeightVal", 100);
				DetachedCriteria dc = DetachedCriteria.For<Person>().CreateCriteria("Pets", "pets").SetProjection(Projections.Max("pets.Weight")).Add(Restrictions.Eq("pets.Weight", 10.0));
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.ProjectionList().Add(Projections.SubQuery(dc), "Weight").Add(Projections.Property("Name"), "Name")).Add(Restrictions.Eq("Name", "Joe"));
				c.SetResultTransformer(Transformers.AliasToBean(typeof (HeaviestPet)));
				c.SetMaxResults(1);
				c.AddOrder(new Order("Id", true));
				IList<HeaviestPet> list = await (c.ListAsync<HeaviestPet>());
				Assert.AreEqual(1, list.Count);
				foreach (HeaviestPet pet in list)
				{
					Assert.AreEqual("Joe", pet.Name);
					Assert.AreEqual(10.0, pet.Weight);
				}
			}
		}

		[Test]
		public async Task CanPageAndSortWithMultipleColumnsOfSameNameAsync()
		{
			using (ISession s = OpenSession())
			{
				ICriteria c = s.CreateCriteria(typeof (Person), "root").CreateCriteria("root.Pets", "pets").SetProjection(Projections.ProjectionList().Add(Projections.Property("root.Id"), "Id").Add(Projections.Property("root.Name"), "Name")).Add(Restrictions.Eq("Name", "Fido"));
				c.AddOrder(new Order("Id", true));
				c.SetResultTransformer(Transformers.AliasToBean(typeof (Person)));
				c.SetMaxResults(1);
				IList<Person> list = await (c.ListAsync<Person>());
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task CanOrderByNamedSubqueryAsync()
		{
			using (ISession s = OpenSession())
			{
				DetachedCriteria dc = DetachedCriteria.For<Person>().Add(Restrictions.Eq("Name", "Joe")).SetProjection(Projections.Max("Name"));
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.ProjectionList().Add(Projections.SubQuery(dc), "NameSubquery")).Add(Restrictions.Eq("Name", "Joe"));
				c.AddOrder(new Order("NameSubquery", true));
				c.SetMaxResults(1);
				IList list = await (c.ListAsync());
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
