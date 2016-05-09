using System;
using System.Data;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ClassMetaType : AbstractType
	{
		public override async Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return await (NullSafeGetAsync(rs, names[0], session, owner));
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
		{
			int index = rs.GetOrdinal(name);
			if (rs.IsDBNull(index))
			{
				return null;
			}
			else
			{
				string str = (string)NHibernateUtil.String.Get(rs, index);
				return string.IsNullOrEmpty(str) ? null : str;
			}
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
			throw new NotSupportedException();
		}

		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			return ToXMLString(value, factory);
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session)
		{
			if (value == null)
			{
				((IDataParameter)st.Parameters[index]).Value = DBNull.Value;
			}
			else
			{
				NHibernateUtil.String.Set(st, value, index);
			}
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			if (settable[0])
				await (NullSafeSetAsync(st, value, index, session));
		}
	}
}