#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using System.Threading;
using System.Transactions;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.SystemTransactions
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TransactionNotificationFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{};
			}
		}

		[Test]
		public async Task TwoTransactionScopesInsideOneSessionAsync()
		{
			var interceptor = new RecordingInterceptor();
			using (var session = sessions.OpenSession(interceptor))
			{
				using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					await (session.CreateCriteria<object>().ListAsync());
					scope.Complete();
				}

				using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					await (session.CreateCriteria<object>().ListAsync());
					scope.Complete();
				}
			}

			Assert.AreEqual(2, interceptor.afterTransactionBeginCalled);
			Assert.AreEqual(2, interceptor.beforeTransactionCompletionCalled);
			Assert.AreEqual(2, interceptor.afterTransactionCompletionCalled);
		}

		[Test]
		public async Task OneTransactionScopesInsideOneSessionAsync()
		{
			var interceptor = new RecordingInterceptor();
			using (var session = sessions.OpenSession(interceptor))
			{
				using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					await (session.CreateCriteria<object>().ListAsync());
					scope.Complete();
				}
			}

			Assert.AreEqual(1, interceptor.afterTransactionBeginCalled);
			Assert.AreEqual(1, interceptor.beforeTransactionCompletionCalled);
			Assert.AreEqual(1, interceptor.afterTransactionCompletionCalled);
		}

		[Description("NH2128, NH3572")]
		[Theory]
		public async Task ShouldNotifyAfterDistributedTransactionAsync(bool doCommit)
		{
			// Note: For distributed transaction, calling Close() on the session isn't
			// supported, so we don't need to test that scenario.
			var interceptor = new RecordingInterceptor();
			ISession s1 = null;
			ISession s2 = null;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				try
				{
					s1 = OpenSession(interceptor);
					s2 = OpenSession(interceptor);
					await (s1.CreateCriteria<object>().ListAsync());
					await (s2.CreateCriteria<object>().ListAsync());
				}
				finally
				{
					if (s1 != null)
						s1.Dispose();
					if (s2 != null)
						s2.Dispose();
				}

				if (doCommit)
					tx.Complete();
			}

			Assert.That(s1.IsOpen, Is.False);
			Assert.That(s2.IsOpen, Is.False);
			Assert.That(interceptor.afterTransactionCompletionCalled, Is.EqualTo(2));
		}

		[Description("NH2128")]
		[Theory]
		public async Task ShouldNotifyAfterDistributedTransactionWithOwnConnectionAsync(bool doCommit)
		{
			// Note: For distributed transaction, calling Close() on the session isn't
			// supported, so we don't need to test that scenario.
			var interceptor = new RecordingInterceptor();
			ISession s1 = null;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				DbConnection ownConnection1 = await (sessions.ConnectionProvider.GetConnectionAsync());
				try
				{
					try
					{
						s1 = sessions.OpenSession(ownConnection1, interceptor);
						await (s1.CreateCriteria<object>().ListAsync());
					}
					finally
					{
						if (s1 != null)
							s1.Dispose();
					}

					if (doCommit)
						tx.Complete();
				}
				finally
				{
					sessions.ConnectionProvider.CloseConnection(ownConnection1);
				}
			}

			// Transaction completion may happen asynchronously, so allow some delay.
			Assert.That(() => s1.IsOpen, Is.False.After(500, 100));
			Assert.That(interceptor.afterTransactionCompletionCalled, Is.EqualTo(1));
		}
	}
}
#endif
