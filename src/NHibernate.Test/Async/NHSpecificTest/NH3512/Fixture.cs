#if NET_4_5
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3512
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private int _id;
		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var employee = new Employee{Name = "Bob", Age = 33, Salary = 100};
					await (session.SaveAsync(employee));
					await (transaction.CommitAsync());
					_id = employee.Id;
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}
		}

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

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicUpdateOnAsync : FixtureAsync
	{
		protected override void Configure(Configuration configuration)
		{
			foreach (var mapping in configuration.ClassMappings)
			{
				mapping.DynamicUpdate = true;
			}
		}

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

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicUpdateOffAsync : FixtureAsync
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
