#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CompositeCustomType : AbstractType, IAbstractComponentType
	{
		public virtual Task<object[]> GetPropertyValuesAsync(object component, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object[]>(GetPropertyValues(component, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object[]>(ex);
			}
		}

		public virtual Task<object> GetPropertyValueAsync(object component, int i, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(GetPropertyValue(component, i, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
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

		public override Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Disassemble(value, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(rs, name, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(rs, names, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			try
			{
				NullSafeSet(st, value, index, settable, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task NullSafeSetAsync(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			try
			{
				NullSafeSet(cmd, value, index, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return IsDirtyAsync(old, current, session);
		}

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
