#if NET_4_5
using System;
using NHibernate.Cfg;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1082
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1082";
			}
		}

		[Test]
		public async Task ExceptionsInBeforeTransactionCompletionAbortTransactionAsync()
		{
#pragma warning disable 618
			Assert.IsFalse(sessions.Settings.IsInterceptorsBeforeTransactionCompletionIgnoreExceptionsEnabled);
#pragma warning restore 618
			var c = new C{ID = 1, Value = "value"};
			var sessionInterceptor = new SessionInterceptorThatThrowsExceptionAtBeforeTransactionCompletion();
			using (ISession s = sessions.OpenSession(sessionInterceptor))
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(c));
					Assert.ThrowsAsync<BadException>(async () => await t.CommitAsync());
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
#pragma warning disable 618
			Assert.IsFalse(sessions.Settings.IsInterceptorsBeforeTransactionCompletionIgnoreExceptionsEnabled);
#pragma warning restore 618
			var c = new C{ID = 1, Value = "value"};
			var synchronization = new SynchronizationThatThrowsExceptionAtBeforeTransactionCompletion();
			using (ISession s = sessions.OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					t.RegisterSynchronization(synchronization);
					await (s.SaveAsync(c));
					Assert.ThrowsAsync<BadException>(async () => await t.CommitAsync());
				}

			using (ISession s = sessions.OpenSession())
			{
				var objectInDb = await (s.GetAsync<C>(1));
				Assert.IsNull(objectInDb);
			}
		}
	}

	[TestFixture]
	[Obsolete("Can be removed when Environment.InterceptorsBeforeTransactionCompletionIgnoreExceptions is removed.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OldBehaviorEnabledFixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1082";
			}
		}

		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.InterceptorsBeforeTransactionCompletionIgnoreExceptions, "true");
			base.Configure(configuration);
		}

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
					Assert.DoesNotThrowAsync(async () => await t.CommitAsync());
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
					Assert.DoesNotThrowAsync(async () => await t.CommitAsync());
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
