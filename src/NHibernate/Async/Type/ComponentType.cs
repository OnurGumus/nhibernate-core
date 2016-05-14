#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Tuple;
using NHibernate.Tuple.Component;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ComponentType : AbstractType, IAbstractComponentType
	{
		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			if (overridesGetHashCode)
				return x.GetHashCode();
			return await (GetHashCodeAsync(x, entityMode));
		}

		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			int result = 17;
			object[] values = await (GetPropertyValuesAsync(x, entityMode));
			unchecked
			{
				for (int i = 0; i < propertySpan; i++)
				{
					object y = values[i];
					result *= 37;
					if (y != null)
						result += await (propertyTypes[i].GetHashCodeAsync(y, entityMode));
				}
			}

			return result;
		}

		public override async Task<bool> IsDirtyAsync(object x, object y, ISessionImplementor session)
		{
			if (x == y)
			{
				return false;
			}

			/* 
			 * NH Different behavior : we don't use the shortcut because NH-1101 
			 * let the tuplizer choose how cosiderer properties when the component is null.
			 */
			EntityMode entityMode = session.EntityMode;
			if (entityMode != EntityMode.Poco && (x == null || y == null))
			{
				return true;
			}

			object[] xvalues = await (GetPropertyValuesAsync(x, entityMode));
			object[] yvalues = await (GetPropertyValuesAsync(y, entityMode));
			for (int i = 0; i < xvalues.Length; i++)
			{
				if (await (propertyTypes[i].IsDirtyAsync(xvalues[i], yvalues[i], session)))
				{
					return true;
				}
			}

			return false;
		}

		public override async Task<bool> IsDirtyAsync(object x, object y, bool[] checkable, ISessionImplementor session)
		{
			if (x == y)
			{
				return false;
			}

			/* 
			 * NH Different behavior : we don't use the shortcut because NH-1101 
			 * let the tuplizer choose how cosiderer properties when the component is null.
			 */
			EntityMode entityMode = session.EntityMode;
			if (entityMode != EntityMode.Poco && (x == null || y == null))
			{
				return true;
			}

			object[] xvalues = await (GetPropertyValuesAsync(x, entityMode));
			object[] yvalues = await (GetPropertyValuesAsync(y, entityMode));
			int loc = 0;
			for (int i = 0; i < xvalues.Length; i++)
			{
				int len = propertyTypes[i].GetColumnSpan(session.Factory);
				if (len <= 1)
				{
					bool dirty = (len == 0 || checkable[loc]) && await (propertyTypes[i].IsDirtyAsync(xvalues[i], yvalues[i], session));
					if (dirty)
					{
						return true;
					}
				}
				else
				{
					bool[] subcheckable = new bool[len];
					Array.Copy(checkable, loc, subcheckable, 0, len);
					bool dirty = await (propertyTypes[i].IsDirtyAsync(xvalues[i], yvalues[i], subcheckable, session));
					if (dirty)
					{
						return true;
					}
				}

				loc += len;
			}

			return false;
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return await (ResolveIdentifierAsync(await (HydrateAsync(rs, names, session, owner)), session, owner));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name = "st"></param>
		/// <param name = "value"></param>
		/// <param name = "begin"></param>
		/// <param name = "session"></param>
		public override async Task NullSafeSetAsync(IDbCommand st, object value, int begin, ISessionImplementor session)
		{
			object[] subvalues = await (NullSafeGetValuesAsync(value, session.EntityMode));
			for (int i = 0; i < propertySpan; i++)
			{
				await (propertyTypes[i].NullSafeSetAsync(st, subvalues[i], begin, session));
				begin += propertyTypes[i].GetColumnSpan(session.Factory);
			}
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int begin, bool[] settable, ISessionImplementor session)
		{
			object[] subvalues = await (NullSafeGetValuesAsync(value, session.EntityMode));
			int loc = 0;
			for (int i = 0; i < propertySpan; i++)
			{
				int len = propertyTypes[i].GetColumnSpan(session.Factory);
				if (len == 0)
				{
				//noop
				}
				else if (len == 1)
				{
					if (settable[loc])
					{
						await (propertyTypes[i].NullSafeSetAsync(st, subvalues[i], begin, session));
						begin++;
					}
				}
				else
				{
					bool[] subsettable = new bool[len];
					Array.Copy(settable, loc, subsettable, 0, len);
					await (propertyTypes[i].NullSafeSetAsync(st, subvalues[i], begin, subsettable, session));
					begin += ArrayHelper.CountTrue(subsettable);
				}

				loc += len;
			}
		}

		private async Task<object[]> NullSafeGetValuesAsync(object value, EntityMode entityMode)
		{
			if (value == null)
			{
				return new object[propertySpan];
			}
			else
			{
				return await (GetPropertyValuesAsync(value, entityMode));
			}
		}

		public override Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, new string[]{name}, session, owner);
		}

		public Task<object> GetPropertyValueAsync(object component, int i, ISessionImplementor session)
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

		public Task<object[]> GetPropertyValuesAsync(object component, EntityMode entityMode)
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

		public Task<object[]> GetPropertyValuesAsync(object component, ISessionImplementor session)
		{
			return GetPropertyValuesAsync(component, session.EntityMode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name = "value"></param>
		/// <param name = "factory"></param>
		/// <returns></returns>
		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			if (value == null)
			{
				return "null";
			}

			IDictionary<string, string> result = new Dictionary<string, string>();
			EntityMode? entityMode = tuplizerMapping.GuessEntityMode(value);
			if (!entityMode.HasValue)
			{
				throw new InvalidCastException(value.GetType().FullName);
			}

			object[] values = await (GetPropertyValuesAsync(value, entityMode.Value));
			for (int i = 0; i < propertyTypes.Length; i++)
			{
				result[propertyNames[i]] = await (propertyTypes[i].ToLoggableStringAsync(values[i], factory));
			}

			return StringHelper.Unqualify(Name) + CollectionPrinter.ToString(result);
		}

		public override async Task<object> DeepCopyAsync(object component, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			if (component == null)
			{
				return null;
			}

			object[] values = await (GetPropertyValuesAsync(component, entityMode));
			for (int i = 0; i < propertySpan; i++)
			{
				values[i] = await (propertyTypes[i].DeepCopyAsync(values[i], entityMode, factory));
			}

			object result = await (InstantiateAsync(entityMode));
			SetPropertyValues(result, values, entityMode);
			//not absolutely necessary, but helps for some
			//equals()/hashCode() implementations
			IComponentTuplizer ct = (IComponentTuplizer)tuplizerMapping.GetTuplizer(entityMode);
			if (ct.HasParentProperty)
			{
				ct.SetParent(result, ct.GetParent(component), factory);
			}

			return result;
		}

		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			if (original == null)
				return null;
			object result = target ?? await (InstantiateAsync(owner, session));
			EntityMode entityMode = session.EntityMode;
			object[] values = await (TypeHelper.ReplaceAsync(await (GetPropertyValuesAsync(original, entityMode)), await (GetPropertyValuesAsync(result, entityMode)), propertyTypes, session, owner, copiedAlready));
			SetPropertyValues(result, values, entityMode);
			return result;
		}

		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
		{
			if (original == null)
				return null;
			object result = target ?? await (InstantiateAsync(owner, session));
			EntityMode entityMode = session.EntityMode;
			object[] values = await (TypeHelper.ReplaceAsync(await (GetPropertyValuesAsync(original, entityMode)), await (GetPropertyValuesAsync(result, entityMode)), propertyTypes, session, owner, copyCache, foreignKeyDirection));
			SetPropertyValues(result, values, entityMode);
			return result;
		}

		/// <summary> This method does not populate the component parent</summary>
		public async Task<object> InstantiateAsync(EntityMode entityMode)
		{
			return await (tuplizerMapping.GetTuplizer(entityMode).InstantiateAsync());
		}

		public virtual async Task<object> InstantiateAsync(object parent, ISessionImplementor session)
		{
			object result = await (InstantiateAsync(session.EntityMode));
			IComponentTuplizer ct = (IComponentTuplizer)tuplizerMapping.GetTuplizer(session.EntityMode);
			if (ct.HasParentProperty && parent != null)
			{
				ct.SetParent(result, session.PersistenceContext.ProxyFor(parent), session.Factory);
			}

			return result;
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			if (value == null)
			{
				return null;
			}
			else
			{
				object[] values = await (GetPropertyValuesAsync(value, session.EntityMode));
				for (int i = 0; i < propertyTypes.Length; i++)
				{
					values[i] = await (propertyTypes[i].DisassembleAsync(values[i], session, owner));
				}

				return values;
			}
		}

		public override async Task<object> AssembleAsync(object obj, ISessionImplementor session, object owner)
		{
			if (obj == null)
			{
				return null;
			}
			else
			{
				object[] values = (object[])obj;
				object[] assembled = new object[values.Length];
				for (int i = 0; i < propertyTypes.Length; i++)
				{
					assembled[i] = await (propertyTypes[i].AssembleAsync(values[i], session, owner));
				}

				object result = await (InstantiateAsync(owner, session));
				SetPropertyValues(result, assembled, session.EntityMode);
				return result;
			}
		}

		public override async Task<object> HydrateAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			int begin = 0;
			bool notNull = false;
			object[] values = new object[propertySpan];
			for (int i = 0; i < propertySpan; i++)
			{
				int length = propertyTypes[i].GetColumnSpan(session.Factory);
				string[] range = ArrayHelper.Slice(names, begin, length); //cache this
				object val = await (propertyTypes[i].HydrateAsync(rs, range, session, owner));
				if (val == null)
				{
					if (isKey)
					{
						return null; //different nullability rules for pk/fk
					}
				}
				else
				{
					notNull = true;
				}

				values[i] = val;
				begin += length;
			}

			if (ReturnedClass.IsValueType)
				return values;
			else
				return notNull ? values : null;
		}

		public override async Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
		{
			if (value != null)
			{
				object result = await (InstantiateAsync(owner, session));
				object[] values = (object[])value;
				object[] resolvedValues = new object[values.Length]; //only really need new array during semiresolve!
				for (int i = 0; i < values.Length; i++)
				{
					resolvedValues[i] = await (propertyTypes[i].ResolveIdentifierAsync(values[i], session, owner));
				}

				SetPropertyValues(result, resolvedValues, session.EntityMode);
				return result;
			}
			else
			{
				return null;
			}
		}

		public override Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
		{
			//note that this implementation is kinda broken
			//for components with many-to-one associations
			return ResolveIdentifierAsync(value, session, owner);
		}

		public override async Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			if (current == null)
			{
				return old != null;
			}

			if (old == null)
			{
				return current != null;
			}

			object[] currentValues = await (GetPropertyValuesAsync(current, session));
			object[] oldValues = (Object[])old;
			int loc = 0;
			for (int i = 0; i < currentValues.Length; i++)
			{
				int len = propertyTypes[i].GetColumnSpan(session.Factory);
				bool[] subcheckable = new bool[len];
				Array.Copy(checkable, loc, subcheckable, 0, len);
				if (await (propertyTypes[i].IsModifiedAsync(oldValues[i], currentValues[i], subcheckable, session)))
				{
					return true;
				}

				loc += len;
			}

			return false;
		}

		public override async Task<int> CompareAsync(object x, object y, EntityMode? entityMode)
		{
			if (x == y)
			{
				return 0;
			}

			object[] xvalues = await (GetPropertyValuesAsync(x, entityMode.GetValueOrDefault()));
			object[] yvalues = await (GetPropertyValuesAsync(y, entityMode.GetValueOrDefault()));
			for (int i = 0; i < propertySpan; i++)
			{
				int propertyCompare = await (propertyTypes[i].CompareAsync(xvalues[i], yvalues[i], entityMode));
				if (propertyCompare != 0)
					return propertyCompare;
			}

			return 0;
		}

		public override async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
		{
			if (x == y)
			{
				return true;
			}

			if (x == null || y == null)
			{
				return false;
			}

			object[] xvalues = await (GetPropertyValuesAsync(x, entityMode));
			object[] yvalues = await (GetPropertyValuesAsync(y, entityMode));
			for (int i = 0; i < propertySpan; i++)
			{
				if (!await (propertyTypes[i].IsEqualAsync(xvalues[i], yvalues[i], entityMode)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			if (x == y)
			{
				return true;
			}

			if (x == null || y == null)
			{
				return false;
			}

			object[] xvalues = await (GetPropertyValuesAsync(x, entityMode));
			object[] yvalues = await (GetPropertyValuesAsync(y, entityMode));
			for (int i = 0; i < propertySpan; i++)
			{
				if (!await (propertyTypes[i].IsEqualAsync(xvalues[i], yvalues[i], entityMode, factory)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<bool> IsSameAsync(object x, object y, EntityMode entityMode)
		{
			if (x == y)
			{
				return true;
			}

			if (x == null || y == null)
			{
				return false;
			}

			object[] xvalues = await (GetPropertyValuesAsync(x, entityMode));
			object[] yvalues = await (GetPropertyValuesAsync(y, entityMode));
			for (int i = 0; i < propertySpan; i++)
			{
				if (!await (propertyTypes[i].IsSameAsync(xvalues[i], yvalues[i], entityMode)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
		{
			bool[] result = new bool[GetColumnSpan(mapping)];
			if (value == null)
			{
				return result;
			}

			object[] values = await (GetPropertyValuesAsync(value, EntityMode.Poco));
			int loc = 0;
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
#endif
