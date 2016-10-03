#if NET_4_5
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1250
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PolymorphicJoinFetchFixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1250";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		[Test]
		public async Task FetchUsingICriteriaAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateCriteria(typeof (Party)).SetMaxResults(10).ListAsync());
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task FetchUsingIQueryAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.CreateQuery("from Party").SetMaxResults(10).ListAsync());
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
