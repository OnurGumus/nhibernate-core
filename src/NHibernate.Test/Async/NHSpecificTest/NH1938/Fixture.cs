#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1938
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task Can_Query_By_Example_Case_InsensitiveAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new Person()
					{Name = "John Smith"}));
					Person examplePerson = new Person()
					{Name = "oHn"};
					IList<Person> matchingPeople;
					matchingPeople = s.CreateCriteria<Person>().Add(Example.Create(examplePerson).EnableLike(MatchMode.Anywhere).IgnoreCase()).List<Person>();
					Assert.That(matchingPeople.Count, Is.EqualTo(1));
					matchingPeople = s.CreateCriteria<Person>().Add(Example.Create(examplePerson).EnableLike(MatchMode.Anywhere)).List<Person>();
					Assert.That(matchingPeople.Count, Is.EqualTo(0));
					t.Rollback();
				}
		}
	}
}
#endif
