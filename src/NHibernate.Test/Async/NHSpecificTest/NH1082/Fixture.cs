#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1082
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task ExceptionsInBeforeTransactionCompletionAbortTransactionAsync()
		{
			Assert.IsFalse(sessions.Settings.IsInterceptorsBeforeTransactionCompletionIgnoreExceptionsEnabled);
			var c = new C{ID = 1, Value = "value"};
			var sessionInterceptor = new SessionInterceptorThatThrowsExceptionAtBeforeTransactionCompletion();
			using (ISession s = sessions.OpenSession(sessionInterceptor))
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(c));
					Assert.Throws<BadException>(async () => await t.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
			{
				var objectInDb = await (s.GetAsync<C>(1));
				Assert.IsNull(objectInDb);
			}
		}

		[Test]
		public async Task ExceptionsInSynchronizationBeforeTransactionCompletionAbortTransactionAsync()
		{
			Assert.IsFalse(sessions.Settings.IsInterceptorsBeforeTransactionCompletionIgnoreExceptionsEnabled);
			var c = new C{ID = 1, Value = "value"};
			var synchronization = new SynchronizationThatThrowsExceptionAtBeforeTransactionCompletion();
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					t.RegisterSynchronization(synchronization);
					await (s.SaveAsync(c));
					Assert.Throws<BadException>(async () => await t.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
			{
				var objectInDb = await (s.GetAsync<C>(1));
				Assert.IsNull(objectInDb);
			}
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OldBehaviorEnabledFixture : BugTestCase
	{
		[Test]
		public async Task ExceptionsInBeforeTransactionCompletionAreIgnoredAsync()
		{
			Assert.IsTrue(sessions.Settings.IsInterceptorsBeforeTransactionCompletionIgnoreExceptionsEnabled);
			var c = new C{ID = 1, Value = "value"};
			var sessionInterceptor = new SessionInterceptorThatThrowsExceptionAtBeforeTransactionCompletion();
			using (ISession s = sessions.OpenSession(sessionInterceptor))
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(c));
					Assert.DoesNotThrow(async () => await t.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
			{
				var objectInDb = await (s.GetAsync<C>(1));
				Assert.IsNotNull(objectInDb);
				await (s.DeleteAsync(objectInDb));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task ExceptionsInSynchronizationBeforeTransactionCompletionAreIgnoredAsync()
		{
			Assert.IsTrue(sessions.Settings.IsInterceptorsBeforeTransactionCompletionIgnoreExceptionsEnabled);
			var c = new C{ID = 1, Value = "value"};
			var synchronization = new SynchronizationThatThrowsExceptionAtBeforeTransactionCompletion();
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					t.RegisterSynchronization(synchronization);
					await (s.SaveAsync(c));
					Assert.DoesNotThrow(async () => await t.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
			{
				var objectInDb = await (s.GetAsync<C>(1));
				Assert.IsNotNull(objectInDb);
				await (s.DeleteAsync(objectInDb));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
