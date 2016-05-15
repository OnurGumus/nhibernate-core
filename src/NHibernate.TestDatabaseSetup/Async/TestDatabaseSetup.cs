#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using NHibernate.Test;
using Npgsql;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.TestDatabaseSetup
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DatabaseSetup
	{
		private static async Task SetupSqlServerAsync(Cfg.Configuration cfg)
		{
			var connStr = cfg.Properties[Cfg.Environment.ConnectionString];
			using (var conn = new SqlConnection(connStr.Replace("initial catalog=nhibernate", "initial catalog=master")))
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = "drop database nhibernate";
					try
					{
						await (cmd.ExecuteNonQueryAsync());
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}

					cmd.CommandText = "create database nhibernate";
					await (cmd.ExecuteNonQueryAsync());
				}
			}
		}

		private static async Task SetupSqlServerOdbcAsync(Cfg.Configuration cfg)
		{
			var connStr = cfg.Properties[Cfg.Environment.ConnectionString];
			using (var conn = new OdbcConnection(connStr.Replace("Database=nhibernateOdbc", "Database=master")))
			{
				await (conn.OpenAsync());
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = "drop database nhibernateOdbc";
					try
					{
						await (cmd.ExecuteNonQueryAsync());
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}

					cmd.CommandText = "create database nhibernateOdbc";
					await (cmd.ExecuteNonQueryAsync());
				}
			}
		}
	}
}
#endif
