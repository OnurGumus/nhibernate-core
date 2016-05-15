#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.DialectTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DialectFixture
	{
		[Test]
		public async Task CurrentTimestampSelectionAsync()
		{
			var conf = TestConfigurationHelper.GetDefaultConfiguration();
			Dialect.Dialect dialect = Dialect.Dialect.GetDialect(conf.Properties);
			if (!dialect.SupportsCurrentTimestampSelection)
			{
				Assert.Ignore("This test does not apply to " + dialect.GetType().FullName);
			}

			var sessions = (ISessionFactoryImplementor)conf.BuildSessionFactory();
			sessions.ConnectionProvider.Configure(conf.Properties);
			IDriver driver = sessions.ConnectionProvider.Driver;
			using (DbConnection connection = await (sessions.ConnectionProvider.GetConnectionAsync()))
			{
				DbCommand statement = driver.GenerateCommand(CommandType.Text, new SqlString(dialect.CurrentTimestampSelectString), new SqlType[0]);
				statement.Connection = connection;
				using (DbDataReader reader = await (statement.ExecuteReaderAsync()))
				{
					Assert.That(await (reader.ReadAsync()), "should return one record");
					Assert.That(reader[0], Is.InstanceOf<DateTime>());
				}
			}
		}
	}
}
#endif
