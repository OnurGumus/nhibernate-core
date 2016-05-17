#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2324
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BulkUpdateWithCustomCompositeType : BugTestCase
	{
		[Test]
		public async Task ShouldAllowBulkupdateWithCompositeUserTypeAsync()
		{
			using (new Scenario(Sfi))
			{
				string queryString = @"update Entity m set m.Data.DataA = :dataA, m.Data.DataB = :dataB";
				using (ISession s = OpenSession())
					using (ITransaction t = s.BeginTransaction())
					{
						var query = s.CreateQuery(queryString).SetDateTime("dataA", new DateTime(2010, 3, 3)).SetDateTime("dataB", new DateTime(2010, 4, 4));
						Assert.That(() => query.ExecuteUpdate(), Throws.Nothing);
						await (t.CommitAsync());
					}
			}
		}
	}
}
#endif
