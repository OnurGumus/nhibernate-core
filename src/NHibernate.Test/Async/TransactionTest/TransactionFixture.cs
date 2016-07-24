#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TransactionTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TransactionFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			// The mapping is only actually needed in one test
			get
			{
				return new string[]{"Simple.hbm.xml"};
			}
		}

		[Test]
		public async Task SecondTransactionShouldntBeCommittedAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction t1 = session.BeginTransaction())
				{
					await (t1.CommitAsync());
				}

				using (ITransaction t2 = session.BeginTransaction())
				{
					Assert.IsFalse(t2.WasCommitted);
					Assert.IsFalse(t2.WasRolledBack);
				}
			}
		}

		[Test]
		public async Task CommitAfterDisposeThrowsExceptionAsync()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				t.Dispose();
				Assert.ThrowsAsync<ObjectDisposedException>(async () => await (t.CommitAsync()));
			}
		}

		[Test]
		public Task RollbackAfterDisposeThrowsExceptionAsync()
		{
			try
			{
				using (ISession s = OpenSession())
				{
					ITransaction t = s.BeginTransaction();
					t.Dispose();
					Assert.Throws<ObjectDisposedException>(() => t.Rollback());
				}

				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void EnlistAfterDisposeDoesNotThrowException()
		{
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				using (DbCommand cmd = s.Connection.CreateCommand())
				{
					t.Dispose();
					t.Enlist(cmd);
				}
			}
		}

		[Test]
		public async Task CommandAfterTransactionShouldWorkAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
				}

				await (s.CreateQuery("from Simple").ListAsync());
				using (ITransaction t = s.BeginTransaction())
				{
					await (t.CommitAsync());
				}

				await (s.CreateQuery("from Simple").ListAsync());
				using (ITransaction t = s.BeginTransaction())
				{
					t.Rollback();
				}

				await (s.CreateQuery("from Simple").ListAsync());
			}
		}

		[Test]
		public async Task WasCommittedOrRolledBackAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					Assert.AreSame(t, s.Transaction);
					Assert.IsFalse(s.Transaction.WasCommitted);
					Assert.IsFalse(s.Transaction.WasRolledBack);
					await (t.CommitAsync());
					// ISession.Transaction returns a new transaction
					// if the previous one completed!
					Assert.IsNotNull(s.Transaction);
					Assert.IsFalse(t == s.Transaction);
					Assert.IsTrue(t.WasCommitted);
					Assert.IsFalse(t.WasRolledBack);
					Assert.IsFalse(s.Transaction.WasCommitted);
					Assert.IsFalse(s.Transaction.WasRolledBack);
					Assert.IsFalse(t.IsActive);
					Assert.IsFalse(s.Transaction.IsActive);
				}

				using (ITransaction t = s.BeginTransaction())
				{
					t.Rollback();
					// ISession.Transaction returns a new transaction
					// if the previous one completed!
					Assert.IsNotNull(s.Transaction);
					Assert.IsFalse(t == s.Transaction);
					Assert.IsTrue(t.WasRolledBack);
					Assert.IsFalse(t.WasCommitted);
					Assert.IsFalse(s.Transaction.WasCommitted);
					Assert.IsFalse(s.Transaction.WasRolledBack);
					Assert.IsFalse(t.IsActive);
					Assert.IsFalse(s.Transaction.IsActive);
				}
			}
		}
	}
}
#endif
