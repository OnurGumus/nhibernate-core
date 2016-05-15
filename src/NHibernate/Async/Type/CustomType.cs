#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NHibernate.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomType : AbstractType, IDiscriminatorType, IVersionType
	{
		public override Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return userType.NullSafeGetAsync(rs, names, owner);
		}

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, new string[]{name}, session, owner);
		}

		public override async Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			if (settable[0])
				await (userType.NullSafeSetAsync(st, value, index));
		}

		public override Task NullSafeSetAsync(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			return userType.NullSafeSetAsync(cmd, value, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name = "value"></param>
		/// <param name = "factory"></param>
		/// <returns></returns>
		public override Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			try
			{
				return Task.FromResult<string>(ToLoggableString(value, factory));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<string>(ex);
			}
		}

		public override Task<object> DeepCopyAsync(object value, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			try
			{
				return Task.FromResult<object>(DeepCopy(value, entityMode, factory));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<int>(GetHashCode(x, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int>(ex);
			}
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return checkable[0] && await (IsDirtyAsync(old, current, session));
		}

		public Task<object> NextAsync(object current, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Next(current, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> SeedAsync(ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Seed(session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
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

		public override Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<bool>(IsEqual(x, y, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public override Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
		{
			try
			{
				return Task.FromResult<bool[]>(ToColumnNullness(value, mapping));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool[]>(ex);
			}
		}
	}
}
#endif
