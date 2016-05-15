#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH467
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task WhereClauseInManyToOneNavigationAsync()
		{
			User inactive = new User();
			inactive.Id = 10;
			inactive.Name = "inactive";
			inactive.IsActive = 0;
			Employee employee = new Employee();
			employee.Id = 20;
			employee.User = inactive;
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(inactive));
					await (s.SaveAsync(employee));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Employee loaded = (Employee)await (s.GetAsync(typeof (Employee), employee.Id));
					Assert.IsNotNull(loaded.User);
					try
					{
						await (NHibernateUtil.InitializeAsync(loaded.User));
						Assert.Fail("Should not have initialized");
					}
					catch (ObjectNotFoundException)
					{
					// Correct
					}

					await (s.DeleteAsync("from Employee"));
					await (s.DeleteAsync("from User"));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
