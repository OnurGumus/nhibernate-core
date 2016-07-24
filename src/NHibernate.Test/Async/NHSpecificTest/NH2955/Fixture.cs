#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2955
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var t = session.BeginTransaction())
				{
					var emp1 = new Employee{Id = 1, FirstName = "Nancy", LastName = "Davolio", Department = "IT"};
					var emp2 = new Employee{Id = 2, FirstName = "Andrew", LastName = "Fuller", Department = "Sales"};
					var emp3 = new Employee{Id = 3, FirstName = "Janet", LastName = "Leverling", Department = "IT"};
					var emp4 = new Employee{Id = 4, FirstName = "Margaret", LastName = "Peacock", Department = "IT"};
					var emp5 = new Employee{Id = 5, FirstName = "Steven", LastName = "Buchanan", Department = "Sales"};
					await (session.SaveAsync(emp1));
					await (session.SaveAsync(emp2));
					await (session.SaveAsync(emp3));
					await (session.SaveAsync(emp4));
					await (session.SaveAsync(emp5));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.DeleteAsync("FROM Employee"));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task EnumerableContainsAsync()
		{
			// ReSharper disable RedundantEnumerableCastCall
			var array = new[]{1, 3, 4}.OfType<int>();
			// ReSharper restore RedundantEnumerableCastCall
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var firstNames = await (session.CreateQuery("select e.FirstName from Employee e where e.Id in (:x)").SetParameterList("x", array).ListAsync<string>());
					Assert.AreEqual(3, firstNames.Count);
					Assert.AreEqual("Nancy", firstNames[0]);
					Assert.AreEqual("Janet", firstNames[1]);
					Assert.AreEqual("Margaret", firstNames[2]);
				}
		}

		[Test]
		public async Task GroupingContainsAsync()
		{
			var array = new[]{1, 3, 4}.ToLookup(x => 1).Single();
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var firstNames = await (session.CreateQuery("select e.FirstName from Employee e where e.Id in (:x)").SetParameterList("x", array).ListAsync<string>());
					Assert.AreEqual(3, firstNames.Count);
					Assert.AreEqual("Nancy", firstNames[0]);
					Assert.AreEqual("Janet", firstNames[1]);
					Assert.AreEqual("Margaret", firstNames[2]);
				}
		}
	}
}
#endif
