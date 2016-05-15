#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

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

		public Task NullSafeSetAsync(DbCommand cmd, object value, int index)
		{
			try
			{
				NullSafeSet(cmd, value, index);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
