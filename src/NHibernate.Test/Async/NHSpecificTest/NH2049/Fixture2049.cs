#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2049
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture2049 : BugTestCase
	{
		[Test]
		[KnownBug("Known bug NH-2049.")]
		public async Task CanCriteriaQueryWithFilterOnJoinClassBaseClassPropertyAsync()
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

		[Test]
		[KnownBug("Known bug NH-2049.", "NHibernate.Exceptions.GenericADOException")]
		public async Task CanHqlQueryWithFilterOnJoinClassBaseClassPropertyAsync()
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
	}
}
#endif
