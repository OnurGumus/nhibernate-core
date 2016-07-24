#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1632
{
	using System.Transactions;
	using Cache;
	using Cfg;
	using Engine;
	using Id;

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1632";
			}
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				configuration.SetProperty(Environment.UseSecondLevelCache, "true").SetProperty(Environment.CacheProvider, typeof (HashtableCacheProvider).AssemblyQualifiedName);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task When_using_DTC_HiLo_knows_to_create_isolated_DTC_transactionAsync()
		{
			object scalar1, scalar2;
			using (var session = sessions.OpenSession())
				using (var command = session.Connection.CreateCommand())
				{
					command.CommandText = "select next_hi from hibernate_unique_key";
					scalar1 = await (command.ExecuteScalarAsync());
				}

			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				var generator = sessions.GetIdentifierGenerator(typeof (Person).FullName);
				Assert.That(generator, Is.InstanceOf<TableHiLoGenerator>());
				using (var session = sessions.OpenSession())
				{
					var id = await (generator.GenerateAsync((ISessionImplementor)session, new Person()));
				}

				// intentionally dispose without committing
				tx.Dispose();
			}

			using (var session = sessions.OpenSession())
				using (var command = session.Connection.CreateCommand())
				{
					command.CommandText = "select next_hi from hibernate_unique_key";
					scalar2 = await (command.ExecuteScalarAsync());
				}

			Assert.AreNotEqual(scalar1, scalar2, "HiLo must run with in its own transaction");
		}

		[Test]
		public void Dispose_session_inside_transaction_scope()
		{
			ISession s;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (s = sessions.OpenSession())
				{
				}

				tx.Complete();
			}

			Assert.IsFalse(s.IsOpen);
		}

		[Test]
		public async Task When_commiting_items_in_DTC_transaction_will_add_items_to_2nd_level_cacheAsync()
		{
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (var s = sessions.OpenSession())
				{
					await (s.SaveAsync(new Nums{ID = 29, NumA = 1, NumB = 3}));
				}

				tx.Complete();
			}

			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (var s = sessions.OpenSession())
				{
					var nums = await (s.LoadAsync<Nums>(29));
					Assert.AreEqual(1, nums.NumA);
					Assert.AreEqual(3, nums.NumB);
				}

				tx.Complete();
			}

			//closing the connection to ensure we can't really use it.
			var connection = await (sessions.ConnectionProvider.GetConnectionAsync());
			sessions.ConnectionProvider.CloseConnection(connection);
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (var s = sessions.OpenSession(connection))
				{
					var nums = await (s.LoadAsync<Nums>(29));
					Assert.AreEqual(1, nums.NumA);
					Assert.AreEqual(3, nums.NumB);
				}

				tx.Complete();
			}

			using (var s = sessions.OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var nums = await (s.LoadAsync<Nums>(29));
					await (s.DeleteAsync(nums));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task When_committing_transaction_scope_will_commit_transactionAsync()
		{
			object id;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = sessions.OpenSession())
				{
					id = await (s.SaveAsync(new Nums{NumA = 1, NumB = 2, ID = 5}));
				}

				tx.Complete();
			}

			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Nums nums = await (s.GetAsync<Nums>(id));
					Assert.IsNotNull(nums);
					await (s.DeleteAsync(nums));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task Will_not_save_when_flush_mode_is_neverAsync()
		{
			object id;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = sessions.OpenSession())
				{
					s.FlushMode = FlushMode.Never;
					id = await (s.SaveAsync(new Nums{NumA = 1, NumB = 2, ID = 5}));
				}

				tx.Complete();
			}

			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Nums nums = await (s.GetAsync<Nums>(id));
					Assert.IsNull(nums);
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task When_using_two_sessions_with_explicit_flushAsync()
		{
			if (!TestDialect.SupportsConcurrentTransactions)
				Assert.Ignore(Dialect.GetType().Name + " does not support concurrent transactions.");
			object id1, id2;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s1 = sessions.OpenSession())
					using (ISession s2 = sessions.OpenSession())
					{
						id1 = await (s1.SaveAsync(new Nums{NumA = 1, NumB = 2, ID = 5}));
						await (s1.FlushAsync());
						id2 = await (s2.SaveAsync(new Nums{NumA = 1, NumB = 2, ID = 6}));
						await (s2.FlushAsync());
						tx.Complete();
					}
			}

			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Nums nums = await (s.GetAsync<Nums>(id1));
					Assert.IsNotNull(nums);
					await (s.DeleteAsync(nums));
					nums = await (s.GetAsync<Nums>(id2));
					Assert.IsNotNull(nums);
					await (s.DeleteAsync(nums));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task When_using_two_sessionsAsync()
		{
			if (!TestDialect.SupportsConcurrentTransactions)
				Assert.Ignore(Dialect.GetType().Name + " does not support concurrent transactions.");
			object id1, id2;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s1 = sessions.OpenSession())
					using (ISession s2 = sessions.OpenSession())
					{
						id1 = await (s1.SaveAsync(new Nums{NumA = 1, NumB = 2, ID = 5}));
						id2 = await (s2.SaveAsync(new Nums{NumA = 1, NumB = 2, ID = 6}));
						tx.Complete();
					}
			}

			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Nums nums = await (s.GetAsync<Nums>(id1));
					Assert.IsNotNull(nums);
					await (s.DeleteAsync(nums));
					nums = await (s.GetAsync<Nums>(id2));
					Assert.IsNotNull(nums);
					await (s.DeleteAsync(nums));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
