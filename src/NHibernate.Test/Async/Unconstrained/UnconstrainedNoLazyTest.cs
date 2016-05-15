#if NET_4_5
using System;
using System.Collections;
using log4net;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Unconstrained
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UnconstrainedNoLazyTest : TestCase
	{
		[Test]
		public async Task UnconstrainedNoCacheAsync()
		{
			ISession session = OpenSession();
			ITransaction tx = session.BeginTransaction();
			Person p = new Person("gavin");
			p.EmployeeId = "123456";
			await (session.SaveAsync(p));
			await (tx.CommitAsync());
			session.Close();
			sessions.Evict(typeof (Person));
			session = OpenSession();
			tx = session.BeginTransaction();
			p = (Person)await (session.GetAsync(typeof (Person), "gavin"));
			Assert.IsNull(p.Employee);
			p.Employee = new Employee("123456");
			await (tx.CommitAsync());
			session.Close();
			sessions.Evict(typeof (Person));
			session = OpenSession();
			tx = session.BeginTransaction();
			p = (Person)await (session.GetAsync(typeof (Person), "gavin"));
			Assert.IsNotNull(p.Employee);
			Assert.IsTrue(NHibernateUtil.IsInitialized(p.Employee));
			await (session.DeleteAsync(p));
			await (tx.CommitAsync());
			session.Close();
		}

		[Test]
		public async Task UnconstrainedOuterJoinFetchAsync()
		{
			ISession session = OpenSession();
			ITransaction tx = session.BeginTransaction();
			Person p = new Person("gavin");
			p.EmployeeId = "123456";
			await (session.SaveAsync(p));
			await (tx.CommitAsync());
			session.Close();
			sessions.Evict(typeof (Person));
			session = OpenSession();
			tx = session.BeginTransaction();
			p = (Person)await (session.CreateCriteria(typeof (Person)).SetFetchMode("Employee", FetchMode.Join).Add(Expression.Eq("Name", "gavin")).UniqueResultAsync());
			Assert.IsNull(p.Employee);
			p.Employee = new Employee("123456");
			await (tx.CommitAsync());
			session.Close();
			sessions.Evict(typeof (Person));
			session = OpenSession();
			tx = session.BeginTransaction();
			p = (Person)await (session.CreateCriteria(typeof (Person)).SetFetchMode("Employee", FetchMode.Join).Add(Expression.Eq("Name", "gavin")).UniqueResultAsync());
			Assert.IsTrue(NHibernateUtil.IsInitialized(p.Employee));
			Assert.IsNotNull(p.Employee);
			await (session.DeleteAsync(p));
			await (tx.CommitAsync());
			session.Close();
		}

		[Test]
		public async Task UnconstrainedAsync()
		{
			ILog log = LogManager.GetLogger(GetType());
			log.Info("Unconstrained - BEGIN");
			log.Info("Creating Person#gavin with EmployeeId = 123456 (non-existent)");
			ISession session = OpenSession();
			ITransaction tx = session.BeginTransaction();
			Person p = new Person("gavin");
			p.EmployeeId = "123456";
			await (session.SaveAsync(p));
			await (tx.CommitAsync());
			session.Close();
			log.Info("Loading Person#gavin and associating it with a new Employee#123456");
			session = OpenSession();
			tx = session.BeginTransaction();
			p = (Person)await (session.GetAsync(typeof (Person), "gavin"));
			Assert.IsNull(p.Employee);
			p.Employee = new Employee("123456");
			await (tx.CommitAsync());
			session.Close();
			log.Info("Reloading Person#gavin and checking that its Employee is not null");
			session = OpenSession();
			tx = session.BeginTransaction();
			p = (Person)await (session.GetAsync(typeof (Person), "gavin"));
			Assert.IsNotNull(p.Employee);
			Assert.IsTrue(NHibernateUtil.IsInitialized(p.Employee));
			Assert.IsNotNull(p.Employee.Id);
			await (session.DeleteAsync(p));
			await (tx.CommitAsync());
			session.Close();
		}

		[Test]
		public async Task ManyToOneUpdateFalseAsync()
		{
			ISession session = OpenSession();
			ITransaction tx = session.BeginTransaction();
			Person p = new Person("gavin");
			p.EmployeeId = "123456";
			p.Unrelated = 10;
			await (session.SaveAsync(p));
			await (tx.CommitAsync());
			session.BeginTransaction();
			p.Employee = new Employee("456123");
			p.Unrelated = 235; // Force update of the object
			await (session.SaveAsync(p.Employee));
			await (session.Transaction.CommitAsync());
			session.Close();
			session = OpenSession();
			session.BeginTransaction();
			p = (Person)await (session.LoadAsync(typeof (Person), "gavin"));
			// Should be null, not Employee#456123
			Assert.IsNull(p.Employee);
			await (session.DeleteAsync(p));
			await (session.DeleteAsync("from Employee"));
			await (session.Transaction.CommitAsync());
			session.Close();
		}
	}
}
#endif
