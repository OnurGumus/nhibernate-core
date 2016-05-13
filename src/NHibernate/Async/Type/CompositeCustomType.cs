using System;
using System.Collections;
using System.Data;
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
			return GetPropertyValuesAsync(component, session.EntityMode);
		}

		public virtual Task<object[]> GetPropertyValuesAsync(object component, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<object[]>(GetPropertyValues(component, entityMode));
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

		public override Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
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

		public override Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
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

		public override Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
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

		public override Task NullSafeSetAsync(IDbCommand cmd, object value, int index, ISessionImplementor session)
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

		public override async Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
		{
			bool[] result = new bool[GetColumnSpan(mapping)];
			if (value == null)
				return result;
			object[] values = await (GetPropertyValuesAsync(value, EntityMode.Poco));
			int loc = 0;
			IType[] propertyTypes = Subtypes;
			for (int i = 0; i < propertyTypes.Length; i++)
			{
				bool[] propertyNullness = await (propertyTypes[i].ToColumnNullnessAsync(values[i], mapping));
				Array.Copy(propertyNullness, 0, result, loc, propertyNullness.Length);
				loc += propertyNullness.Length;
			}

			return result;
		}
	}
}