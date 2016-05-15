#if NET_4_5
using System;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1922
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanExecuteQueryOnStatelessSessionUsingDetachedCriteriaAsync()
		{
			using (var stateless = sessions.OpenStatelessSession())
			{
				var dc = DetachedCriteria.For<Customer>().Add(Restrictions.Eq("ValidUntil", new DateTime(2000, 1, 1)));
				var cust = await (dc.GetExecutableCriteria(stateless).UniqueResultAsync());
				Assert.IsNotNull(cust);
			}
		}
	}
}
#endif
