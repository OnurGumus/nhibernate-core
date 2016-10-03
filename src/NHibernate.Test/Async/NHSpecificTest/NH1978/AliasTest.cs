#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1978
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AliasTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task ShouldReturnPlanFromEmployeeAsync()
		{
			using (var s = OpenSession())
				using (var trans = s.BeginTransaction())
				{
					var plan = new _401k{PlanName = "test"};
					await (s.SaveAsync(plan));
					await (s.FlushAsync());
					await (s.RefreshAsync(plan));
					var emp = new Employee{EmpName = "name", PlanParent = plan};
					await (s.SaveAsync(emp));
					trans.Rollback();
				}
		}
	}
}
#endif
