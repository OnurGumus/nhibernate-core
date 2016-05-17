#if NET_4_5
using log4net;
using NHibernate.Cfg;
using NUnit.Framework;
using log4net.Core;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1093
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		private async Task CleanupAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				using (s.BeginTransaction())
				{
					s.CreateQuery("delete from SimpleCached").ExecuteUpdate();
					await (s.Transaction.CommitAsync());
				}
			}
		}

		private async Task FillDbAsync()
		{
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new SimpleCached{Description = "Simple 1"}));
					await (s.SaveAsync(new SimpleCached{Description = "Simple 2"}));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		[Description("Without configured cache, shouldn't throw exception")]
		public async Task NoExceptionAsync()
		{
			await (FillDbAsync());
			NormalList();
			CriteriaQueryCache();
			HqlQueryCache();
			await (CleanupAsync());
		}
	}
}
#endif
