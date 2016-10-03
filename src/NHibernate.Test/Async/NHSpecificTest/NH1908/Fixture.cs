#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1908
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task QueryPropertyInBothFilterAndQueryAsync()
		{
			using (ISession s = OpenSession())
			{
				s.EnableFilter("validity").SetParameter("date", DateTime.Now);
				await (s.CreateQuery(@"
				select 
					inv.ID
				from 
					Invoice inv
						join inv.Category cat with cat.ValidUntil > :now
						left join cat.ParentCategory parentCat
				where
					inv.ID = :invId
					and inv.Issued < :now
				").SetDateTime("now", DateTime.Now).SetInt32("invId", -999).ListAsync());
			}
		}

		[Test]
		public async Task QueryPropertyInBothFilterAndQueryUsingWithAsync()
		{
			using (ISession s = OpenSession())
			{
				s.EnableFilter("validity").SetParameter("date", DateTime.Now);
				await (s.CreateQuery(@"
				select 
					inv.ID
				from 
					Invoice inv
						join inv.Category cat with cat.ValidUntil > :now
						left join cat.ParentCategory parentCat with parentCat.ID != :myInt
				where
					inv.ID = :invId
					and inv.Issued < :now
				").SetDateTime("now", DateTime.Now).SetInt32("invId", -999).SetInt32("myInt", -888).ListAsync());
			}
		}
	}
}
#endif
