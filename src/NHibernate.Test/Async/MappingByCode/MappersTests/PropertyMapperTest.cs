#if NET_4_5
using System;
using System.Linq;
using System.Data.Common;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Impl;
using NHibernate.Properties;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.UserTypes;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.MappingByCode.MappersTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MyType : IUserType
	{
		public Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(rs, names, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task NullSafeSetAsync(DbCommand cmd, object value, int index)
		{
			try
			{
				NullSafeSet(cmd, value, index);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MyCompoType : ICompositeUserType
	{
		public Task<object> NullSafeGetAsync(DbDataReader dr, string[] names, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(dr, names, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task NullSafeSetAsync(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
		{
			try
			{
				NullSafeSet(cmd, value, index, settable, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> DisassembleAsync(object value, ISessionImplementor session)
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
