#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypeParameters
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultValueIntegerType : IUserType, IParameterizedType
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
	}
}
#endif
