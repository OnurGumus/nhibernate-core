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
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public static partial class TypeHelper
	{
		
		/// <summary>Apply the <see cref="ICacheAssembler.BeforeAssembleAsync(object,ISessionImplementor,CancellationToken)" /> operation across a series of values.</summary>
		/// <param name="row">The values</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		public static async Task BeforeAssembleAsync(object[] row, ICacheAssembler[] types, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			for (int i = 0; i < types.Length; i++)
			{
				if (!Equals(LazyPropertyInitializer.UnfetchedProperty, row[i]) && !Equals(BackrefPropertyAccessor.Unknown, row[i]))
				{
					await (types[i].BeforeAssembleAsync(row[i], session, cancellationToken)).ConfigureAwait(false);
				}
			}
		}
		
		/// <summary>
		/// Apply the <see cref="ICacheAssembler.AssembleAsync(object,ISessionImplementor,object,CancellationToken)" /> operation across a series of values.
		/// </summary>
		/// <param name="row">The values</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns></returns>
		public static async Task<object[]> AssembleAsync(object[] row, ICacheAssembler[] types, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var assembled = new object[row.Length];
			for (int i = 0; i < row.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, row[i]) || Equals(BackrefPropertyAccessor.Unknown, row[i]))
				{
					assembled[i] = row[i];
				}
				else
				{
					assembled[i] = await (types[i].AssembleAsync(row[i], session, owner, cancellationToken)).ConfigureAwait(false);
				}
			}
			return assembled;
		}
		
		/// <summary>
		/// Apply the <see cref="IType.ReplaceAsync(object, object, ISessionImplementor, object, IDictionary,CancellationToken)" /> operation across a series of values.
		/// </summary>
		/// <param name="original">The source of the state</param>
		/// <param name="target">The target into which to replace the source values.</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <param name="copiedAlready">Represent a cache of already replaced state</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns> The replaced state</returns>
		public static async Task<object[]> ReplaceAsync(object[] original, object[] target, IType[] types, ISessionImplementor session, 																	 object owner, IDictionary copiedAlready, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var copied = new object[original.Length];
			for (int i = 0; i < original.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, original[i]) || Equals(BackrefPropertyAccessor.Unknown, original[i]))
				{
					copied[i] = target[i];
				}
				else if (target[i] == LazyPropertyInitializer.UnfetchedProperty)
				{
					// Should be no need to check for target[i] == PropertyAccessStrategyBackRefImpl.UNKNOWN
					// because PropertyAccessStrategyBackRefImpl.get( object ) returns
					// PropertyAccessStrategyBackRefImpl.UNKNOWN, so target[i] == original[i].
					//
					// We know from above that original[i] != LazyPropertyInitializer.UNFETCHED_PROPERTY &&
					// original[i] != PropertyAccessStrategyBackRefImpl.UNKNOWN;
					// This is a case where the entity being merged has a lazy property
					// that has been initialized. Copy the initialized value from original.
					if (types[i].IsMutable)
					{
						copied[i] = types[i].DeepCopy(original[i], session.Factory);
					}
					else
					{
						copied[i] = original[i];
					}
				}
				else
				{
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copiedAlready, cancellationToken)).ConfigureAwait(false);
				}
			}
			return copied;
		}

		/// <summary>
		/// Apply the <see cref="IType.ReplaceAsync(object, object, ISessionImplementor, object, IDictionary, ForeignKeyDirection,CancellationToken)" />
		/// operation across a series of values.
		/// </summary>
		/// <param name="original">The source of the state</param>
		/// <param name="target">The target into which to replace the source values.</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <param name="copyCache">A map representing a cache of already replaced state</param>
		/// <param name="foreignKeyDirection">FK directionality to be applied to the replacement</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns> The replaced state</returns>
		public static async Task<object[]> ReplaceAsync(object[] original, object[] target, IType[] types, 			ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			object[] copied = new object[original.Length];
			for (int i = 0; i < types.Length; i++)
			{
				if (Equals(LazyPropertyInitializer.UnfetchedProperty, original[i]) || Equals(BackrefPropertyAccessor.Unknown, original[i]))
				{
					copied[i] = target[i];
				}
				else
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copyCache, foreignKeyDirection, cancellationToken)).ConfigureAwait(false);
			}
			return copied;
		}

		/// <summary>
		/// Apply the <see cref="IType.ReplaceAsync(object, object, ISessionImplementor, object, IDictionary, ForeignKeyDirection,CancellationToken)" />
		/// operation across a series of values, as long as the corresponding <see cref="IType"/> is an association.
		/// </summary>
		/// <param name="original">The source of the state</param>
		/// <param name="target">The target into which to replace the source values.</param>
		/// <param name="types">The value types</param>
		/// <param name="session">The originating session</param>
		/// <param name="owner">The entity "owning" the values</param>
		/// <param name="copyCache">A map representing a cache of already replaced state</param>
		/// <param name="foreignKeyDirection">FK directionality to be applied to the replacement</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns> The replaced state</returns>
		/// <remarks>
		/// If the corresponding type is a component type, then apply <see cref="ReplaceAssociationsAsync(object[],object[],IType[],ISessionImplementor,object,IDictionary,ForeignKeyDirection,CancellationToken)" />
		/// across the component subtypes but do not replace the component value itself.
		/// </remarks>
		public static async Task<object[]> ReplaceAssociationsAsync(object[] original, object[] target, IType[] types, 			ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
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

					object[] componentCopy = await (ReplaceAssociationsAsync(origComponentValues, targetComponentValues, subtypes, session, null, copyCache, foreignKeyDirection, cancellationToken)).ConfigureAwait(false);
					
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
					copied[i] = await (types[i].ReplaceAsync(original[i], target[i], session, owner, copyCache, foreignKeyDirection, cancellationToken)).ConfigureAwait(false);
				}
			}
			return copied;
		}
	}
}