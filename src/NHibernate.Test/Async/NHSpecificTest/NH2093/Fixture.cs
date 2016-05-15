#if NET_4_5
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2093
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task NHibernateProxyHelperReturnsCorrectTypeAsync()
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
					var person = await (s.LoadAsync<Person>(1));
					var type = await (NHibernateProxyHelper.GuessClassAsync(person));
					Assert.AreEqual(type, typeof (Person));
				}

				using (var s = OpenSession())
				{
					var person = await (s.GetAsync<Person>(1));
					var type = await (NHibernateProxyHelper.GuessClassAsync(person));
					Assert.AreEqual(type, typeof (Person));
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

		[Test]
		public async Task CanUseFieldInterceptingProxyAsHQLArgumentAsync()
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
					var person = await (s.GetAsync<Person>(1));
					var list = await ((await (s.CreateQuery("from Employee where Person = :p").SetEntityAsync("p", person))).ListAsync<Employee>());
					Assert.AreEqual(list.Count, 1);
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
