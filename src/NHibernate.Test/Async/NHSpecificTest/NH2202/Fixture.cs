#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2202
{
	using System.Linq;
	using Criterion;
	using NUnit.Framework;

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var emp = new Employee()
					{EmployeeId = 1, NationalId = 1000};
					emp.Addresses.Add(new EmployeeAddress()
					{Employee = emp, Type = "Postal"});
					emp.Addresses.Add(new EmployeeAddress()
					{Employee = emp, Type = "Shipping"});
					await (s.SaveAsync(emp));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from EmployeeAddress"));
					await (tx.CommitAsync());
				}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Employee"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task CanProjectEmployeeFromAddressUsingCriteriaAsync()
		{
			using (var s = OpenSession())
			{
				var employees = await (s.CreateCriteria<EmployeeAddress>("x3").Add(Restrictions.Eq("Type", "Postal")).SetProjection(Projections.Property("Employee")).ListAsync<Employee>());
				Assert.That(employees.FirstOrDefault(), Is.InstanceOf<Employee>());
			}
		}
	}
}
#endif
