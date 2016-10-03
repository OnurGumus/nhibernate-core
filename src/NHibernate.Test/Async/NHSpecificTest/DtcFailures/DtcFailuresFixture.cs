#if NET_4_5
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Transactions;
using log4net;
using log4net.Repository.Hierarchy;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.DtcFailures
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DtcFailuresFixtureAsync : TestCaseAsync
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (DtcFailuresFixtureAsync));
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.DtcFailures.Mappings.hbm.xml"};
			}
		}

		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return TestDialect.GetTestDialect(dialect).SupportsDistributedTransactions;
		}

		protected override async Task CreateSchemaAsync()
		{
			// Copied from Configure method.
			Configuration config = new Configuration();
			if (TestConfigurationHelper.hibernateConfigFile != null)
				config.Configure(TestConfigurationHelper.hibernateConfigFile);
			// Our override so we can set nullability on database column without NHibernate knowing about it.
			config.BeforeBindMapping += BeforeBindMapping;
			// Copied from AddMappings methods.
			Assembly assembly = Assembly.Load(MappingsAssembly);
			foreach (string file in Mappings)
				config.AddResource(MappingsAssembly + "." + file, assembly);
			// Copied from CreateSchema method, but we use our own config.
			await (new SchemaExport(config).CreateAsync(false, true));
		}

		private void BeforeBindMapping(object sender, BindMappingEventArgs e)
		{
			HbmProperty prop = e.Mapping.RootClasses[0].Properties.OfType<HbmProperty>().Single(p => p.Name == "NotNullData");
			prop.notnull = true;
			prop.notnullSpecified = true;
		}

		[Test]
		public async Task WillNotCrashOnDtcPrepareFailureAsync()
		{
			var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
			using (ISession s = sessions.OpenSession())
			{
				await (s.SaveAsync(new Person{NotNullData = null})); // Cause a SQL not null constraint violation.
			}

			new ForceEscalationToDistributedTx();
			tx.Complete();
			try
			{
				tx.Dispose();
				Assert.Fail("Expected failure");
			}
			catch (AssertionException)
			{
				throw;
			}
			catch (Exception)
			{
			}
		}

		[Test]
		public void Can_roll_back_transaction()
		{
			if (Dialect is FirebirdDialect)
				Assert.Ignore("Firebird driver does not support distributed transactions");
			var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
			using (ISession s = sessions.OpenSession())
			{
				new ForceEscalationToDistributedTx(true); //will rollback tx
				s.Save(new Person{CreatedAt = DateTime.Today});
				tx.Complete();
			}

			try
			{
				tx.Dispose();
				Assert.Fail("Expected tx abort");
			}
			catch (TransactionAbortedException)
			{
			//expected   
			}
		}

		[Test]
		[Description("Another action inside the transaction do the rollBack outside nh-session-scope.")]
		public void RollbackOutsideNh()
		{
			if (Dialect is FirebirdDialect)
				Assert.Ignore("Firebird driver does not support distributed transactions");
			try
			{
				using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					using (ISession s = sessions.OpenSession())
					{
						var person = new Person{CreatedAt = DateTime.Now};
						s.Save(person);
					}

					new ForceEscalationToDistributedTx(true); //will rollback tx
					txscope.Complete();
				}

				log.DebugFormat("Transaction fail.");
				Assert.Fail("Expected tx abort");
			}
			catch (TransactionAbortedException)
			{
				log.DebugFormat("Transaction aborted.");
			}
		}

		[Test]
		[Description("rollback inside nh-session-scope should not commit save and the transaction should be aborted.")]
		public void TransactionInsertWithRollBackTask()
		{
			if (Dialect is FirebirdDialect)
				Assert.Ignore("Firebird driver does not support distributed transactions");
			try
			{
				using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					using (ISession s = sessions.OpenSession())
					{
						var person = new Person{CreatedAt = DateTime.Now};
						s.Save(person);
						new ForceEscalationToDistributedTx(true); //will rollback tx
						person.CreatedAt = DateTime.Now;
						s.Update(person);
					}

					txscope.Complete();
				}

				log.DebugFormat("Transaction fail.");
				Assert.Fail("Expected tx abort");
			}
			catch (TransactionAbortedException)
			{
				log.DebugFormat("Transaction aborted.");
			}
		}

		[Test, Ignore("Not fixed.")]
		[Description(@"Two session in two txscope 
(without an explicit NH transaction and without an explicit flush) 
and with a rollback in the second dtc and a ForceRollback outside nh-session-scope.")]
		public async Task TransactionInsertLoadWithRollBackTaskAsync()
		{
			object savedId;
			using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = sessions.OpenSession())
				{
					var person = new Person{CreatedAt = DateTime.Now};
					savedId = await (s.SaveAsync(person));
				}

				txscope.Complete();
			}

			try
			{
				using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					using (ISession s = sessions.OpenSession())
					{
						var person = await (s.GetAsync<Person>(savedId));
						person.CreatedAt = DateTime.Now;
						await (s.UpdateAsync(person));
					}

					new ForceEscalationToDistributedTx(true);
					log.Debug("completing the tx scope");
					txscope.Complete();
				}

				log.Debug("Transaction fail.");
				Assert.Fail("Expected tx abort");
			}
			catch (TransactionAbortedException)
			{
				log.Debug("Transaction aborted.");
			}
			finally
			{
				using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					using (ISession s = sessions.OpenSession())
					{
						var person = await (s.GetAsync<Person>(savedId));
						await (s.DeleteAsync(person));
					}

					txscope.Complete();
				}
			}
		}

		private int totalCall;
		[Test]
		public async Task CanDeleteItemInDtcAsync()
		{
			object id;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = sessions.OpenSession())
				{
					id = await (s.SaveAsync(new Person{CreatedAt = DateTime.Today}));
					new ForceEscalationToDistributedTx();
					tx.Complete();
				}
			}

			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = sessions.OpenSession())
				{
					new ForceEscalationToDistributedTx();
					await (s.DeleteAsync(await (s.GetAsync<Person>(id))));
					tx.Complete();
				}
			}
		}

		[Test]
		[Description("Open/Close a session inside a TransactionScope fails.")]
		public async Task NH1744Async()
		{
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = sessions.OpenSession())
				{
					await (s.FlushAsync());
				}

				using (ISession s = sessions.OpenSession())
				{
					await (s.FlushAsync());
				}
			//and I always leave the transaction disposed without calling tx.Complete(), I let the database server to rollback all actions in this test. 
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ForceEscalationToDistributedTx : IEnlistmentNotification
		{
			private readonly bool shouldRollBack;
			private readonly int thread;
			public ForceEscalationToDistributedTx(bool shouldRollBack)
			{
				this.shouldRollBack = shouldRollBack;
				thread = Thread.CurrentThread.ManagedThreadId;
				System.Transactions.Transaction.Current.EnlistDurable(Guid.NewGuid(), this, EnlistmentOptions.None);
			}

			public ForceEscalationToDistributedTx(): this (false)
			{
			}

			public void Prepare(PreparingEnlistment preparingEnlistment)
			{
				Assert.AreNotEqual(thread, Thread.CurrentThread.ManagedThreadId);
				if (shouldRollBack)
				{
					log.Debug(">>>>Force Rollback<<<<<");
					preparingEnlistment.ForceRollback();
				}
				else
				{
					preparingEnlistment.Prepared();
				}
			}

			public void Commit(Enlistment enlistment)
			{
				enlistment.Done();
			}

			public void Rollback(Enlistment enlistment)
			{
				enlistment.Done();
			}

			public void InDoubt(Enlistment enlistment)
			{
				enlistment.Done();
			}
		}
	}
}
#endif
