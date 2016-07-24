#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Engine;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ConnectionTest
{
	[TestFixture, Ignore("Not yet supported. Need AutoClosed feature.(TransactionContext)")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ThreadLocalCurrentSessionTestAsync : ConnectionManagementTestCaseAsync
	{
		protected override ISession GetSessionUnderTest()
		{
			ISession session = OpenSession();
			session.BeginTransaction();
			return session;
		}

		protected override async Task ConfigureAsync(Configuration configuration)
		{
			await (base.ConfigureAsync(cfg));
			cfg.SetProperty(Environment.CurrentSessionContextClass, typeof (TestableThreadLocalContext).AssemblyQualifiedName);
			cfg.SetProperty(Environment.GenerateStatistics, "true");
		}

		protected override async Task ReleaseAsync(ISession session)
		{
			long initialCount = sessions.Statistics.SessionCloseCount;
			await (session.Transaction.CommitAsync());
			long subsequentCount = sessions.Statistics.SessionCloseCount;
			Assert.AreEqual(initialCount + 1, subsequentCount, "Session still open after commit");
			// also make sure it was cleaned up from the internal ThreadLocal...
			Assert.IsFalse(TestableThreadLocalContext.HasBind(), "session still bound to internal ThreadLocal");
		}

		//TODO: Need AutoCloseEnabled feature after commit.
		[Test]
		public async Task ContextCleanupAsync()
		{
			ISession session = sessions.OpenSession();
			session.BeginTransaction();
			await (session.Transaction.CommitAsync());
			Assert.IsFalse(session.IsOpen, "session open after txn completion");
			Assert.IsFalse(TestableThreadLocalContext.IsSessionBound(session), "session still bound after txn completion");
			ISession session2 = OpenSession();
			Assert.IsFalse(session.Equals(session2), "same session returned after txn completion");
			session2.Close();
			Assert.IsFalse(session2.IsOpen, "session open after closing");
			Assert.IsFalse(TestableThreadLocalContext.IsSessionBound(session2), "session still bound after closing");
		}

		[Test]
		public void TransactionProtection()
		{
			using (ISession session = OpenSession())
			{
				try
				{
					session.CreateQuery("from Silly");
					Assert.Fail("method other than beginTransaction{} allowed");
				}
				catch (HibernateException)
				{
				// ok
				}
			}
		}
	}
}
#endif
