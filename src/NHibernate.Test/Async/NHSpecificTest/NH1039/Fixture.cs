#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1039
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1039";
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task testAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person person = new Person("1");
					person.Name = "John Doe";
					var set = new HashSet<object>();
					set.Add("555-1234");
					set.Add("555-4321");
					person.Properties.Add("Phones", set);
					await (s.SaveAsync(person));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person person = (Person)await (s.CreateCriteria(typeof (Person)).UniqueResultAsync());
					Assert.AreEqual("1", person.ID);
					Assert.AreEqual("John Doe", person.Name);
					Assert.AreEqual(1, person.Properties.Count);
					Assert.That(person.Properties["Phones"], Is.InstanceOf<ISet<object>>());
					Assert.IsTrue(((ISet<object>)person.Properties["Phones"]).Contains("555-1234"));
					Assert.IsTrue(((ISet<object>)person.Properties["Phones"]).Contains("555-4321"));
				}
		}
	}
}
#endif
