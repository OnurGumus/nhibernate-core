#if NET_4_5
using System.Collections;
using System.Data.Common;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.TransactionTest
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
		public async Task CommitAsync()
		{
			var interceptor = new RecordingInterceptor();
			using (ISession session = sessions.OpenSession(interceptor))
			{
				ITransaction tx = session.BeginTransaction();
				await (tx.CommitAsync());
				Assert.That(interceptor.afterTransactionBeginCalled, Is.EqualTo(1));
				Assert.That(interceptor.beforeTransactionCompletionCalled, Is.EqualTo(1));
				Assert.That(interceptor.afterTransactionCompletionCalled, Is.EqualTo(1));
			}
		}

		[Theory]
		[Description("NH2128")]
		public async Task ShouldNotifyAfterTransactionAsync(bool usePrematureClose)
		{
			var interceptor = new RecordingInterceptor();
			ISession s;
			using (s = OpenSession(interceptor))
				using (s.BeginTransaction())
				{
					await (s.CreateCriteria<object>().ListAsync());
					// Call session close while still inside transaction?
					if (usePrematureClose)
						s.Close();
				}

			Assert.That(s.IsOpen, Is.False);
			Assert.That(interceptor.afterTransactionCompletionCalled, Is.EqualTo(1));
		}

		[Description("NH2128")]
		[Theory]
		public async Task ShouldNotifyAfterTransactionWithOwnConnectionAsync(bool usePrematureClose)
		{
			var interceptor = new RecordingInterceptor();
			ISession s;
			using (DbConnection ownConnection = await (sessions.ConnectionProvider.GetConnectionAsync()))
			{
				using (s = sessions.OpenSession(ownConnection, interceptor))
					using (s.BeginTransaction())
					{
						await (s.CreateCriteria<object>().ListAsync());
						// Call session close while still inside transaction?
						if (usePrematureClose)
							s.Close();
					}
			}

			Assert.That(s.IsOpen, Is.False);
			Assert.That(interceptor.afterTransactionCompletionCalled, Is.EqualTo(1));
		}
	}
}
#endif
