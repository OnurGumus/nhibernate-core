#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.DriverTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OracleDataClientDriverFixtureAsync
	{
		/// <summary>
		/// Testing NH-302 to verify that a DbType.Boolean gets replaced
		/// with an appropriate type.
		/// </summary>
		[Test]
		[Category("ODP.NET")]
		[Explicit]
		public void NoBooleanParameters()
		{
			OracleDataClientDriver driver = new OracleDataClientDriver();
			SqlStringBuilder builder = new SqlStringBuilder();
			builder.Add("select * from table1 where col1=");
			builder.Add(Parameter.Placeholder);
			DbCommand cmd = driver.GenerateCommand(CommandType.Text, builder.ToSqlString(), new SqlType[]{SqlTypeFactory.Boolean});
			DbParameter param = cmd.Parameters[0] as DbParameter;
			Assert.AreEqual("col1", param.ParameterName, "kept same param name");
			Assert.IsFalse(param.DbType == DbType.Boolean, "should not still be a DbType.Boolean");
		}
	}
}
#endif
