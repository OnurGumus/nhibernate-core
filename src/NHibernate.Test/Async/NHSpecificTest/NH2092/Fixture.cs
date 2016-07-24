#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2092
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task ConstrainedLazyLoadedOneToOneUsingCastleProxyAsync()
		{
			try
			{
				using (var s = OpenSession())
				{
					var person = new Person{Id = 1, Name = "Person1"};
					var employee = new Employee{Id = 1, Name = "Emp1", Person = person};
					await (s.SaveAsync(person));
					await (s.SaveAsync(employee));
					await (s.FlushAsync());
				}

				using (var s = OpenSession())
				{
					var employee = await (s.GetAsync<Employee>(1));
					Assert.That(NHibernateUtil.IsInitialized(employee.Person), Is.False);
					Assert.That("Person1", Is.EqualTo(employee.Person.Name));
					Assert.That(NHibernateUtil.IsInitialized(employee.Person), Is.True);
				}
			}
			finally
			{
				using (var s = OpenSession())
				{
					await (s.DeleteAsync("from Employee"));
					await (s.DeleteAsync("from Person"));
					await (s.FlushAsync());
				}
			}
		}
	}
}
#endif
