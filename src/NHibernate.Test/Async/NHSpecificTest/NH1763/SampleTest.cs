#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1763
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CanUseConditionalOnCompositeTypeAsync()
		{
			using (ISession session = OpenSession())
			{
				await (session.CreateCriteria<Customer>().SetProjection(Projections.Conditional(Restrictions.IdEq(1), Projections.Property("Name"), Projections.Property("Name2"))).ListAsync());
			}
		}
	}
}
#endif
