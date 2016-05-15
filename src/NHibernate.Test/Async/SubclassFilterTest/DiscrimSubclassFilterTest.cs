#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.SubclassFilterTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DiscrimSubclassFilterTest : TestCase
	{
		[Test]
		public async Task FiltersWithSubclassAsync()
		{
			ISession s = OpenSession();
			s.EnableFilter("region").SetParameter("userRegion", "US");
			ITransaction t = s.BeginTransaction();
			await (PrepareTestDataAsync(s));
			s.Clear();
			IList results;
			results = await (s.CreateQuery("from Person").ListAsync());
			Assert.AreEqual(4, results.Count, "Incorrect qry result count");
			s.Clear();
			results = await (s.CreateQuery("from Employee").ListAsync());
			Assert.AreEqual(2, results.Count, "Incorrect qry result count");
			s.Clear();
			results = (await (s.CreateQuery("from Person as p left join fetch p.Minions").ListAsync<Person>())).Distinct().ToList();
			Assert.AreEqual(4, results.Count, "Incorrect qry result count");
			foreach (Person p in results)
			{
				// find john
				if (p.Name.Equals("John Doe"))
				{
					Employee john = (Employee)p;
					Assert.AreEqual(1, john.Minions.Count, "Incorrect fecthed minions count");
					break;
				}
			}

			s.Clear();
			results = (await (s.CreateQuery("from Employee as p left join fetch p.Minions").ListAsync<Employee>())).Distinct().ToList();
			Assert.AreEqual(2, results.Count, "Incorrect qry result count");
			foreach (Person p in results)
			{
				if (p.Name.Equals("John Doe"))
				{
					Employee john = (Employee)p;
					Assert.AreEqual(1, john.Minions.Count, "Incorrect fecthed minions count");
					break;
				}
			}

			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync("from Customer c where c.ContactOwner is not null"));
			await (s.DeleteAsync("from Employee e where e.Manager is not null"));
			await (s.DeleteAsync("from Person"));
			await (t.CommitAsync());
			s.Close();
		}

		private static async Task PrepareTestDataAsync(ISession s)
		{
			Employee john = new Employee("John Doe");
			john.Company = ("JBoss");
			john.Department = ("hr");
			john.Title = ("hr guru");
			john.Region = ("US");
			Employee polli = new Employee("Polli Wog");
			polli.Company = ("JBoss");
			polli.Department = ("hr");
			polli.Title = ("hr novice");
			polli.Region = ("US");
			polli.Manager = (john);
			john.Minions.Add(polli);
			Employee suzie = new Employee("Suzie Q");
			suzie.Company = ("JBoss");
			suzie.Department = ("hr");
			suzie.Title = ("hr novice");
			suzie.Region = ("EMEA");
			suzie.Manager = (john);
			john.Minions.Add(suzie);
			Customer cust = new Customer("John Q Public");
			cust.Company = ("Acme");
			cust.Region = ("US");
			cust.ContactOwner = (john);
			Person ups = new Person("UPS guy");
			ups.Company = ("UPS");
			ups.Region = ("US");
			await (s.SaveAsync(john));
			await (s.SaveAsync(cust));
			await (s.SaveAsync(ups));
			await (s.FlushAsync());
		}
	}
}
#endif
