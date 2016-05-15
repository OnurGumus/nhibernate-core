#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.DomainModel
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DoubleStringType : ICompositeUserType
	{
		public async Task<Object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, Object owner)
		{
			string first = (string)await (NHibernateUtil.String.NullSafeGetAsync(rs, names[0], session, owner));
			string second = (string)await (NHibernateUtil.String.NullSafeGetAsync(rs, names[1], session, owner));
			return (first == null && second == null) ? null : new string[]{first, second};
		}

		public async Task NullSafeSetAsync(DbCommand st, Object value, int index, bool[] settable, ISessionImplementor session)
		{
			string[] strings = (value == null) ? new string[2] : (string[])value;
			if (settable[0])
				await (NHibernateUtil.String.NullSafeSetAsync(st, strings[0], index++, session));
			if (settable[1])
				await (NHibernateUtil.String.NullSafeSetAsync(st, strings[1], index, session));
		}

		public Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Assemble(cached, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> DisassembleAsync(Object value, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Disassemble(value, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Replace(original, target, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
