#if NET_4_5
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Custom.MsSQL
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MSSQLTestAsync : CustomStoredProcSupportTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"SqlTest.Custom.MsSQL.MSSQLEmployment.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}
	}
}
#endif
