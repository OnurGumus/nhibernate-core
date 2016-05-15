#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.VersionTest.Db.MsSQL
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BinaryTimestamp : IUserVersionType
	{
		public Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(rs, names, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
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
