#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Exceptions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2049
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture2049Async : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var p = new Person{Id = 1, Name = "Name"};
					await (session.SaveAsync(p));
					var ic = new IndividualCustomer{Deleted = false, Person = p, Id = 1};
					await (session.SaveAsync(ic));
					var deletedPerson = new Person{Id = 2, Name = "Name Deleted"};
					await (session.SaveAsync(deletedPerson));
					var deletedCustomer = new IndividualCustomer{Deleted = true, Person = deletedPerson, Id = 2};
					await (session.SaveAsync(deletedCustomer));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				await (session.DeleteAsync("from System.Object"));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task CanCriteriaQueryWithFilterOnJoinClassBaseClassPropertyAsync()
		{
			Assert.ThrowsAsync<GenericADOException>(async () =>
			{
				using (ISession session = OpenSession())
				{
					session.EnableFilter("DeletedCustomer").SetParameter("deleted", false);
					IList<Person> persons = await (session.CreateCriteria(typeof (Person)).ListAsync<Person>());
					Assert.That(persons, Has.Count.EqualTo(1));
					Assert.That(persons[0].Id, Is.EqualTo(1));
					Assert.That(persons[0].IndividualCustomer, Is.Not.Null);
					Assert.That(persons[0].IndividualCustomer.Id, Is.EqualTo(1));
					Assert.That(persons[0].IndividualCustomer.Deleted, Is.False);
				}
			}

			, KnownBug.Issue("NH-2049"));
		}

		[Test]
		public async Task CanHqlQueryWithFilterOnJoinClassBaseClassPropertyAsync()
		{
			Assert.ThrowsAsync<GenericADOException>(async () =>
			{
				using (ISession session = OpenSession())
				{
					session.EnableFilter("DeletedCustomer").SetParameter("deleted", false);
					var persons = await (session.CreateQuery("from Person as person left join person.IndividualCustomer as indCustomer").ListAsync<Person>());
					Assert.That(persons, Has.Count.EqualTo(1));
					Assert.That(persons[0].Id, Is.EqualTo(1));
					Assert.That(persons[0].IndividualCustomer, Is.Not.Null);
					Assert.That(persons[0].IndividualCustomer.Id, Is.EqualTo(1));
					Assert.That(persons[0].IndividualCustomer.Deleted, Is.False);
				}
			}

			, KnownBug.Issue("NH-2049"));
		}
	}
}
#endif
