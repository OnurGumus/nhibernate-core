#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CasingTestAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task ToUpperAsync()
		{
			var name = await ((
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToUpper()).SingleAsync());
			Assert.That(name, Is.EqualTo("NANCY"));
		}

		[Test]
		public async Task ToUpperInvariantAsync()
		{
			var name = await ((
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToUpperInvariant()).SingleAsync());
			Assert.That(name, Is.EqualTo("NANCY"));
		}

		[Test]
		public async Task ToLowerAsync()
		{
			var name = await ((
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToLower()).SingleAsync());
			Assert.That(name, Is.EqualTo("nancy"));
		}

		[Test]
		public async Task ToLowerInvariantAsync()
		{
			var name = await ((
				from e in db.Employees
				where e.EmployeeId == 1
				select e.FirstName.ToLowerInvariant()).SingleAsync());
			Assert.That(name, Is.EqualTo("nancy"));
		}
	}
}
#endif
