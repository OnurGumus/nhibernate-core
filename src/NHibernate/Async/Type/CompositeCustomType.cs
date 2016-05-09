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

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CompositeCustomType : AbstractType, IAbstractComponentType
	{
		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			return userType.Assemble(cached, session, owner);
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return userType.NullSafeGet(rs, names, session, owner);
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return userType.NullSafeGet(rs, new string[]{name}, session, owner);
		}

		public override async Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			return userType.Replace(original, current, session, owner);
		}

		public virtual async Task<object> GetPropertyValueAsync(object component, int i, ISessionImplementor session)
		{
			return GetPropertyValue(component, i);
		}

		public virtual async Task<object[]> GetPropertyValuesAsync(object component, ISessionImplementor session)
		{
			return await (GetPropertyValuesAsync(component, session.EntityMode));
		}

		public virtual async Task<object[]> GetPropertyValuesAsync(object component, EntityMode entityMode)
		{
			int len = Subtypes.Length;
			object[] result = new object[len];
			for (int i = 0; i < len; i++)
			{
				result[i] = GetPropertyValue(component, i);
			}

			return result;
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return await (IsDirtyAsync(old, current, session));
		}

		public override async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
		{
			return userType.Equals(x, y);
		}

		public override async Task<object> DeepCopyAsync(object value, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return userType.DeepCopy(value);
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			return userType.Disassemble(value, session);
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

		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			return value == null ? "null" : value.ToString();
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			userType.NullSafeSet(st, value, index, settable, session);
		}

		public override async Task NullSafeSetAsync(IDbCommand cmd, object value, int index, ISessionImplementor session)
		{
			bool[] settable = Enumerable.Repeat(true, GetColumnSpan(session.Factory)).ToArray();
			userType.NullSafeSet(cmd, value, index, settable, session);
		}
	}
}