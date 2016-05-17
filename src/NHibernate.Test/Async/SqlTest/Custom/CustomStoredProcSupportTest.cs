#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Custom
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class CustomStoredProcSupportTest : CustomSQLSupportTest
	{
		[Test]
		public async Task EntityStoredProcedureAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			Organization ifa = new Organization("IFA");
			Organization jboss = new Organization("JBoss");
			Person gavin = new Person("Gavin");
			Employment emp = new Employment(gavin, jboss, "AU");
			await (s.SaveAsync(ifa));
			await (s.SaveAsync(jboss));
			await (s.SaveAsync(gavin));
			await (s.SaveAsync(emp));
			IQuery namedQuery = s.GetNamedQuery("selectAllEmployments");
			IList list = namedQuery.List();
			Assert.IsTrue(list[0] is Employment);
			await (s.DeleteAsync(emp));
			await (s.DeleteAsync(ifa));
			await (s.DeleteAsync(jboss));
			await (s.DeleteAsync(gavin));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
