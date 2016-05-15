#if NET_4_5
using System;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Exceptions;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.Component.Basic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ComponentWithUniqueConstraintTests : TestCaseMappingByCode
	{
		[Test]
		public async Task CanBePersistedWithUniqueValuesAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Employee{HireDate = DateTime.Today, Person = new Person{Name = "Bill", Dob = new DateTime(2000, 1, 1)}};
					var e2 = new Employee{HireDate = DateTime.Today, Person = new Person{Name = "Hillary", Dob = new DateTime(2000, 1, 1)}};
					await (session.SaveAsync(e1));
					await (session.SaveAsync(e2));
					await (transaction.CommitAsync());
				}

			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var employees = session.Query<Employee>().ToList();
					Assert.That(employees.Count, Is.EqualTo(2));
					Assert.That(employees.Select(employee => employee.Person.Name).ToArray(), Is.EquivalentTo(new[]{"Hillary", "Bill"}));
				}
		}

		[Test]
		public Task CannotBePersistedWithNonUniqueValuesAsync()
		{
			try
			{
				CannotBePersistedWithNonUniqueValues();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
