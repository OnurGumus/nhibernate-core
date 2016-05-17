#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1291AnonExample
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1291AnonExampleFixture : BugTestCase
	{
		[Test]
		public async Task CanCreateAnonExampleForIntAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList list = s.CreateCriteria(typeof (Person)).Add(Example.Create(new PersonIQAnon(40))).List();
					//c# 3.5: Example.Create( new { IQ = 40 } )
					Assert.AreEqual(1, list.Count);
					Assert.AreEqual("Fred", ((Person)list[0]).Name);
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanCreateAnonExampleForStringLikeCompareAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList list = s.CreateCriteria(typeof (Person)).Add(Example.Create(new PersonNameAnon("%all%")).EnableLike()).List();
					//c# 3.5: Example.Create( new { Name = "%all%" } )
					Assert.AreEqual(1, list.Count);
					Assert.AreEqual("Sally", ((Person)list[0]).Name);
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanQueryUsingSavedRelationsAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<Person> people = s.CreateCriteria(typeof (Person)).List<Person>();
					Home h1 = new Home("Eugene", 97402);
					Home h2 = new Home("Klamath Falls", 97603);
					people[0].Home = h1;
					people[1].Home = h2;
					await (s.SaveAsync(h1));
					await (s.SaveAsync(h2));
					IList list = s.CreateCriteria(typeof (Person)).CreateCriteria("Home").Add(Example.Create(h1)).List();
					Assert.AreEqual(1, list.Count);
					Assert.AreEqual("Joe", ((Person)list[0]).Name);
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanQueryUsingAnonRelationsAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<Person> people = s.CreateCriteria(typeof (Person)).List<Person>();
					Home h1 = new Home("Eugene", 97402);
					Home h2 = new Home("Klamath Falls", 97603);
					people[0].Home = h1;
					people[1].Home = h2;
					await (s.SaveAsync(h1));
					await (s.SaveAsync(h2));
					IList list = s.CreateCriteria(typeof (Person)).CreateCriteria("Home").Add(Example.Create(new HomeAnon(97402))).List();
					//c# 3.5: Example.Create( new { Zip = 97402 } )
					Assert.AreEqual(1, list.Count);
					Assert.AreEqual("Joe", ((Person)list[0]).Name);
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
