#if NET_4_5
using System.Collections;
using System.Data.Common;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TransactionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TransactionNotificationFixture : TestCase
	{
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
	}
}
#endif
