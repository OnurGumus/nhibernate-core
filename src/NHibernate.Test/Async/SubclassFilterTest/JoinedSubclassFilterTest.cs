﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NUnit.Framework;
using System.Linq;

namespace NHibernate.Test.SubclassFilterTest
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class JoinedSubclassFilterTestAsync : TestCase
	{
		protected override IList Mappings
		{
			get { return new string[] {"SubclassFilterTest.joined-subclass.hbm.xml"}; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

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

			foreach (Person p in  results)
			{
				// find john
				if (p.Name.Equals("John Doe"))
				{
					Employee john = (Employee) p;
					Assert.AreEqual(2, john.Minions.Count, "Incorrect fecthed minions count");
					break;
				}
			}
			s.Clear();

			// TODO : currently impossible to define a collection-level filter w/
			// joined-subclass elements that will filter based on a superclass
			// column and function correctly in (theta only?) outer joins;
			// this is consistent with the behaviour of a collection-level where.
			// this might be one argument for "pulling" the attached class-level
			// filters into collection assocations,
			// although we'd need some way to apply the appropriate alias in that
			// scenario.
			results = (await (s.CreateQuery("from Person as p left join fetch p.Minions").ListAsync<Person>())).Distinct().ToList();
			Assert.AreEqual(4, results.Count, "Incorrect qry result count");
			foreach (Person p in results)
			{
				if (p.Name.Equals("John Doe"))
				{
					Employee john = (Employee) p;
					Assert.AreEqual(2, john.Minions.Count, "Incorrect fecthed minions count");
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
					Employee john = (Employee) p;
					Assert.AreEqual(2, john.Minions.Count, "Incorrect fecthed minions count");
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

		private static async Task PrepareTestDataAsync(ISession s, CancellationToken cancellationToken = default(CancellationToken))
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

			await (s.SaveAsync(john, cancellationToken));
			await (s.SaveAsync(cust, cancellationToken));
			await (s.SaveAsync(ups, cancellationToken));

			await (s.FlushAsync(cancellationToken));
		}
	}
}