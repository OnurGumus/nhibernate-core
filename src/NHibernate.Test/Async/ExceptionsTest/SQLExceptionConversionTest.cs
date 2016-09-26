#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NHibernate.Dialect;
using NHibernate.Exceptions;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExceptionsTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SQLExceptionConversionTestAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"ExceptionsTest.User.hbm.xml", "ExceptionsTest.Group.hbm.xml"};
			}
		}

		protected override void Configure(Cfg.Configuration configuration)
		{
			if (Dialect is MsSql2000Dialect)
			{
				configuration.SetProperty(Cfg.Environment.SqlExceptionConverter, typeof (MSSQLExceptionConverterExample).AssemblyQualifiedName);
			}

			if (Dialect is Oracle8iDialect)
			{
				configuration.SetProperty(Cfg.Environment.SqlExceptionConverter, typeof (OracleClientExceptionConverterExample).AssemblyQualifiedName);
			}

			if (Dialect is PostgreSQLDialect)
			{
				configuration.SetProperty(Cfg.Environment.SqlExceptionConverter, typeof (PostgresExceptionConverterExample).AssemblyQualifiedName);
			}

			if (Dialect is FirebirdDialect)
			{
				configuration.SetProperty(Cfg.Environment.SqlExceptionConverter, typeof (FbExceptionConverterExample).AssemblyQualifiedName);
			}
		}

		[Test]
		public async Task IntegrityViolationAsync()
		{
			if (Dialect is SQLiteDialect)
				Assert.Ignore("Example exception converter not implemented.");
			//ISQLExceptionConverter converter = Dialect.BuildSQLExceptionConverter();
			ISQLExceptionConverter converter = sessions.Settings.SqlExceptionConverter;
			ISession session = OpenSession();
			session.BeginTransaction();
			DbConnection connection = session.Connection;
			// Attempt to insert some bad values into the T_MEMBERSHIP table that should
			// result in a constraint violation
			DbCommand ps = null;
			try
			{
				ps = connection.CreateCommand();
				ps.CommandType = CommandType.Text;
				ps.CommandText = "INSERT INTO T_MEMBERSHIP (user_id, group_id) VALUES (@p1, @p2)";
				DbParameter pr = ps.CreateParameter();
				pr.ParameterName = "p1";
				pr.DbType = DbType.Int64;
				pr.Value = 52134241L; // Non-existent user_id
				ps.Parameters.Add(pr);
				pr = ps.CreateParameter();
				pr.ParameterName = "p2";
				pr.DbType = DbType.Int64;
				pr.Value = 5342L; // Non-existent group_id
				ps.Parameters.Add(pr);
				session.Transaction.Enlist(ps);
				await (ps.ExecuteNonQueryAsync());
				Assert.Fail("INSERT should have failed");
			}
			catch (Exception sqle)
			{
				ADOExceptionReporter.LogExceptions(sqle, "Just output!!!!");
				Exception adoException = converter.Convert(new AdoExceptionContextInfo{SqlException = sqle});
				Assert.AreEqual(typeof (ConstraintViolationException), adoException.GetType(), "Bad conversion [" + sqle.Message + "]");
				ConstraintViolationException ex = (ConstraintViolationException)adoException;
				Console.WriteLine("Violated constraint name: " + ex.ConstraintName);
			}
			finally
			{
				if (ps != null)
				{
					try
					{
						ps.Dispose();
					}
					catch (Exception)
					{
					// ignore...
					}
				}
			}

			session.Transaction.Rollback();
			session.Close();
		}

		[Test]
		public async Task BadGrammarAsync()
		{
			if (Dialect is SQLiteDialect)
				Assert.Ignore("Example exception converter not implemented.");
			//ISQLExceptionConverter converter = Dialect.BuildSQLExceptionConverter();
			ISQLExceptionConverter converter = sessions.Settings.SqlExceptionConverter;
			ISession session = OpenSession();
			DbConnection connection = session.Connection;
			// prepare/execute a query against a non-existent table
			DbCommand ps = null;
			try
			{
				ps = connection.CreateCommand();
				ps.CommandType = CommandType.Text;
				ps.CommandText = "SELECT user_id, user_name FROM tbl_no_there";
				await (ps.ExecuteNonQueryAsync());
				Assert.Fail("SQL compilation should have failed");
			}
			catch (Exception sqle)
			{
				Assert.AreEqual(typeof (SQLGrammarException), converter.Convert(new AdoExceptionContextInfo{SqlException = sqle}).GetType(), "Bad conversion [" + sqle.Message + "]");
			}
			finally
			{
				if (ps != null)
				{
					try
					{
						ps.Dispose();
					}
					catch (Exception)
					{
					// ignore...
					}
				}
			}

			session.Close();
		}
	}
}
#endif
