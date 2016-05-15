#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1617
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseDataTypeInFormulaWithCriteriaQueryAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					string sql = "from User";
					IList<User> list = await (session.CreateQuery(sql).ListAsync<User>());
					Assert.That(list.Count, Is.EqualTo(1));
					Assert.That(list[0].OrderStatus, Is.EqualTo(2));
				}
			}
		}
	}
}
#endif
