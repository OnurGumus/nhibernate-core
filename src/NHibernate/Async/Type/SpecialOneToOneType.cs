using System;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SpecialOneToOneType : OneToOneType
	{
		public override async Task<object> HydrateAsync(System.Data.IDataReader rs, string[] names, Engine.ISessionImplementor session, object owner)
		{
			return await (GetIdentifierOrUniqueKeyType(session.Factory).NullSafeGetAsync(rs, names, session, owner));
		}
	}
}