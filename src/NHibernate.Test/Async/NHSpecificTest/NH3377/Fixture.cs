#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using NHibernate.Dialect;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3377
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob", Age = "17", Solde = "5.4"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally", Age = "16"};
					await (session.SaveAsync(e2));
					var e3 = new Entity{Name = "true", Age = "10"};
					await (session.SaveAsync(e3));
					var e4 = new Entity{Name = "2014-10-13", Age = "11"};
					await (session.SaveAsync(e4));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
