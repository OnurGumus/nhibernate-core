#if NET_4_5
using System;
using System.Data.Common;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ClassMetaType : AbstractType
	{
		public override Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, names[0], session, owner);
		}

		public override async Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			int index = rs.GetOrdinal(name);
			if (await (rs.IsDBNullAsync(index)))
			{
				return null;
			}
			else
			{
				string str = (string)NHibernateUtil.String.Get(rs, index);
				return string.IsNullOrEmpty(str) ? null : str;
			}
		}

		public override async Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			if (settable[0])
				await (NullSafeSetAsync(st, value, index, session));
		}

		public override Task NullSafeSetAsync(DbCommand st, object value, int index, ISessionImplementor session)
		{
			try
			{
				NullSafeSet(st, value, index, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
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
