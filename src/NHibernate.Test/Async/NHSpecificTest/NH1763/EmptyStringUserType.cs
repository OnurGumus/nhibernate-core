#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1763
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EmptyStringUserType : ICompositeUserType
	{
		public Task<object> AssembleAsync(object cached, NHibernate.Engine.ISessionImplementor session, object owner)
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

		public Task<object> DisassembleAsync(Object value, NHibernate.Engine.ISessionImplementor session)
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

		public Task<object> NullSafeGetAsync(DbDataReader rs, String[] names, NHibernate.Engine.ISessionImplementor session, Object owner)
		{
			return NHibernateUtil.String.NullSafeGetAsync(rs, names[0], session, owner);
		}

		public async Task NullSafeSetAsync(DbCommand st, Object value, int index, bool[] settable, NHibernate.Engine.ISessionImplementor session)
		{
			if (settable[0])
			{
				string str = null;
				if (value != null)
					str = value.ToString().Trim();
				if (str == String.Empty)
				{
					str = null;
					await (NHibernateUtil.String.NullSafeSetAsync(st, str, index, session));
				}
				else
					await (NHibernateUtil.String.NullSafeSetAsync(st, value, index, session));
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
