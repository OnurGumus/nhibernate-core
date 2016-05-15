#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.DriverTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FirebirdClientDriverFixture
	{
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
	}
}
#endif
