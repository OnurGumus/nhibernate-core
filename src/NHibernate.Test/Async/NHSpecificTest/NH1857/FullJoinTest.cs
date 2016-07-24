#if NET_4_5
using System;
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1857
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FullJoinTestAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Employee(1, "Employee1", new DateTime(1995, 1, 1));
					var e2 = new Employee(2, "Employee2", new DateTime(2007, 8, 1));
					var e3 = new Employee(3, "Employee3", new DateTime(2009, 5, 1));
					var d1 = new Department(1, "Department S");
					d1.AddEmployee(e1);
					d1.AddEmployee(e2);
					await (session.SaveOrUpdateAsync(d1));
					await (session.SaveOrUpdateAsync(e1));
					await (session.SaveOrUpdateAsync(e2));
					await (session.SaveOrUpdateAsync(e3));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from Employee").ExecuteUpdateAsync());
					await (session.CreateQuery("delete from Department").ExecuteUpdateAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			if (dialect is MySQLDialect)
				return false;
			if (dialect is InformixDialect)
				return false;
			if (dialect is SQLiteDialect)
				return false;
			return true;
		}

		[Test]
		public async Task TestFullJoinAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var q = session.CreateQuery("from Employee as e full join e.Department");
					var result = await (q.ListAsync());
					Assert.AreEqual(3, result.Count);
				}
		}
	}
}
#endif
