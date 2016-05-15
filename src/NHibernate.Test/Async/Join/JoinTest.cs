#if NET_4_5
using System.Collections.Generic;
using log4net;
using NHibernate.Criterion;
using NUnit.Framework;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Test.Join
{
	using NHibernate.Test.Subclass;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class JoinTest : TestCase
	{
		[Test]
		public async Task TestSequentialSelectsAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Employee mark = new Employee();
					mark.Name = "Mark";
					mark.Title = "internal sales";
					mark.Sex = 'M';
					mark.Address = "buckhead";
					mark.Zip = "30305";
					mark.Country = "USA";
					Customer joe = new Customer();
					joe.Name = "Joe";
					joe.Address = "San Francisco";
					joe.Zip = "54353";
					joe.Country = "USA";
					joe.Comments = "very demanding";
					joe.Sex = 'M';
					joe.Salesperson = mark;
					Person yomomma = new Person();
					yomomma.Name = "mom";
					yomomma.Sex = 'F';
					await (s.SaveAsync(yomomma));
					await (s.SaveAsync(mark));
					await (s.SaveAsync(joe));
					await (s.FlushAsync());
					s.Clear();
					Person p = await (s.GetAsync<Person>(yomomma.Id));
					Assert.AreEqual(yomomma.Name, p.Name);
					Assert.AreEqual(yomomma.Sex, p.Sex);
					s.Clear();
					// Copied from H3.  Don't really know what it is testing
					//Assert.AreEqual(0, s.CreateQuery("from System.Serializable").List().Count);
					Assert.AreEqual(3, (await (s.CreateQuery("from Person").ListAsync())).Count);
					Assert.AreEqual(1, (await (s.CreateQuery("from Person p where p.class is null").ListAsync())).Count);
					Assert.AreEqual(1, (await (s.CreateQuery("from Person p where p.class = Customer").ListAsync())).Count);
					Assert.AreEqual(1, (await (s.CreateQuery("from Customer c").ListAsync())).Count);
					s.Clear();
					IList customers = await (s.CreateQuery("from Customer c left join fetch c.Salesperson").ListAsync());
					foreach (Customer c in customers)
					{
						Assert.IsTrue(NHibernateUtil.IsInitialized(c.Salesperson));
						Assert.AreEqual("Mark", c.Salesperson.Name);
					}

					Assert.AreEqual(1, customers.Count);
					s.Clear();
					mark = (Employee)await (s.GetAsync(typeof (Employee), mark.Id));
					joe = (Customer)await (s.GetAsync(typeof (Customer), joe.Id));
					mark.Zip = "30306";
					await (s.FlushAsync());
					s.Clear();
					Assert.AreEqual(1, (await (s.CreateQuery("from Person p where p.Zip = '30306'").ListAsync())).Count);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestSequentialSelectsOptionalDataAsync()
		{
			// The "optional" attribute on <join/> does not yet work
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					User jesus = new User();
					jesus.Name = "Jesus Olvera y Martinez";
					jesus.Sex = 'M';
					await (s.SaveAsync(jesus));
					//objectsNeedDeleting.Add(jesus);
					Assert.AreEqual(1, (await (s.CreateQuery("from Person").ListAsync())).Count);
					Assert.AreEqual(0, (await (s.CreateQuery("from Person p where p.class is null").ListAsync())).Count);
					Assert.AreEqual(1, (await (s.CreateQuery("from Person p where p.class = User").ListAsync())).Count);
					Assert.AreEqual(1, (await (s.CreateQuery("from User u").ListAsync())).Count);
					s.Clear();
					await (ExecuteStatementAsync(s, tx, "delete from t_user"));
					s.Clear();
					// Clean up the test data
					await (s.DeleteAsync(jesus));
					await (s.FlushAsync());
					Assert.AreEqual(0, (await (s.CreateQuery("from Person").ListAsync())).Count);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestOptionalAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = CreatePerson("A guy");
					p.HomePhone = null;
					p.BusinessPhone = null;
					p.OthersPhones = null;
					await (s.SaveAsync(p));
					await (s.FlushAsync());
					s.Clear();
					DbCommand cmd = s.Connection.CreateCommand();
					tx.Enlist(cmd);
					cmd.CommandText = "select count(*) from phone where phone_id = " + p.Id.ToString();
					cmd.CommandType = CommandType.Text;
					Int64 count = Convert.ToInt64(await (cmd.ExecuteScalarAsync()));
					Assert.AreEqual(0, count);
					await (tx.CommitAsync());
				}
		}

		private async Task<Person> PreparePersonWithInverseJoinAsync(ISession s, ITransaction tx, string name, string stuffName)
		{
			Person p = CreatePerson(name);
			await (s.SaveAsync(p));
			await (s.FlushAsync());
			s.Clear();
			if (stuffName != null)
			{
				int count = await (ExecuteStatementAsync(s, tx, string.Format("insert into inversed_stuff (stuff_id, StuffName) values ({0}, '{1}')", p.Id, stuffName)));
				Assert.AreEqual(1, count, "Insert statement failed.");
			}

			return p;
		}

		[Test]
		public async Task TestInverseJoinSelectedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					string stuffName = "name of the stuff";
					Person p = await (PreparePersonWithInverseJoinAsync(s, tx, "John", stuffName));
					Person result = (Person)await (s.GetAsync(typeof (Person), p.Id));
					Assert.IsNotNull(result);
					Assert.AreEqual(stuffName, result.StuffName);
					await (ExecuteStatementAsync(s, tx, "delete from inversed_stuff"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestInverseJoinNotUpdatedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					string stuffName = "name of the stuff";
					Person p = await (PreparePersonWithInverseJoinAsync(s, tx, "John", stuffName));
					Person personToUpdate = (Person)await (s.GetAsync(typeof (Person), p.Id));
					Assert.IsNotNull(personToUpdate);
					personToUpdate.StuffName = "new stuff name";
					await (s.FlushAsync());
					s.Clear();
					Person loaded = (Person)await (s.GetAsync(typeof (Person), p.Id));
					Assert.AreEqual(stuffName, loaded.StuffName, "StuffName should not have been updated");
					await (ExecuteStatementAsync(s, tx, "delete from inversed_stuff"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestInverseJoinNotInsertedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person p = CreatePerson("John");
					p.StuffName = "stuff name in TestInverse_Select";
					await (s.SaveAsync(p));
					await (s.FlushAsync());
					s.Clear();
					Person result = (Person)await (s.GetAsync(typeof (Person), p.Id));
					Assert.IsNotNull(result);
					Assert.IsNull(result.StuffName);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestInverseJoinNotDeletedAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					string stuffName = "stuff not deleted";
					Person p = await (PreparePersonWithInverseJoinAsync(s, tx, "John", stuffName));
					long personId = p.Id;
					await (s.DeleteAsync(p));
					DbCommand cmd = s.Connection.CreateCommand();
					tx.Enlist(cmd);
					cmd.CommandText = string.Format("select count(stuff_id) from inversed_stuff where stuff_id = {0}", personId);
					cmd.CommandType = CommandType.Text;
					Int64 count = Convert.ToInt64(await (cmd.ExecuteScalarAsync()));
					Assert.AreEqual(1, count, "Row from an inverse <join> was deleted.");
					DbCommand cmd2 = s.Connection.CreateCommand();
					tx.Enlist(cmd2);
					cmd2.CommandText = string.Format("select StuffName from inversed_stuff where stuff_id = {0}", personId);
					cmd2.CommandType = CommandType.Text;
					string retrievedStuffName = (string)await (cmd2.ExecuteScalarAsync());
					Assert.AreEqual(stuffName, retrievedStuffName, "Retrieved inverse <join> does not match");
					await (ExecuteStatementAsync(s, tx, "delete from inversed_stuff"));
					await (tx.CommitAsync());
				}
		}

		private async Task<Person[]> CreateAndInsertPersonsAsync(ISession s, int count)
		{
			Person[] result = new Person[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = CreatePerson("Person " + i.ToString());
				await (s.SaveAsync(result[i]));
			}

			await (s.FlushAsync());
			s.Clear();
			return result;
		}

		[Test]
		public async Task TestRetrieveUsingGetAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					// Create a new person John
					Person john = CreatePerson("John");
					await (s.SaveAsync(john));
					await (s.FlushAsync());
					s.Clear();
					Person p = (Person)await (s.GetAsync(typeof (Person), john.Id));
					Assert.IsTrue(PersonsAreEqual(john, p));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestRetrieveUsingCriteriaInterfaceAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person[] people = await (CreateAndInsertPersonsAsync(s, 3));
					ICriteria criteria = s.CreateCriteria(typeof (Person)).Add(Expression.Eq("Name", people[1].Name));
					IList list = await (criteria.ListAsync());
					Assert.AreEqual(1, list.Count);
					Assert.IsTrue(PersonsAreEqual(people[1], (Person)list[0]));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestRetrieveUsingHqlAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person[] people = await (CreateAndInsertPersonsAsync(s, 3));
					IQuery query = s.CreateQuery("from Person p where p.Name = :name").SetParameter("name", people[1].Name);
					IList list = await (query.ListAsync());
					Assert.AreEqual(1, list.Count);
					Assert.IsTrue(PersonsAreEqual(people[1], (Person)list[0]));
					await (tx.CommitAsync());
				}
		}

		private async Task<Employee[]> CreateAndInsertEmployeesAsync(ISession s, int count)
		{
			Employee[] result = new Employee[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = CreateEmployee("Employee " + i.ToString(), "Title " + i.ToString());
				await (s.SaveAsync(result[i]));
			}

			await (s.FlushAsync());
			s.Clear();
			return result;
		}

		[Test]
		public async Task TestSimpleInsertAndRetrieveEmployeeAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					// Create a new employee Jack
					Employee jack = CreateEmployee("Jack", "Boss");
					await (s.SaveAsync(jack));
					await (s.FlushAsync());
					s.Clear();
					IList list = await (s.CreateQuery("from Employee p where p.Id = :id").SetParameter("id", jack.Id).ListAsync());
					Assert.AreEqual(1, list.Count);
					Assert.IsTrue(list[0] is Employee);
					Assert.IsTrue(EmployeesAreEqual(jack, (Employee)list[0]));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestDeleteUsingHqlAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person[] people = new Person[3];
					for (int i = 0; i < people.Length; i++)
					{
						people[i] = CreatePerson(string.Format("Person {0}", i + 1));
						await (s.SaveAsync(people[i]));
					}

					await (s.FlushAsync());
					s.Clear();
					await (s.DeleteAsync("from Person"));
					await (s.FlushAsync());
					IList list = await (s.CreateQuery("from Person").ListAsync());
					Assert.AreEqual(0, list.Count);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestUpdateEmployeeAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Employee[] employees = await (CreateAndInsertEmployeesAsync(s, 3));
					Employee emp0 = (Employee)await (s.GetAsync(typeof (Employee), employees[0].Id));
					Assert.IsNotNull(emp0);
					emp0.Address = "Address";
					emp0.BusinessPhone = "BusinessPhone";
					emp0.Country = "Country";
					emp0.HomePhone = "HomePhone";
					emp0.Manager = employees[2];
					emp0.Name = "Name";
					emp0.Salary = 20000;
					emp0.Title = "Title";
					emp0.Zip = "Zip";
					await (NHibernateUtil.InitializeAsync(emp0.Meetings));
					await (NHibernateUtil.InitializeAsync(emp0.OthersPhones));
					emp0.Meetings.Add(new Meeting{Employee = emp0, Description = "vacation def"});
					// Not updating emp0.Sex because it is marked update=false in the mapping file.
					await (s.FlushAsync());
					s.Clear();
					Employee emp0updated = (Employee)await (s.GetAsync(typeof (Employee), employees[0].Id));
					Assert.IsTrue(EmployeesAreEqual(emp0, emp0updated));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task Learn_SubclassBehaviorAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					SubclassOne one = new SubclassOne();
					one.TestDateTime = DateTime.Now;
					await (s.SaveAsync(one));
					await (s.FlushAsync());
					s.Clear();
					SubclassOne result = (SubclassOne)await (s.GetAsync(typeof (SubclassBase), one.Id));
					Assert.IsNotNull(result);
					Assert.IsTrue(result is SubclassOne);
					await (s.DeleteAsync(result));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task PolymorphicGetByTypeofSuperclassAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Employee[] employees = await (CreateAndInsertEmployeesAsync(s, 1));
					Employee emp0 = (Employee)await (s.GetAsync(typeof (Person), employees[0].Id));
					Assert.IsNotNull(emp0);
					Assert.IsTrue(emp0 is Employee);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
