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
	public abstract partial class MutableType : NullableType
	{
		public override Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			try
			{
				return Task.FromResult<object>(Replace(original, target, session, owner, copiedAlready));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
