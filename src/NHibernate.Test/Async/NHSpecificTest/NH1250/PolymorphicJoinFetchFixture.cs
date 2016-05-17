#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1250
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PolymorphicJoinFetchFixture : BugTestCase
	{
		[Test]
		public async Task FetchUsingICriteriaAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.CreateCriteria(typeof (Party)).SetMaxResults(10).List();
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task FetchUsingIQueryAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.CreateQuery("from Party").SetMaxResults(10).List();
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
