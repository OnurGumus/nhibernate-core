#if NET_4_5
using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class ImmutableType : NullableType
	{
		public override Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			try
			{
				return Task.FromResult<object>(Replace(original, current, session, owner, copiedAlready));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
