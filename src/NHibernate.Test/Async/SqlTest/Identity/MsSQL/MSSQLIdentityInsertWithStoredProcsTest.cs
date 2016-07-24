#if NET_4_5
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest.Identity.MsSQL
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MSSQLIdentityInsertWithStoredProcsTestAsync : IdentityInsertWithStoredProcsTestAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		protected override string GetExpectedInsertOrgLogStatement(string orgName)
		{
			return string.Format("exec nh_organization_native_id_insert @p0;@p0 = '{0}' [Type: String (4000)]", orgName);
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"SqlTest.Identity.MsSQL.MSSQLIdentityInsertWithStoredProcs.hbm.xml"};
			}
		}
	}
}
#endif
