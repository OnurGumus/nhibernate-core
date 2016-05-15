#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NHibernate.Transaction;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EmptyMappingsFixture : TestCase
	{
		[Test]
		public async Task InvalidQueryAsync()
		{
			try
			{
				using (ISession s = OpenSession())
				{
					await (s.CreateQuery("from SomeInvalidClass").ListAsync());
				}
			}
			catch (QueryException)
			{
			//
			}
		}

		[Test]
		public async Task DisconnectShouldNotCloseUserSuppliedConnectionAsync()
		{
			DbConnection conn = await (sessions.ConnectionProvider.GetConnectionAsync());
			try
			{
				using (ISession s = OpenSession())
				{
					s.Disconnect();
					s.Reconnect(conn);
					Assert.AreSame(conn, s.Disconnect());
					Assert.AreEqual(ConnectionState.Open, conn.State);
				}
			}
			finally
			{
				sessions.ConnectionProvider.CloseConnection(conn);
			}
		}
	}
}
#endif
