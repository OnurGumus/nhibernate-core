#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using NHibernate.Linq;
using System.Linq;
using NHibernate.Linq.Functions;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2244
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				await (s.DeleteAsync("from A"));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task LinqComponentTypeEqualityAsync()
		{
			ISession s = OpenSession();
			try
			{
				await (s.SaveAsync(new A{Phone = new PhoneNumber(1, "555-1111")}));
				await (s.SaveAsync(new A{Phone = new PhoneNumber(1, "555-2222")}));
				await (s.SaveAsync(new A{Phone = new PhoneNumber(1, "555-3333")}));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				A item = s.Query<A>().Where(a => a.Phone == new PhoneNumber(1, "555-2222")).Single();
				Assert.AreEqual("555-2222", item.Phone.Number);
			}
			finally
			{
				s.Close();
			}
		}
	}
}
#endif
