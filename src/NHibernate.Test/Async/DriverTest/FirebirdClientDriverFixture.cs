﻿#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.DriverTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FirebirdClientDriverFixtureAsync
	{
		private string _connectionString;
		private FirebirdClientDriver _driver;
		[Test]
		public async Task ConnectionPooling_OpenThenCloseThenOpenAnotherOne_OnlyOneConnectionIsPooledAsync()
		{
			MakeDriver();
			var connection1 = await (MakeConnectionAsync());
			var connection2 = await (MakeConnectionAsync());
			//open first connection
			await (connection1.OpenAsync());
			await (VerifyCountOfEstablishedConnectionsIsAsync(1));
			//return it to the pool
			connection1.Close();
			await (VerifyCountOfEstablishedConnectionsIsAsync(1));
			//open the second connection
			await (connection2.OpenAsync());
			await (VerifyCountOfEstablishedConnectionsIsAsync(1));
			//return it to the pool
			connection2.Close();
			await (VerifyCountOfEstablishedConnectionsIsAsync(1));
		}

		[Test]
		public async Task ConnectionPooling_OpenThenCloseTwoAtTheSameTime_TowConnectionsArePooledAsync()
		{
			MakeDriver();
			var connection1 = await (MakeConnectionAsync());
			var connection2 = await (MakeConnectionAsync());
			//open first connection
			await (connection1.OpenAsync());
			await (VerifyCountOfEstablishedConnectionsIsAsync(1));
			//open second one
			await (connection2.OpenAsync());
			await (VerifyCountOfEstablishedConnectionsIsAsync(2));
			//return connection1 to the pool
			connection1.Close();
			await (VerifyCountOfEstablishedConnectionsIsAsync(2));
			//return connection2 to the pool
			connection2.Close();
			await (VerifyCountOfEstablishedConnectionsIsAsync(2));
		}

		private void MakeDriver()
		{
			var cfg = TestConfigurationHelper.GetDefaultConfiguration();
			var dlct = cfg.GetProperty("dialect");
			if (!dlct.Contains("Firebird"))
				Assert.Ignore("Applies only to Firebird");
			_driver = new FirebirdClientDriver();
			_connectionString = cfg.GetProperty("connection.connection_string");
		}

		private async Task<DbConnection> MakeConnectionAsync()
		{
			var result = await (_driver.CreateConnectionAsync());
			result.ConnectionString = _connectionString;
			return result;
		}

		private async Task VerifyCountOfEstablishedConnectionsIsAsync(int expectedCount)
		{
			var physicalConnections = await (GetEstablishedConnectionsAsync());
			Assert.That(physicalConnections, Is.EqualTo(expectedCount));
		}

		private async Task<int> GetEstablishedConnectionsAsync()
		{
			using (var conn = await (_driver.CreateConnectionAsync()))
			{
				conn.ConnectionString = _connectionString;
				await (conn.OpenAsync());
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = "select count(*) from mon$attachments where mon$attachment_id <> current_connection";
					return (int)await (cmd.ExecuteScalarAsync());
				}
			}
		}

		private DbCommand BuildSelectCaseCommand(SqlType paramType)
		{
			var sqlString = new SqlStringBuilder().Add("select (case when col = ").AddParameter().Add(" then ").AddParameter().Add(" else ").AddParameter().Add(" end) from table").ToSqlString();
			return _driver.GenerateCommand(CommandType.Text, sqlString, new[]{paramType, paramType, paramType});
		}

		private DbCommand BuildSelectConcatCommand(SqlType paramType)
		{
			var sqlString = new SqlStringBuilder().Add("select col || ").AddParameter().Add(" || ").Add("col ").Add("from table").ToSqlString();
			return _driver.GenerateCommand(CommandType.Text, sqlString, new[]{paramType});
		}

		private DbCommand BuildSelectAddCommand(SqlType paramType)
		{
			var sqlString = new SqlStringBuilder().Add("select col + ").AddParameter().Add(" from table").ToSqlString();
			return _driver.GenerateCommand(CommandType.Text, sqlString, new[]{paramType});
		}

		private DbCommand BuildInsertWithParamsInSelectCommand(SqlType paramType)
		{
			var sqlString = new SqlStringBuilder().Add("insert into table1 (col1, col2) ").Add("select col1, ").AddParameter().Add(" from table2").ToSqlString();
			return _driver.GenerateCommand(CommandType.Text, sqlString, new[]{paramType});
		}

		private DbCommand BuildInsertWithParamsInSelectCommandWithSelectInColumnName(SqlType paramType)
		{
			var sqlString = new SqlStringBuilder().Add("insert into table1 (col1_select_aaa) ").Add("values(").AddParameter().Add(") from table2").ToSqlString();
			return _driver.GenerateCommand(CommandType.Text, sqlString, new[]{paramType});
		}

		private DbCommand BuildInsertWithParamsInSelectCommandWithWhereInColumnName(SqlType paramType)
		{
			var sqlString = new SqlStringBuilder().Add("insert into table1 (col1_where_aaa) ").Add("values(").AddParameter().Add(") from table2").ToSqlString();
			return _driver.GenerateCommand(CommandType.Text, sqlString, new[]{paramType});
		}
	}
}
#endif
