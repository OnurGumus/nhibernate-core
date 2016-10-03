#if NET_4_5
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2392
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
		public async Task CompositeUserTypeSettabilityAsync()
		{
			ISession s = OpenSession();
			try
			{
				await (s.SaveAsync(new A{StringData1 = "first", StringData2 = "second"}));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				A a = (await (s.CreateCriteria<A>().ListAsync<A>())).First();
				a.MyPhone = new PhoneNumber(1, null);
				await (s.SaveAsync(a));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}

			s = OpenSession();
			try
			{
				A a = (await (s.CreateCriteria<A>().ListAsync<A>())).First();
				a.MyPhone = new PhoneNumber(1, "555-1234");
				await (s.SaveAsync(a));
				await (s.FlushAsync());
			}
			finally
			{
				s.Close();
			}
		}
	}
}
#endif
