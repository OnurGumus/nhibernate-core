#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Stat;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1001
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1001";
			}
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				cfg.SetProperty(Environment.GenerateStatistics, "true");
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		[Ignore("To be fixed")]
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
					emp.Department = dept;
					await (sess.SaveAsync(emp));
					await (tx.CommitAsync());
					employeeId = emp.Id;
				}

			await (ExecuteStatementAsync(string.Format("UPDATE EMPLOYEES SET DEPARTMENT_ID = 99999 WHERE EMPLOYEE_ID = {0}", employeeId)));
			IStatistics stat = sessions.Statistics;
			stat.Clear();
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.GetAsync<Employee>(employeeId));
					Assert.AreEqual(1, stat.PrepareStatementCount);
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
