#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MetaType : AbstractType
	{
		public override async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			object key = await (baseType.NullSafeGetAsync(rs, names, session, owner));
			return key == null ? null : values[key];
		}

		public override async Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			object key = await (baseType.NullSafeGetAsync(rs, name, session, owner));
			return key == null ? null : values[key];
		}

		public override async Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			if (settable[0])
				await (NullSafeSetAsync(st, value, index, session));
		}

		public override Task NullSafeSetAsync(DbCommand st, object value, int index, ISessionImplementor session)
		{
			return baseType.NullSafeSetAsync(st, value == null ? null : keys[(string)value], index, session);
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return checkable[0] && await (IsDirtyAsync(old, current, session));
		}

		public override Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, System.Collections.IDictionary copiedAlready)
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
