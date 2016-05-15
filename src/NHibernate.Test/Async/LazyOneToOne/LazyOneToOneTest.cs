#if NET_4_5
using System;
using System.Collections;
using NHibernate.Intercept;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.LazyOneToOne
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LazyOneToOneTest : TestCase
	{
		[Test]
		public async Task LazyAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			var p = new Person{Name = "Gavin"};
			var p2 = new Person{Name = "Emmanuel"};
			var e = new Employee(p);
			new Employment(e, "JBoss");
			var old = new Employment(e, "IFA")
			{EndDate = DateTime.Today};
			await (s.PersistAsync(p));
			await (s.PersistAsync(p2));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			p = await (s.CreateQuery("from Person where name='Gavin'").UniqueResultAsync<Person>());
			Assert.That(!await (NHibernateUtil.IsPropertyInitializedAsync(p, "Employee")));
			Assert.That(p.Employee.Person, Is.SameAs(p));
			Assert.That(NHibernateUtil.IsInitialized(p.Employee.Employments));
			Assert.That(p.Employee.Employments.Count, Is.EqualTo(1));
			p2 = await (s.CreateQuery("from Person where name='Emmanuel'").UniqueResultAsync<Person>());
			Assert.That(p2.Employee, Is.Null);
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			p = await (s.GetAsync<Person>("Gavin"));
			Assert.That(!await (NHibernateUtil.IsPropertyInitializedAsync(p, "Employee")));
			Assert.That(p.Employee.Person, Is.SameAs(p));
			Assert.That(NHibernateUtil.IsInitialized(p.Employee.Employments));
			Assert.That(p.Employee.Employments.Count, Is.EqualTo(1));
			p2 = await (s.GetAsync<Person>("Emmanuel"));
			Assert.That(p2.Employee, Is.Null);
			await (s.DeleteAsync(p2));
			await (s.DeleteAsync(old));
			await (s.DeleteAsync(p));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
