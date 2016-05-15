#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1907
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleCustomType : IUserType
	{
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

		public async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner)
		{
			int index0 = rs.GetOrdinal(names[0]);
			if (await (rs.IsDBNullAsync(index0)))
			{
				return null;
			}

			int value = rs.GetInt32(index0);
			return new MyType{ToPersist = value};
		}
	}
}
#endif
