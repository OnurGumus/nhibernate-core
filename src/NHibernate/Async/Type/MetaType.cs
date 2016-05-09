using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MetaType : AbstractType
	{
		public override async Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			object key = await (baseType.NullSafeGetAsync(rs, names, session, owner));
			return key == null ? null : values[key];
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
		{
			object key = await (baseType.NullSafeGetAsync(rs, name, session, owner));
			return key == null ? null : values[key];
		}

		public override async Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, System.Collections.IDictionary copiedAlready)
		{
			return original;
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return checkable[0] && await (IsDirtyAsync(old, current, session));
		}

		public override async Task<object> DeepCopyAsync(object value, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return value;
		}

		public override async Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
		{
			return await (baseType.ToColumnNullnessAsync(value, mapping));
		}

		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			return ToXMLString(value, factory);
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session)
		{
			await (baseType.NullSafeSetAsync(st, value == null ? null : keys[(string)value], index, session));
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			if (settable[0])
				await (NullSafeSetAsync(st, value, index, session));
		}
	}
}