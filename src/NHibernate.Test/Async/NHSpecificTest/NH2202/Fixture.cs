#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2202
{
	using System.Linq;
	using Criterion;
	using NUnit.Framework;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
