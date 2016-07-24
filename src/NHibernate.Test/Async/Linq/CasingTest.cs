#if NET_4_5
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CasingTestAsync : LinqTestCaseAsync
	{
		[Test]
		public void ToUpper()
		{
			var name = (
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToUpper()).Single();
			Assert.That(name, Is.EqualTo("NANCY"));
		}

		[Test]
		public void ToUpperInvariant()
		{
			var name = (
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToUpperInvariant()).Single();
			Assert.That(name, Is.EqualTo("NANCY"));
		}

		[Test]
		public void ToLower()
		{
			var name = (
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToLower()).Single();
			Assert.That(name, Is.EqualTo("nancy"));
		}

		[Test]
		public void ToLowerInvariant()
		{
			var name = (
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToLowerInvariant()).Single();
			Assert.That(name, Is.EqualTo("nancy"));
		}
	}
}
#endif
