#if NET_4_5
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Custom.Firebird
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FireBirdTestAsync : CustomStoredProcSupportTestAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"SqlTest.Custom.Firebird.FireBirdEmployment.hbm.xml"};
			}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is FirebirdDialect;
		}
	}
}
#endif
