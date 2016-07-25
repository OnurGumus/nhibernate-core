#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using NUnit.Framework;
using NHibernate.Hql.Ast.ANTLR;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class TestCaseAsync
	{
		private const bool OutputDdl = false;
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;
		private static readonly ILog log = LogManager.GetLogger(typeof (TestCaseAsync));
		protected Dialect.Dialect Dialect
		{
			get
			{
				return NHibernate.Dialect.Dialect.GetDialect(cfg.Properties);
			}
		}

		protected TestDialect TestDialect
		{
			get
			{
				return TestDialect.GetTestDialect(Dialect);
			}
		}

		/// <summary>
		/// To use in in-line test
		/// </summary>
		protected bool IsAntlrParser
		{
			get
			{
				return sessions.Settings.QueryTranslatorFactory is ASTQueryTranslatorFactory;
			}
		}

		protected ISession lastOpenedSession;
		private DebugConnectionProvider connectionProvider;
		/// <summary>
		/// Mapping files used in the TestCase
		/// </summary>
		protected abstract IList Mappings
		{
			get;
		}

		/// <summary>
		/// Assembly to load mapping files from (default is NHibernate.DomainModel).
		/// </summary>
		protected virtual string MappingsAssembly
		{
			get
			{
				return "NHibernate.DomainModel";
			}
		}

		static TestCaseAsync()
		{
			// Configure log4net here since configuration through an attribute doesn't always work.
			XmlConfigurator.Configure();
		}

		/// <summary>
		/// Creates the tables used in this TestCase
		/// </summary>
		[OneTimeSetUp]
		public async Task TestFixtureSetUpAsync()
		{
			try
			{
				await (ConfigureAsync());
				if (!AppliesTo(Dialect))
				{
					Assert.Ignore(GetType() + " does not apply to " + Dialect);
				}

				await (CreateSchemaAsync());
				try
				{
					BuildSessionFactory();
					if (!AppliesTo(sessions))
					{
						Assert.Ignore(GetType() + " does not apply with the current session-factory configuration");
					}
				}
				catch
				{
					await (DropSchemaAsync());
					throw;
				}
			}
			catch (Exception e)
			{
				await (CleanupAsync());
				log.Error("Error while setting up the test fixture", e);
				throw;
			}
		}

		/// <summary>
		/// Removes the tables used in this TestCase.
		/// </summary>
		/// <remarks>
		/// If the tables are not cleaned up sometimes SchemaExport runs into
		/// Sql errors because it can't drop tables because of the FKs.  This 
		/// will occur if the TestCase does not have the same hbm.xml files
		/// included as a previous one.
		/// </remarks>
		[OneTimeTearDown]
		public async Task TestFixtureTearDownAsync()
		{
			// If TestFixtureSetup fails due to an IgnoreException, it will still run the teardown.
			// We don't want to try to clean up again since the setup would have already done so.
			// If cfg is null already, that indicates it's already been cleaned up and we needn't.
			if (cfg != null)
			{
				if (!AppliesTo(Dialect))
					return;
				await (DropSchemaAsync());
				await (CleanupAsync());
			}
		}

		protected virtual Task OnSetUpAsync()
		{
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Set up the test. This method is not overridable, but it calls
		/// <see cref = "OnSetUp"/> which is.
		/// </summary>
		[SetUp]
		public Task SetUpAsync()
		{
			return OnSetUpAsync();
		}

		protected virtual Task OnTearDownAsync()
		{
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Checks that the test case cleans up after itself. This method
		/// is not overridable, but it calls <see cref = "OnTearDown"/> which is.
		/// </summary>
		[TearDown]
		public async Task TearDownAsync()
		{
			await (OnTearDownAsync());
			bool wasClosed = CheckSessionWasClosed();
			bool wasCleaned = await (CheckDatabaseWasCleanedAsync());
			bool wereConnectionsClosed = CheckConnectionsWereClosed();
			bool fail = !wasClosed || !wasCleaned || !wereConnectionsClosed;
			if (fail)
			{
				Assert.Fail("Test didn't clean up after itself. session closed: " + wasClosed + " database cleaned: " + wasCleaned + " connection closed: " + wereConnectionsClosed);
			}
		}

		private bool CheckSessionWasClosed()
		{
			if (lastOpenedSession != null && lastOpenedSession.IsOpen)
			{
				log.Error("Test case didn't close a session, closing");
				lastOpenedSession.Close();
				return false;
			}

			return true;
		}

		protected virtual async Task<bool> CheckDatabaseWasCleanedAsync()
		{
			if (sessions.GetAllClassMetadata().Count == 0)
			{
				// Return early in the case of no mappings, also avoiding
				// a warning when executing the HQL below.
				return true;
			}

			bool empty;
			using (ISession s = sessions.OpenSession())
			{
				IList objects = await (s.CreateQuery("from System.Object o").ListAsync());
				empty = objects.Count == 0;
			}

			if (!empty)
			{
				log.Error("Test case didn't clean up the database after itself, re-creating the schema");
				await (DropSchemaAsync());
				await (CreateSchemaAsync());
			}

			return empty;
		}

		private bool CheckConnectionsWereClosed()
		{
			if (connectionProvider == null || !connectionProvider.HasOpenConnections)
			{
				return true;
			}

			log.Error("Test case didn't close all open connections, closing");
			connectionProvider.CloseAllConnections();
			return false;
		}

		private async Task ConfigureAsync()
		{
			cfg = TestConfigurationHelper.GetDefaultConfiguration();
			AddMappings(cfg);
			await (ConfigureAsync(cfg));
			ApplyCacheSettings(cfg);
		}

		protected virtual void AddMappings(Configuration configuration)
		{
			Assembly assembly = Assembly.Load(MappingsAssembly);
			foreach (string file in Mappings)
			{
				configuration.AddResource(MappingsAssembly + "." + file, assembly);
			}
		}

		protected virtual Task CreateSchemaAsync()
		{
			return new SchemaExport(cfg).CreateAsync(OutputDdl, true);
		}

		protected virtual Task DropSchemaAsync()
		{
			return new SchemaExport(cfg).DropAsync(OutputDdl, true);
		}

		protected virtual void BuildSessionFactory()
		{
			sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
			connectionProvider = sessions.ConnectionProvider as DebugConnectionProvider;
		}

		private async Task CleanupAsync()
		{
			if (sessions != null)
			{
				await (sessions.CloseAsync());
			}

			sessions = null;
			connectionProvider = null;
			lastOpenedSession = null;
			cfg = null;
		}

		public async Task<int> ExecuteStatementAsync(string sql)
		{
			if (cfg == null)
			{
				cfg = TestConfigurationHelper.GetDefaultConfiguration();
			}

			using (IConnectionProvider prov = ConnectionProviderFactory.NewConnectionProvider(cfg.Properties))
			{
				DbConnection conn = await (prov.GetConnectionAsync());
				try
				{
					using (DbTransaction tran = conn.BeginTransaction())
						using (DbCommand comm = conn.CreateCommand())
						{
							comm.CommandText = sql;
							comm.Transaction = tran;
							comm.CommandType = CommandType.Text;
							int result = await (comm.ExecuteNonQueryAsync());
							tran.Commit();
							return result;
						}
				}
				finally
				{
					prov.CloseConnection(conn);
				}
			}
		}

		public async Task<int> ExecuteStatementAsync(ISession session, ITransaction transaction, string sql)
		{
			using (DbCommand cmd = session.Connection.CreateCommand())
			{
				cmd.CommandText = sql;
				if (transaction != null)
					transaction.Enlist(cmd);
				return await (cmd.ExecuteNonQueryAsync());
			}
		}

		protected ISessionFactoryImplementor Sfi
		{
			get
			{
				return sessions;
			}
		}

		protected virtual ISession OpenSession()
		{
			lastOpenedSession = sessions.OpenSession();
			return lastOpenedSession;
		}

		protected virtual ISession OpenSession(IInterceptor sessionLocalInterceptor)
		{
			lastOpenedSession = sessions.OpenSession(sessionLocalInterceptor);
			return lastOpenedSession;
		}

		protected virtual void ApplyCacheSettings(Configuration configuration)
		{
			if (CacheConcurrencyStrategy == null)
			{
				return;
			}

			foreach (PersistentClass clazz in configuration.ClassMappings)
			{
				bool hasLob = false;
				foreach (Property prop in clazz.PropertyClosureIterator)
				{
					if (prop.Value.IsSimpleValue)
					{
						IType type = ((SimpleValue)prop.Value).Type;
						if (type == NHibernateUtil.BinaryBlob)
						{
							hasLob = true;
						}
					}
				}

				if (!hasLob && !clazz.IsInherited)
				{
					configuration.SetCacheConcurrencyStrategy(clazz.EntityName, CacheConcurrencyStrategy);
				}
			}

			foreach (Mapping.Collection coll in configuration.CollectionMappings)
			{
				configuration.SetCollectionCacheConcurrencyStrategy(coll.Role, CacheConcurrencyStrategy);
			}
		}

#region Properties overridable by subclasses
		protected virtual bool AppliesTo(Dialect.Dialect dialect)
		{
			return true;
		}

		protected virtual bool AppliesTo(ISessionFactoryImplementor factory)
		{
			return true;
		}

		protected virtual Task ConfigureAsync(Configuration configuration)
		{
			return TaskHelper.CompletedTask;
		}

		protected virtual string CacheConcurrencyStrategy
		{
			get
			{
				return "nonstrict-read-write";
			}
		//get { return null; }
		}
#endregion
	}
}
#endif
