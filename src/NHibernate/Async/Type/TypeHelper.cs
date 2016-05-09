using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Properties;
using NHibernate.Tuple;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	/// <summary>
	/// Collection of convenience methods relating to operations across arrays of types...
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class TypeHelper
	{
		public static async Task BeforeAssembleAsync(object[] row, ICacheAssembler[] types, ISessionImplementor session)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (!Equals(LazyPropertyInitializer.UnfetchedProperty, row[i]) && !Equals(BackrefPropertyAccessor.Unknown, row[i]))
				{
					await (types[i].BeforeAssembleAsync(row[i], session));
				}
			}
		}

		public static async Task<object[]> AssembleAsync(object[] row, ICacheAssembler[] types, ISessionImplementor session, object owner)
		{
			var assembled = new object[row.Length];
			for (int i = 0; i < row.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, row[i]) || Equals(BackrefPropertyAccessor.Unknown, row[i]))
				{
					assembled[i] = row[i];
				}
				else
				{
					assembled[i] = await (types[i].AssembleAsync(row[i], session, owner));
				}
			}

			return assembled;
		}

		public static async Task<object[]> ReplaceAsync(object[] original, object[] target, IType[] types, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			var copied = new object[original.Length];
			for (int i = 0; i < original.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, original[i]) || Equals(BackrefPropertyAccessor.Unknown, original[i]))
				{
					copied[i] = target[i];
				}
				else
				{
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copiedAlready));
				}
			}

			return copied;
		}

		public static async Task<object[]> ReplaceAsync(object[] original, object[] target, IType[] types, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
		{
			object[] copied = new object[original.Length];
			for (int i = 0; i < types.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, original[i]) || Equals(BackrefPropertyAccessor.Unknown, original[i]))
				{
					copied[i] = target[i];
				}
				else
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copyCache, foreignKeyDirection));
			}

			return copied;
		}

		public static async Task<object[]> ReplaceAssociationsAsync(object[] original, object[] target, IType[] types, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
		{
			object[] copied = new object[original.Length];
			for (int i = 0; i < types.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, original[i]) || Equals(BackrefPropertyAccessor.Unknown, original[i]))
				{
					copied[i] = target[i];
				}
				else if (types[i].IsComponentType)
				{
					// need to extract the component values and check for subtype replacements...
					IAbstractComponentType componentType = (IAbstractComponentType)types[i];
					IType[] subtypes = componentType.Subtypes;
					object[] origComponentValues = original[i] == null ? new object[subtypes.Length] : await (componentType.GetPropertyValuesAsync(original[i], session));
					object[] targetComponentValues = target[i] == null ? new object[subtypes.Length] : await (componentType.GetPropertyValuesAsync(target[i], session));
					object[] componentCopy = await (ReplaceAssociationsAsync(origComponentValues, targetComponentValues, subtypes, session, null, copyCache, foreignKeyDirection));
					if (!componentType.IsAnyType && target[i] != null)
						componentType.SetPropertyValues(target[i], componentCopy, session.EntityMode);
					copied[i] = target[i];
				}
				else if (!types[i].IsAssociationType)
				{
					copied[i] = target[i];
				}
				else
				{
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copyCache, foreignKeyDirection));
				}
			}

			return copied;
		}

		public static async Task<int[]> FindModifiedAsync(StandardProperty[] properties, object[] currentState, object[] previousState, bool[][] includeColumns, bool anyUninitializedProperties, ISessionImplementor session)
		{
			int[] results = null;
			int count = 0;
			int span = properties.Length;
			for (int i = 0; i < span; i++)
			{
				bool dirty = !Equals(LazyPropertyInitializer.UnfetchedProperty, currentState[i]) && properties[i].IsDirtyCheckable(anyUninitializedProperties) && await (properties[i].Type.IsModifiedAsync(previousState[i], currentState[i], includeColumns[i], session));
				if (dirty)
				{
					if (results == null)
					{
						results = new int[span];
					}

					results[count++] = i;
				}
			}

			if (count == 0)
			{
				return null;
			}
			else
			{
				int[] trimmed = new int[count];
				Array.Copy(results, 0, trimmed, 0, count);
				return trimmed;
			}
		}

		private static async Task<bool> DirtyAsync(StandardProperty[] properties, object[] currentState, object[] previousState, bool[][] includeColumns, bool anyUninitializedProperties, ISessionImplementor session, int i)
		{
			if (Equals(LazyPropertyInitializer.UnfetchedProperty, currentState[i]))
				return false;
			if (Equals(LazyPropertyInitializer.UnfetchedProperty, previousState[i]))
				return true;
			return properties[i].IsDirtyCheckable(anyUninitializedProperties) && await (properties[i].Type.IsDirtyAsync(previousState[i], currentState[i], includeColumns[i], session));
		}

		public static async Task<int[]> FindDirtyAsync(StandardProperty[] properties, object[] currentState, object[] previousState, bool[][] includeColumns, bool anyUninitializedProperties, ISessionImplementor session)
		{
			int[] results = null;
			int count = 0;
			int span = properties.Length;
			for (int i = 0; i < span; i++)
			{
				var dirty = await (DirtyAsync(properties, currentState, previousState, includeColumns, anyUninitializedProperties, session, i));
				if (dirty)
				{
					if (results == null)
					{
						results = new int[span];
					}

					results[count++] = i;
				}
			}

			if (count == 0)
			{
				return null;
			}
			else
			{
				int[] trimmed = new int[count];
				Array.Copy(results, 0, trimmed, 0, count);
				return trimmed;
			}
		}

		public static async Task DeepCopyAsync(object[] values, IType[] types, bool[] copy, object[] target, ISessionImplementor session)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (copy[i])
				{
					if (Equals(LazyPropertyInitializer.UnfetchedProperty, values[i]) || Equals(BackrefPropertyAccessor.Unknown, values[i]))
					{
						target[i] = values[i];
					}
					else
					{
						target[i] = await (types[i].DeepCopyAsync(values[i], session.EntityMode, session.Factory));
					}
				}
			}
		}

		public static async Task<object[]> DisassembleAsync(object[] row, ICacheAssembler[] types, bool[] nonCacheable, ISessionImplementor session, object owner)
		{
			object[] disassembled = new object[row.Length];
			for (int i = 0; i < row.Length; i++)
			{
				if (nonCacheable != null && nonCacheable[i])
				{
					disassembled[i] = LazyPropertyInitializer.UnfetchedProperty;
				}
				else if (Equals(LazyPropertyInitializer.UnfetchedProperty, row[i]) || Equals(BackrefPropertyAccessor.Unknown, row[i]))
				{
					disassembled[i] = row[i];
				}
				else
				{
					disassembled[i] = await (types[i].DisassembleAsync(row[i], session, owner));
				}
			}

			return disassembled;
		}
	}
}