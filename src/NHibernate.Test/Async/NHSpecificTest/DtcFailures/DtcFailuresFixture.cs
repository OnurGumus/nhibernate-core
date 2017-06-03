﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


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

namespace NHibernate.Test.NHSpecificTest.DtcFailures
{
	using System.Threading.Tasks;
	[TestFixture]
	public class DtcFailuresFixtureAsync : TestCase
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(DtcFailuresFixtureAsync));

		protected override IList Mappings
		{
			get { return new[] {"NHSpecificTest.DtcFailures.Mappings.hbm.xml"}; }
		}

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return TestDialect.GetTestDialect(dialect).SupportsDistributedTransactions;
		}

        protected override void CreateSchema()
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
            new SchemaExport(config).Create(false, true);
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
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(new Person {NotNullData = null}));  // Cause a SQL not null constraint violation.
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
			catch (Exception) {}
		}

		[Test]
		public async Task Can_roll_back_transactionAsync()
		{
			if (Dialect is FirebirdDialect)
				Assert.Ignore("Firebird driver does not support distributed transactions");

			var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
			using (ISession s = OpenSession())
			{
				new ForceEscalationToDistributedTx(true); //will rollback tx
				await (s.SaveAsync(new Person { CreatedAt = DateTime.Today }));

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
		public async Task RollbackOutsideNhAsync()
		{
			if (Dialect is FirebirdDialect)
				Assert.Ignore("Firebird driver does not support distributed transactions");

			try
			{
				using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					using (ISession s = OpenSession())
					{
						var person = new Person { CreatedAt = DateTime.Now };
						await (s.SaveAsync(person));
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
		public async Task TransactionInsertWithRollBackTaskAsync()
		{
			if (Dialect is FirebirdDialect)
				Assert.Ignore("Firebird driver does not support distributed transactions");

			try
			{
				using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{
					using (ISession s = OpenSession())
					{
						var person = new Person {CreatedAt = DateTime.Now};
						await (s.SaveAsync(person));
						new ForceEscalationToDistributedTx(true); //will rollback tx
						person.CreatedAt = DateTime.Now;
						await (s.UpdateAsync(person));
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

		private int totalCall;

		[Test]
		public async Task CanDeleteItemInDtcAsync()
		{
			object id;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = OpenSession())
				{
					id = await (s.SaveAsync(new Person {CreatedAt = DateTime.Today}));

					new ForceEscalationToDistributedTx();

					tx.Complete();
				}
			}

			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (ISession s = OpenSession())
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
				using (ISession s = OpenSession())
				{
					await (s.FlushAsync());
				}

				using (ISession s = OpenSession())
				{
					await (s.FlushAsync());
				}

				//and I always leave the transaction disposed without calling tx.Complete(), I let the database server to rollback all actions in this test. 
			} 
		}

		public class ForceEscalationToDistributedTx : IEnlistmentNotification
		{
			private readonly bool shouldRollBack;
			private readonly int thread;

			public ForceEscalationToDistributedTx(bool shouldRollBack)
			{
				this.shouldRollBack = shouldRollBack;
				thread = Thread.CurrentThread.ManagedThreadId;
				System.Transactions.Transaction.Current.EnlistDurable(Guid.NewGuid(), this, EnlistmentOptions.None);
			}

			public ForceEscalationToDistributedTx() : this(false) {}

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