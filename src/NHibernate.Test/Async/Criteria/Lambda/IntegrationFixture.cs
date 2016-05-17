#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.Criteria.Lambda
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IntegrationFixture : TestCase
	{
		[Test]
		public async Task DetachedQuery_SimpleCriterionAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person()
					{Name = "test person 1", Age = 20}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				QueryOver<Person> personQuery = QueryOver.Of<Person>().Where(p => p.Name == "test person 1");
				IList<Person> actual = personQuery.GetExecutableQueryOver(s).List();
				Assert.That(actual[0].Age, Is.EqualTo(20));
			}
		}

		[Test]
		public async Task FilterNullComponentAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var p1 = new Person()
					{Detail = new PersonDetail()
					{MaidenName = "test", Anniversary = new DateTime(2007, 06, 05)}};
					var p2 = new Person()
					{Detail = null};
					await (s.SaveAsync(p1));
					await (s.SaveAsync(p2));
					var nullDetails = s.QueryOver<Person>().Where(p => p.Detail == null).List();
					Assert.That(nullDetails.Count, Is.EqualTo(1));
					Assert.That(nullDetails[0].Id, Is.EqualTo(p2.Id));
				}
		}

		[Test]
		public async Task OnClauseAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person()
					{Name = "John"}.AddChild(new Child()
					{Nickname = "John"}).AddChild(new Child()
					{Nickname = "Judy"})));
					await (s.SaveAsync(new Person()
					{Name = "Jean"}));
					await (s.SaveAsync(new Child()
					{Nickname = "James"}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				Child childAlias = null;
				Person parentAlias = null;
				var children = s.QueryOver(() => childAlias).Left.JoinQueryOver(c => c.Parent, () => parentAlias, p => p.Name == childAlias.Nickname).WhereRestrictionOn(p => p.Name).IsNotNull.List();
				Assert.That(children, Has.Count.EqualTo(1));
			}

			using (ISession s = OpenSession())
			{
				Child childAlias = null;
				Person parentAlias = null;
				var parentNames = s.QueryOver(() => childAlias).Left.JoinAlias(c => c.Parent, () => parentAlias, p => p.Name == childAlias.Nickname).Select(c => parentAlias.Name).List<string>();
				Assert.That(parentNames.Count(n => !string.IsNullOrEmpty(n)), Is.EqualTo(1));
			}

			using (ISession s = OpenSession())
			{
				Person personAlias = null;
				Child childAlias = null;
				var people = s.QueryOver<Person>(() => personAlias).Left.JoinQueryOver(p => p.Children, () => childAlias, c => c.Nickname == personAlias.Name).WhereRestrictionOn(c => c.Nickname).IsNotNull.List();
				Assert.That(people, Has.Count.EqualTo(1));
			}

			using (ISession s = OpenSession())
			{
				Person personAlias = null;
				Child childAlias = null;
				var childNames = s.QueryOver<Person>(() => personAlias).Left.JoinAlias(p => p.Children, () => childAlias, c => c.Nickname == personAlias.Name).Select(p => childAlias.Nickname).List<string>();
				Assert.That(childNames.Count(n => !string.IsNullOrEmpty(n)), Is.EqualTo(1));
			}
		}

		[Test]
		public async Task UniqueResultAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person()
					{Name = "test person 1", Age = 20}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				Person actual = s.QueryOver<Person>().SingleOrDefault();
				Assert.That(actual.Name, Is.EqualTo("test person 1"));
			}

			using (ISession s = OpenSession())
			{
				string actual = s.QueryOver<Person>().Select(p => p.Name).SingleOrDefault<string>();
				Assert.That(actual, Is.EqualTo("test person 1"));
			}
		}

		[Test]
		public async Task IsTypeAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var father1 = new Person()
					{Name = "Father 1"};
					var father2 = new CustomPerson()
					{Name = "Father 2"};
					var person1 = new Person()
					{Name = "Person 1", Father = father2};
					var person2 = new CustomPerson()
					{Name = "Person 2", Father = father1};
					await (s.SaveAsync(father1));
					await (s.SaveAsync(father2));
					await (s.SaveAsync(person1));
					await (s.SaveAsync(person2));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				var actual = s.QueryOver<Person>().Where(p => p is CustomPerson).And(p => p.Father != null).List();
				Assert.That(actual.Count, Is.EqualTo(1));
				Assert.That(actual[0].Name, Is.EqualTo("Person 2"));
			}

			using (ISession s = OpenSession())
			{
				var actual = s.QueryOver<Person>().Where(p => p.GetType() == typeof (CustomPerson)).And(p => p.Father != null).List();
				Assert.That(actual.Count, Is.EqualTo(1));
				Assert.That(actual[0].Name, Is.EqualTo("Person 2"));
			}

			using (ISession s = OpenSession())
			{
				Person f = null;
				var actual = s.QueryOver<Person>().JoinAlias(p => p.Father, () => f).Where(() => f is CustomPerson).List();
				Assert.That(actual.Count, Is.EqualTo(1));
				Assert.That(actual[0].Name, Is.EqualTo("Person 1"));
			}

			using (ISession s = OpenSession())
			{
				Person f = null;
				var actual = s.QueryOver<Person>().JoinAlias(p => p.Father, () => f).Where(() => f.GetType() == typeof (CustomPerson)).List();
				Assert.That(actual.Count, Is.EqualTo(1));
				Assert.That(actual[0].Name, Is.EqualTo("Person 1"));
			}
		}

		[Test]
		public async Task OverrideEagerJoinAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Parent().AddChild(new JoinedChild()).AddChild(new JoinedChild())));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				var persons = s.QueryOver<Parent>().List();
				Assert.That(NHibernateUtil.IsInitialized(persons[0].Children), "Default query did not eagerly load children");
			}

			using (ISession s = OpenSession())
			{
				var persons = s.QueryOver<Parent>().Fetch(p => p.Children).Lazy.List();
				Assert.That(persons.Count, Is.EqualTo(1));
				Assert.That(!NHibernateUtil.IsInitialized(persons[0].Children), "Children not lazy loaded");
			}
		}

		[Test]
		public async Task RowCountAsync()
		{
			await (SetupPagingDataAsync());
			using (ISession s = OpenSession())
			{
				IQueryOver<Person> query = s.QueryOver<Person>().JoinQueryOver(p => p.Children).OrderBy(c => c.Age).Desc.Skip(2).Take(1);
				IList<Person> results = query.List();
				int rowCount = query.RowCount();
				object bigRowCount = query.RowCountInt64();
				Assert.That(results.Count, Is.EqualTo(1));
				Assert.That(results[0].Name, Is.EqualTo("Name 3"));
				Assert.That(rowCount, Is.EqualTo(4));
				Assert.That(bigRowCount, Is.TypeOf<long>());
				Assert.That(bigRowCount, Is.EqualTo(4));
			}
		}

		[Test]
		public async Task FunctionsOrderAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person()
					{Name = "p2", BirthDate = new DateTime(2008, 07, 06)}));
					await (s.SaveAsync(new Person()
					{Name = "p1", BirthDate = new DateTime(2009, 08, 07)}));
					await (s.SaveAsync(new Person()
					{Name = "p3", BirthDate = new DateTime(2007, 06, 05)}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					var persons = s.QueryOver<Person>().OrderBy(p => p.BirthDate.Year).Desc.List();
					Assert.That(persons.Count, Is.EqualTo(3));
					Assert.That(persons[0].Name, Is.EqualTo("p1"));
					Assert.That(persons[1].Name, Is.EqualTo("p2"));
					Assert.That(persons[2].Name, Is.EqualTo("p3"));
				}
		}

		[Test]
		public async Task MultiCriteriaAsync()
		{
			var driver = sessions.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);
			await (SetupPagingDataAsync());
			using (ISession s = OpenSession())
			{
				IQueryOver<Person> query = s.QueryOver<Person>().JoinQueryOver(p => p.Children).OrderBy(c => c.Age).Desc.Skip(2).Take(1);
				var multiCriteria = s.CreateMultiCriteria().Add("page", query).Add<int>("count", query.ToRowCountQuery());
				var pageResults = (IList<Person>)multiCriteria.GetResult("page");
				var countResults = (IList<int>)multiCriteria.GetResult("count");
				Assert.That(pageResults.Count, Is.EqualTo(1));
				Assert.That(pageResults[0].Name, Is.EqualTo("Name 3"));
				Assert.That(countResults.Count, Is.EqualTo(1));
				Assert.That(countResults[0], Is.EqualTo(4));
			}

			using (ISession s = OpenSession())
			{
				QueryOver<Person> query = QueryOver.Of<Person>().JoinQueryOver(p => p.Children).OrderBy(c => c.Age).Desc.Skip(2).Take(1);
				var multiCriteria = s.CreateMultiCriteria().Add("page", query).Add<int>("count", query.ToRowCountQuery());
				var pageResults = (IList<Person>)multiCriteria.GetResult("page");
				var countResults = (IList<int>)multiCriteria.GetResult("count");
				Assert.That(pageResults.Count, Is.EqualTo(1));
				Assert.That(pageResults[0].Name, Is.EqualTo("Name 3"));
				Assert.That(countResults.Count, Is.EqualTo(1));
				Assert.That(countResults[0], Is.EqualTo(4));
			}
		}

		private async Task SetupPagingDataAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person()
					{Name = "Name 1", Age = 1}.AddChild(new Child()
					{Nickname = "Name 1.1", Age = 1})));
					await (s.SaveAsync(new Person()
					{Name = "Name 2", Age = 2}.AddChild(new Child()
					{Nickname = "Name 2.1", Age = 3})));
					await (s.SaveAsync(new Person()
					{Name = "Name 3", Age = 3}.AddChild(new Child()
					{Nickname = "Name 3.1", Age = 2})));
					await (s.SaveAsync(new Person()
					{Name = "Name 4", Age = 4}.AddChild(new Child()
					{Nickname = "Name 4.1", Age = 4})));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task StatelessSessionAsync()
		{
			int personId;
			using (var ss = sessions.OpenStatelessSession())
				using (var t = ss.BeginTransaction())
				{
					var person = new Person{Name = "test1"};
					ss.Insert(person);
					personId = person.Id;
					await (t.CommitAsync());
				}

			using (var ss = sessions.OpenStatelessSession())
				using (ss.BeginTransaction())
				{
					var statelessPerson1 = ss.QueryOver<Person>().List()[0];
					Assert.That(statelessPerson1.Id, Is.EqualTo(personId));
					var statelessPerson2 = QueryOver.Of<Person>().GetExecutableQueryOver(ss).List()[0];
					Assert.That(statelessPerson2.Id, Is.EqualTo(personId));
				}
		}
	}
}
#endif
