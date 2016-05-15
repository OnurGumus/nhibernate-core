#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Transaction;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Component.Basic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ComponentTest : TestCase
	{
		[Test]
		public async Task TestUpdateFalseAsync()
		{
			User u;
			sessions.Statistics.Clear();
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					u = new User("gavin", "secret", new Person("Gavin King", new DateTime(1999, 12, 31), "Karbarook Ave"));
					await (s.PersistAsync(u));
					await (s.FlushAsync());
					u.Person.Name = "XXXXYYYYY";
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(sessions.Statistics.EntityInsertCount, Is.EqualTo(1));
			Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(0));
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					u = (User)await (s.GetAsync(typeof (User), "gavin"));
					Assert.That(u.Person.Name, Is.EqualTo("Gavin King"));
					await (s.DeleteAsync(u));
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(sessions.Statistics.EntityDeleteCount, Is.EqualTo(1));
		}

		[Test]
		public async Task TestComponentAsync()
		{
			User u;
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					u = new User("gavin", "secret", new Person("Gavin King", new DateTime(1999, 12, 31), "Karbarook Ave"));
					await (s.PersistAsync(u));
					await (s.FlushAsync());
					u.Person.ChangeAddress("Phipps Place");
					await (t.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					u = (User)await (s.GetAsync(typeof (User), "gavin"));
					Assert.That(u.Person.Address, Is.EqualTo("Phipps Place"));
					Assert.That(u.Person.PreviousAddress, Is.EqualTo("Karbarook Ave"));
					Assert.That(u.Person.Yob, Is.EqualTo(u.Person.Dob.Year));
					u.Password = "$ecret";
					await (t.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					u = (User)await (s.GetAsync(typeof (User), "gavin"));
					Assert.That(u.Person.Address, Is.EqualTo("Phipps Place"));
					Assert.That(u.Person.PreviousAddress, Is.EqualTo("Karbarook Ave"));
					Assert.That(u.Password, Is.EqualTo("$ecret"));
					await (s.DeleteAsync(u));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task TestComponentStateChangeAndDirtinessAsync()
		{
			// test for HHH-2366
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					User u = new User("steve", "hibernater", new Person("Steve Ebersole", new DateTime(1999, 12, 31), "Main St"));
					await (s.PersistAsync(u));
					await (s.FlushAsync());
					long intialUpdateCount = sessions.Statistics.EntityUpdateCount;
					u.Person.Address = "Austin";
					await (s.FlushAsync());
					Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(intialUpdateCount + 1));
					intialUpdateCount = sessions.Statistics.EntityUpdateCount;
					u.Person.Address = "Cedar Park";
					await (s.FlushAsync());
					Assert.That(sessions.Statistics.EntityUpdateCount, Is.EqualTo(intialUpdateCount + 1));
					await (s.DeleteAsync(u));
					await (t.CommitAsync());
					s.Close();
				}
		}

		[Test]
		[Ignore("Ported from Hibernate. Read properties not supported in NH yet.")]
		public async Task TestCustomColumnReadAndWriteAsync()
		{
			const double HEIGHT_INCHES = 73;
			const double HEIGHT_CENTIMETERS = HEIGHT_INCHES * 2.54d;
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					User u = new User("steve", "hibernater", new Person("Steve Ebersole", new DateTime(1999, 12, 31), "Main St"));
					u.Person.HeightInches = HEIGHT_INCHES;
					await (s.PersistAsync(u));
					await (s.FlushAsync());
					// Test value conversion during insert
					double heightViaSql = (double)await (s.CreateSQLQuery("select height_centimeters from t_user where t_user.username='steve'").UniqueResultAsync());
					Assert.That(heightViaSql, Is.EqualTo(HEIGHT_CENTIMETERS).Within(0.01d));
					// Test projection
					double heightViaHql = (double)await (s.CreateQuery("select u.Person.HeightInches from User u where u.Id = 'steve'").UniqueResultAsync());
					Assert.That(heightViaHql, Is.EqualTo(HEIGHT_INCHES).Within(0.01d));
					// Test restriction and entity load via criteria
					u = (User)await (s.CreateCriteria(typeof (User)).Add(Restrictions.Between("Person.HeightInches", HEIGHT_INCHES - 0.01d, HEIGHT_INCHES + 0.01d)).UniqueResultAsync());
					Assert.That(u.Person.HeightInches, Is.EqualTo(HEIGHT_INCHES).Within(0.01d));
					// Test predicate and entity load via HQL
					u = (User)await (s.CreateQuery("from User u where u.Person.HeightInches between ? and ?").SetDouble(0, HEIGHT_INCHES - 0.01d).SetDouble(1, HEIGHT_INCHES + 0.01d).UniqueResultAsync());
					Assert.That(u.Person.HeightInches, Is.EqualTo(HEIGHT_INCHES).Within(0.01d));
					// Test update
					u.Person.HeightInches = 1;
					await (s.FlushAsync());
					heightViaSql = (double)await (s.CreateSQLQuery("select height_centimeters from t_user where t_user.username='steve'").UniqueResultAsync());
					Assert.That(heightViaSql, Is.EqualTo(2.54d).Within(0.01d));
					await (s.DeleteAsync(u));
					await (t.CommitAsync());
					s.Close();
				}
		}

		[Test]
		[Ignore("Ported from Hibernate - failing in NH")]
		public async Task TestComponentQueriesAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					Employee emp = new Employee();
					emp.HireDate = new DateTime(1999, 12, 31);
					emp.Person = new Person();
					emp.Person.Name = "steve";
					emp.Person.Dob = new DateTime(1999, 12, 31);
					await (s.SaveAsync(emp));
					await (s.CreateQuery("from Employee e where e.Person = :p and 1=1 and 2=2").SetParameter("p", emp.Person).ListAsync());
					await (s.CreateQuery("from Employee e where :p = e.Person").SetParameter("p", emp.Person).ListAsync());
					await (s.CreateQuery("from Employee e where e.Person = ('steve', current_timestamp)").ListAsync());
					await (s.DeleteAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}
		}

		[Test]
		public async Task TestComponentFormulaQueryAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.CreateQuery("from User u where u.Person.Yob = 1999").ListAsync());
					await (s.CreateCriteria(typeof (User)).Add(Property.ForName("Person.Yob").Between(1999, 2002)).ListAsync());
					if (Dialect.SupportsRowValueConstructorSyntax)
					{
						await (s.CreateQuery("from User u where u.Person = ('gavin', :dob, 'Peachtree Rd', 'Karbarook Ave', 1974, 'Peachtree Rd')").SetDateTime("dob", new DateTime(1974, 3, 25)).ListAsync());
						await (s.CreateQuery("from User where Person = ('gavin', :dob, 'Peachtree Rd', 'Karbarook Ave', 1974, 'Peachtree Rd')").SetDateTime("dob", new DateTime(1974, 3, 25)).ListAsync());
					}

					await (t.CommitAsync());
					s.Close();
				}
		}

		[Test]
		public async Task TestNamedQueryAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.GetNamedQuery("userNameIn").SetParameterList("nameList", new object[]{"1ovthafew", "turin", "xam"}).ListAsync());
					await (t.CommitAsync());
					s.Close();
				}
		}

		[Test]
		public async Task TestMergeComponentAsync()
		{
			Employee emp = null;
			IEnumerator<Employee> enumerator = null;
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = new Employee();
					emp.HireDate = new DateTime(1999, 12, 31);
					emp.Person = new Person();
					emp.Person.Name = "steve";
					emp.Person.Dob = new DateTime(1999, 12, 31);
					await (s.PersistAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.GetAsync(typeof (Employee), emp.Id));
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(emp.OptionalComponent, Is.Null);
			emp.OptionalComponent = new OptionalComponent();
			emp.OptionalComponent.Value1 = "emp-value1";
			emp.OptionalComponent.Value2 = "emp-value2";
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.MergeAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.GetAsync(typeof (Employee), emp.Id));
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(emp.OptionalComponent.Value1, Is.EqualTo("emp-value1"));
			Assert.That(emp.OptionalComponent.Value2, Is.EqualTo("emp-value2"));
			emp.OptionalComponent.Value1 = null;
			emp.OptionalComponent.Value2 = null;
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.MergeAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.GetAsync(typeof (Employee), emp.Id));
					await (NHibernateUtil.InitializeAsync(emp.DirectReports));
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(emp.OptionalComponent, Is.Null);
			Employee emp1 = new Employee();
			emp1.HireDate = new DateTime(1999, 12, 31);
			emp1.Person = new Person();
			emp1.Person.Name = "bozo";
			emp1.Person.Dob = new DateTime(1999, 12, 31);
			emp.DirectReports.Add(emp1);
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.MergeAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.GetAsync(typeof (Employee), emp.Id));
					await (NHibernateUtil.InitializeAsync(emp.DirectReports));
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(emp.DirectReports.Count, Is.EqualTo(1));
			enumerator = emp.DirectReports.GetEnumerator();
			enumerator.MoveNext();
			emp1 = (Employee)enumerator.Current;
			Assert.That(emp1.OptionalComponent, Is.Null);
			emp1.OptionalComponent = new OptionalComponent();
			emp1.OptionalComponent.Value1 = "emp1-value1";
			emp1.OptionalComponent.Value2 = "emp1-value2";
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.MergeAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.GetAsync(typeof (Employee), emp.Id));
					await (NHibernateUtil.InitializeAsync(emp.DirectReports));
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(emp.DirectReports.Count, Is.EqualTo(1));
			enumerator = emp.DirectReports.GetEnumerator();
			enumerator.MoveNext();
			emp1 = (Employee)enumerator.Current;
			Assert.That(emp1.OptionalComponent.Value1, Is.EqualTo("emp1-value1"));
			Assert.That(emp1.OptionalComponent.Value2, Is.EqualTo("emp1-value2"));
			emp1.OptionalComponent.Value1 = null;
			emp1.OptionalComponent.Value2 = null;
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.MergeAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}

			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					emp = (Employee)await (s.GetAsync(typeof (Employee), emp.Id));
					await (NHibernateUtil.InitializeAsync(emp.DirectReports));
					await (t.CommitAsync());
					s.Close();
				}

			Assert.That(emp.DirectReports.Count, Is.EqualTo(1));
			enumerator = emp.DirectReports.GetEnumerator();
			enumerator.MoveNext();
			emp1 = (Employee)enumerator.Current;
			Assert.That(emp1.OptionalComponent, Is.Null);
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync(emp));
					await (t.CommitAsync());
					s.Close();
				}
		}
	}
}
#endif
