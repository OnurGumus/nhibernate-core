#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeType : PrimitiveType, IIdentifierType, ILiteralType, IVersionType
	{
		public virtual Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return SeedAsync(session);
		}

		public virtual Task<object> SeedAsync(ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Seed(session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<int>(GetHashCode(x, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int>(ex);
			}
		}
	}
}
#endif
