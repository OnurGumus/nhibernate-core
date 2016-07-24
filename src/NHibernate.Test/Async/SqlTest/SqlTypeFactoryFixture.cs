#if NET_4_5
using System.Data;
using NHibernate.SqlTypes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlTypeFactoryFixtureAsync
	{
		[Test]
		[Description("Should cache constructed types")]
		public void GetSqlTypeWithPrecisionScale()
		{
			var st = SqlTypeFactory.GetSqlType(DbType.Decimal, 10, 2);
			Assert.That(st, Is.SameAs(SqlTypeFactory.GetSqlType(DbType.Decimal, 10, 2)));
			Assert.That(st, Is.Not.SameAs(SqlTypeFactory.GetSqlType(DbType.Decimal, 10, 1)));
			Assert.That(st, Is.Not.SameAs(SqlTypeFactory.GetSqlType(DbType.Double, 10, 2)));
		}
	}
}
#endif
