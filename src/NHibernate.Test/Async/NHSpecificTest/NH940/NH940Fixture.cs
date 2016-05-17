#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH940
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH940Fixture : BugTestCase
	{
		[Test]
		public async Task BugAsync()
		{
			A a = new A();
			B b = new B();
			a.B = b;
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			await (s.SaveAsync(a));
			await (t.CommitAsync());
			s.Close();
			s = OpenSession();
			IList l = s.CreateCriteria(typeof (A)).List();
			try
			{
				((A)l[0]).Execute();
				Assert.Fail("Should have thrown MyException");
			}
			catch (MyException)
			{
			// OK
			}
			catch (Exception e)
			{
				Assert.Fail("Should have thrown MyException, thrown {0} instead", e);
			}

			s = OpenSession();
			t = s.BeginTransaction();
			await (s.DeleteAsync(a));
			await (t.CommitAsync());
			s.Close();
		}
	}
}
#endif
