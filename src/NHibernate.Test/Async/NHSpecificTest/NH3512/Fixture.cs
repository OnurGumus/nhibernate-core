#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3512
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		protected async Task UpdateBaseEntityAsync()
		{
			using (ISession session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var person = await (session.GetAsync<Person>(_id));
					var before = person.Version;
					person.Age++;
					await (transaction.CommitAsync());
					Assert.That(person.Version, Is.GreaterThan(before));
				}
		}

		protected async Task UpdateDerivedEntityAsync()
		{
			using (ISession session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var employee = await (session.GetAsync<Employee>(_id));
					var before = employee.Version;
					employee.Salary += 10;
					await (transaction.CommitAsync());
					Assert.That(employee.Version, Is.GreaterThan(before));
				}
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicUpdateOn : Fixture
	{
		[Test]
		public async Task ShouldChangeVersionWhenBasePropertyChangedAsync()
		{
			await (UpdateBaseEntityAsync());
		}

		[Test]
		public async Task ShouldChangeVersionWhenDerivedPropertyChangedAsync()
		{
			await (UpdateDerivedEntityAsync());
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicUpdateOff : Fixture
	{
		[Test]
		public async Task ShouldChangeVersionWhenBasePropertyChangedAsync()
		{
			await (UpdateBaseEntityAsync());
		}

		[Test]
		public async Task ShouldChangeVersionWhenDerivedPropertyChangedAsync()
		{
			await (UpdateDerivedEntityAsync());
		}
	}
}
#endif
