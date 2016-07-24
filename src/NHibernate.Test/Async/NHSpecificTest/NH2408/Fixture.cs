#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2408
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Dialect.MsSql2000Dialect;
		}

		[Test]
		public async Task ShouldGenerateCorrectSqlStatementAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.CreateQuery("from Animal a where a.Name = ?");
				query.SetParameter(0, "Prince");
				query.SetLockMode("a", LockMode.Upgrade);
				Assert.DoesNotThrowAsync(async () => await (query.ListAsync()));
			}
		}
	}
}
#endif
