#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Custom
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class CustomSQLSupportTest : TestCase
	{
		[Test]
		public async Task HandSQLAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Organization ifa = new Organization("IFA");
			Organization jboss = new Organization("JBoss");
			Person gavin = new Person("Gavin");
			Employment emp = new Employment(gavin, jboss, "AU");
			object orgId = await (s.SaveAsync(jboss));
			await (s.SaveAsync(ifa));
			await (s.SaveAsync(gavin));
			await (s.SaveAsync(emp));
			await (t.CommitAsync());
			t = s.BeginTransaction();
			Person christian = new Person("Christian");
			await (s.SaveAsync(christian));
			Employment emp2 = new Employment(christian, jboss, "EU");
			await (s.SaveAsync(emp2));
			await (t.CommitAsync());
			s.Close();
			sessions.Evict(typeof (Organization));
			sessions.Evict(typeof (Person));
			sessions.Evict(typeof (Employment));
			s = OpenSession();
			t = s.BeginTransaction();
			jboss = (Organization)await (s.GetAsync(typeof (Organization), orgId));
			Assert.AreEqual(jboss.Employments.Count, 2);
			emp = (Employment)GetFirstItem(jboss.Employments);
			gavin = emp.Employee;
			Assert.AreEqual(gavin.Name, "GAVIN");
			Assert.AreEqual(await (s.GetCurrentLockModeAsync(gavin)), LockMode.Upgrade);
			emp.EndDate = DateTime.Today;
			Employment emp3 = new Employment(gavin, jboss, "US");
			await (s.SaveAsync(emp3));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			t = s.BeginTransaction();
			IEnumerator iter = (await (s.GetNamedQuery("allOrganizationsWithEmployees").ListAsync())).GetEnumerator();
			Assert.IsTrue(iter.MoveNext());
			Organization o = (Organization)iter.Current;
			Assert.AreEqual(o.Employments.Count, 3);
			foreach (Employment e in o.Employments)
			{
				await (s.DeleteAsync(e));
			}

			foreach (Employment e in o.Employments)
			{
				await (s.DeleteAsync(e.Employee));
			}

			await (s.DeleteAsync(o));
			Assert.IsFalse(iter.MoveNext());
			await (s.DeleteAsync(ifa));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
