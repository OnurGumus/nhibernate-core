#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1654
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1654";
			}
		}

		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		[Test]
		public async Task TestAsync()
		{
			int employeeId;
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					var emp = new Employee();
					emp.Id = 1;
					emp.FirstName = "John";
					await (sess.SaveAsync(emp));
					await (tx.CommitAsync());
					employeeId = emp.Id;
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					var load = await (sess.LoadAsync<Employee>(employeeId));
					Assert.AreEqual("John", load.FirstNameFormula);
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.DeleteAsync("from Employee"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
