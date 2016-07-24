#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Connection;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.ConnectionStringTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NamedConnectionStringFixtureAsync
	{
		[Test]
		public Task InvalidNamedConnectedStringThrowsAsync()
		{
			try
			{
				Dictionary<string, string> settings = new Dictionary<string, string>();
				settings.Add(Environment.ConnectionStringName, "MyConStr");
				ConnectionProvider cp = new MockConnectionProvider();
				Assert.Throws<HibernateException>(() => cp.Configure(settings), "Could not find named connection string MyConStr");
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public void ConnectionStringInSettingsOverrideNamedConnectionSTring()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			string conStr = "Test Connection String";
			settings.Add(Environment.ConnectionString, conStr);
			settings.Add(Environment.ConnectionStringName, "MyConStr");
			MockConnectionProvider cp = new MockConnectionProvider();
			cp.Configure(settings);
			Assert.AreEqual(conStr, cp.PublicConnectionString);
		}

		[Test]
		public void CanGetNamedConnectionStringFromConfiguration()
		{
			Dictionary<string, string> settings = new Dictionary<string, string>();
			settings.Add(Environment.ConnectionStringName, "DummyConnectionString");
			MockConnectionProvider cp = new MockConnectionProvider();
			cp.Configure(settings);
			Assert.AreEqual("TestConnectionString-TestConnectionString", cp.PublicConnectionString);
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MockConnectionProvider : ConnectionProvider
	{
		/// <summary>
		/// Get an open <see cref = "DbConnection"/>.
		/// </summary>
		/// <returns>An open <see cref = "DbConnection"/>.</returns>
		public override Task<DbConnection> GetConnectionAsync()
		{
			try
			{
				return Task.FromResult<DbConnection>(GetConnection());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<DbConnection>(ex);
			}
		}
	}
}
#endif
