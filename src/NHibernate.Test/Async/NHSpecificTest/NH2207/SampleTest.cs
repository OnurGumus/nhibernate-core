#if NET_4_5
using System;
using System.Data;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2207
{
	[TestFixture, Ignore("Demostration of external issue")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTestAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect as MsSql2008Dialect != null;
		}

		[Test]
		public async Task WithoutUseNHSqlDataProviderWorkProperlyAsync()
		{
			var createTable = "CREATE TABLE TryDate([Id] [int] IDENTITY(1,1) NOT NULL,[MyDate] [date] NOT NULL)";
			var dropTable = "DROP TABLE TryDate";
			var insertTable = "INSERT INTO TryDate([MyDate]) VALUES(@p0)";
			using (var sqlConnection = new System.Data.SqlClient.SqlConnection(cfg.Properties[Cfg.Environment.ConnectionString]))
			{
				await (sqlConnection.OpenAsync());
				using (var tx = sqlConnection.BeginTransaction())
				{
					var command = sqlConnection.CreateCommand();
					command.Transaction = tx;
					command.CommandText = createTable;
					await (command.ExecuteNonQueryAsync());
					tx.Commit();
				}

				try
				{
					using (var tx = sqlConnection.BeginTransaction())
					{
						var command = sqlConnection.CreateCommand();
						command.Transaction = tx;
						command.CommandText = insertTable;
						var dateParam = command.CreateParameter();
						dateParam.ParameterName = "@p0";
						dateParam.DbType = DbType.Date;
						dateParam.Value = DateTime.MinValue.Date;
						command.Parameters.Add(dateParam);
						await (command.ExecuteNonQueryAsync());
						tx.Commit();
					}
				}
				finally
				{
					using (var tx = sqlConnection.BeginTransaction())
					{
						var command = sqlConnection.CreateCommand();
						command.Transaction = tx;
						command.CommandText = dropTable;
						await (command.ExecuteNonQueryAsync());
						tx.Commit();
					}
				}
			}
		}

		[Test]
		public async Task Dates_Before_1753_Should_Not_Insert_NullAsync()
		{
			object savedId;
			var expectedStoredValue = DateTime.MinValue.Date.AddDays(1).Date;
			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var concrete = new DomainClass{Date = expectedStoredValue.AddMinutes(90)};
					savedId = await (session.SaveAsync(concrete));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var savedObj = await (session.GetAsync<DomainClass>(savedId));
					Assert.That(savedObj.Date, Is.EqualTo(expectedStoredValue));
					await (session.DeleteAsync(savedObj));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
