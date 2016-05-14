#if NET_4_5
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SpecialOneToOneType : OneToOneType
	{
		public override async Task<object> HydrateAsync(DbDataReader rs, string[] names, Engine.ISessionImplementor session, object owner)
		{
			return await (GetIdentifierOrUniqueKeyType(session.Factory).NullSafeGetAsync(rs, names, session, owner));
		}
	}
}
#endif
