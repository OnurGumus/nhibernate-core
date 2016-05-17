#if NET_4_5
using System;
using System.Collections;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1507
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		private async Task CreateDataAsync()
		{
			//Employee
			var emp = new Employee{Address = "Zombie street", City = "Bitonto", PostalCode = "66666", FirstName = "tomb", LastName = "mutilated"};
			//and his related orders
			var order = new Order{OrderDate = DateTime.Now, Employee = emp, ShipAddress = "dead zone 1", ShipCountry = "Deadville"};
			var order2 = new Order{OrderDate = DateTime.Now, Employee = emp, ShipAddress = "dead zone 2", ShipCountry = "Deadville"};
			//Employee with no related orders but with same PostalCode
			var emp2 = new Employee{Address = "Gut street", City = "Mariotto", Country = "Arised", PostalCode = "66666", FirstName = "carcass", LastName = "purulent"};
			//Order with no related employee but with same ShipCountry
			var order3 = new Order{OrderDate = DateTime.Now, ShipAddress = "dead zone 2", ShipCountry = "Deadville"};
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(emp));
					await (session.SaveAsync(emp2));
					await (session.SaveAsync(order));
					await (session.SaveAsync(order2));
					await (session.SaveAsync(order3));
					await (tx.CommitAsync());
				}
			}
		}

		private async Task CleanupDataAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					//delete empolyee and related orders
					await (session.DeleteAsync("from Employee ee where ee.PostalCode = '66666'"));
					//delete order not related to employee
					await (session.DeleteAsync("from Order oo where oo.ShipCountry = 'Deadville'"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
