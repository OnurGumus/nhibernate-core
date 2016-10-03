#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH980
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH980FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task IdGeneratorShouldUseQuotedTableNameAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					IdOnly obj = new IdOnly();
					await (s.SaveAsync(obj));
					await (s.FlushAsync());
					await (s.DeleteAsync(obj));
					await (t.CommitAsync());
				}
		}
	}
}
#endif
