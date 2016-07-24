#if NET_4_5
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Custom.MySQL
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MySQLTestAsync : CustomStoredProcSupportTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"SqlTest.Custom.MySQL.MySQLEmployment.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MySQL5Dialect || dialect is MySQLDialect;
		}
	}
}
#endif
