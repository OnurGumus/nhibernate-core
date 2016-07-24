#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;

namespace NHibernate.Test.SqlTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NullDateUserType : IUserType
	{
		public async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner)
		{
			int ordinal = rs.GetOrdinal(names[0]);
			if (await (rs.IsDBNullAsync(ordinal)))
			{
				return DateTime.MinValue;
			}
			else
			{
				return rs.GetDateTime(ordinal);
			}
		}
	}
}
#endif
