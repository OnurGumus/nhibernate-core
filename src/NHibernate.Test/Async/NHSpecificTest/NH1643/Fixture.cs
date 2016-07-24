#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Stat;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1643
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1643";
			}
		}

		[Test]
		public async Task TestAsync()
		{
			int employeeId;
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					Department dept = new Department();
					dept.Id = 11;
					dept.Name = "Animal Testing";
					await (sess.SaveAsync(dept));
					Employee emp = new Employee();
					emp.Id = 1;
					emp.FirstName = "John";
					emp.LastName = "Doe";
					emp.Departments.Add(dept);
					await (sess.SaveAsync(emp));
					await (tx.CommitAsync());
					employeeId = emp.Id;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					var load = await (sess.LoadAsync<Employee>(employeeId));
					Assert.AreEqual(1, load.Departments.Count);
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.DeleteAsync("from Employee"));
					await (sess.DeleteAsync("from Department"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
