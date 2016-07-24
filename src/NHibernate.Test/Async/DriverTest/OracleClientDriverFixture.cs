#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.DriverTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OracleClientDriverFixtureAsync
	{
		/// <summary>
		/// Verify that the correct Connection Class is being loaded.
		/// </summary>
		[Test]
		public async Task ConnectionClassNameAsync()
		{
			IDriver driver = new OracleClientDriver();
			DbConnection conn = await (driver.CreateConnectionAsync());
			Assert.AreEqual("System.Data.OracleClient.OracleConnection", conn.GetType().FullName);
		}

		/// <summary>
		/// Verify that the correct Command class is being loaded.
		/// </summary>
		[Test]
		public void CommandClassName()
		{
			OracleClientDriver driver = new OracleClientDriver();
			DbCommand cmd = driver.CreateCommand();
			Assert.AreEqual("System.Data.OracleClient.OracleCommand", cmd.GetType().FullName);
		}
	}
}
#endif
