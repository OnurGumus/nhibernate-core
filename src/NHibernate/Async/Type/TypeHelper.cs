﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Properties;
using NHibernate.Tuple;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public static partial class TypeHelper
	{
		
		/// <summary>Apply the <see cref="ICacheAssembler.BeforeAssembleAsync(object,NHibernate.Engine.ISessionImplementor)" /> operation across a series of values.</summary>
		/// <param name="row">The values</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		public static async Task BeforeAssembleAsync(object[] row, ICacheAssembler[] types, ISessionImplementor session)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (!Equals(LazyPropertyInitializer.UnfetchedProperty, row[i]) && !Equals(BackrefPropertyAccessor.Unknown, row[i]))
				{
					await (types[i].BeforeAssembleAsync(row[i], session)).ConfigureAwait(false);
				}
			}
		}
		
		/// <summary>
		/// Apply the <see cref="ICacheAssembler.AssembleAsync(object,NHibernate.Engine.ISessionImplementor,object)" /> operation across a series of values.
		/// </summary>
		/// <param name="row">The values</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <returns></returns>
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
					assembled[i] = await (types[i].AssembleAsync(row[i], session, owner)).ConfigureAwait(false);
				}
			}
			return assembled;
		}
		
		/// <summary>
		/// Apply the <see cref="IType.ReplaceAsync(object, object, ISessionImplementor, object, IDictionary)" /> operation across a series of values.
		/// </summary>
		/// <param name="original">The source of the state</param>
		/// <param name="target">The target into which to replace the source values.</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <param name="copiedAlready">Represent a cache of already replaced state</param>
		/// <returns> The replaced state</returns>
		public static async Task<object[]> ReplaceAsync(object[] original, object[] target, IType[] types, ISessionImplementor session,
																	 object owner, IDictionary copiedAlready)
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
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copiedAlready)).ConfigureAwait(false);
				}
			}
			return copied;
		}

		/// <summary>
		/// Apply the <see cref="IType.ReplaceAsync(object, object, ISessionImplementor, object, IDictionary, ForeignKeyDirection)" />
		/// operation across a series of values.
		/// </summary>
		/// <param name="original">The source of the state</param>
		/// <param name="target">The target into which to replace the source values.</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <param name="copyCache">A map representing a cache of already replaced state</param>
		/// <param name="foreignKeyDirection">FK directionality to be applied to the replacement</param>
		/// <returns> The replaced state</returns>
		public static async Task<object[]> ReplaceAsync(object[] original, object[] target, IType[] types,
			ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
		{
			object[] copied = new object[original.Length];
			for (int i = 0; i < types.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, original[i]) || Equals(BackrefPropertyAccessor.Unknown, original[i]))
				{
					copied[i] = target[i];
				}
				else
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copyCache, foreignKeyDirection)).ConfigureAwait(false);
			}
			return copied;
		}

		/// <summary>
		/// Apply the <see cref="IType.ReplaceAsync(object, object, ISessionImplementor, object, IDictionary, ForeignKeyDirection)" />
		/// operation across a series of values, as long as the corresponding <see cref="IType"/> is an association.
		/// </summary>
		/// <param name="original">The source of the state</param>
		/// <param name="target">The target into which to replace the source values.</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <param name="copyCache">A map representing a cache of already replaced state</param>
		/// <param name="foreignKeyDirection">FK directionality to be applied to the replacement</param>
		/// <returns> The replaced state</returns>
		/// <remarks>
		/// If the corresponding type is a component type, then apply <see cref="ReplaceAssociationsAsync(object[],object[],NHibernate.Type.IType[],NHibernate.Engine.ISessionImplementor,object,System.Collections.IDictionary,NHibernate.Type.ForeignKeyDirection)" />
		/// across the component subtypes but do not replace the component value itself.
		/// </remarks>
		public static async Task<object[]> ReplaceAssociationsAsync(object[] original, object[] target, IType[] types,
			ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
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
					object[] origComponentValues = original[i] == null ? new object[subtypes.Length] : componentType.GetPropertyValues(original[i], session);
					object[] targetComponentValues = target[i] == null ? new object[subtypes.Length] : componentType.GetPropertyValues(target[i], session);

					object[] componentCopy = await (ReplaceAssociationsAsync(origComponentValues, targetComponentValues, subtypes, session, null, copyCache, foreignKeyDirection)).ConfigureAwait(false);
					
					if (!componentType.IsAnyType && target[i] != null)
						componentType.SetPropertyValues(target[i], componentCopy);
					
					copied[i] = target[i];
				}
				else if (!types[i].IsAssociationType)
				{
					copied[i] = target[i];
				}
				else
				{
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copyCache, foreignKeyDirection)).ConfigureAwait(false);
				}
			}
			return copied;
		}
	}
}