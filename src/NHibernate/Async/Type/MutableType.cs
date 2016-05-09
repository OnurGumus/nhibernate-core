using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class MutableType : NullableType
	{
		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			if (await (IsEqualAsync(original, target, session.EntityMode)))
				return original;
			return await (DeepCopyAsync(original, session.EntityMode, session.Factory));
		}

		public override async Task<object> DeepCopyAsync(object value, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return (value == null) ? null : DeepCopyNotNull(value);
		}
	}
}