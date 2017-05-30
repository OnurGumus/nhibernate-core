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
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Collection.Generic
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class PersistentGenericMap<TKey, TValue> : AbstractPersistentCollection, IDictionary<TKey, TValue>, ICollection
	{

		public override Task<ICollection> GetOrphansAsync(object snapshot, string entityName, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<ICollection>(cancellationToken);
			}
			try
			{
				var sn = (IDictionary<TKey, TValue>) snapshot;
				return GetOrphansAsync((ICollection)sn.Values, (ICollection)WrappedMap.Values, entityName, Session, cancellationToken);
			}
			catch (Exception ex)
			{
				return Task.FromException<ICollection>(ex);
			}
		}

		public override async Task<object> ReadFromAsync(DbDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			object element = await (role.ReadElementAsync(rs, owner, descriptor.SuffixedElementAliases, Session, cancellationToken)).ConfigureAwait(false);
			object index = await (role.ReadIndexAsync(rs, descriptor.SuffixedIndexAliases, Session, cancellationToken)).ConfigureAwait(false);

			AddDuringInitialize(index, element);
			return element;
		}

		/// <summary>
		/// Initializes this PersistentGenericMap from the cached values.
		/// </summary>
		/// <param name="persister">The CollectionPersister to use to reassemble the PersistentGenericMap.</param>
		/// <param name="disassembled">The disassembled PersistentGenericMap.</param>
		/// <param name="owner">The owner object.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		public override async Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			object[] array = (object[])disassembled;
			int size = array.Length;
			BeforeInitialize(persister, size);
			for (int i = 0; i < size; i += 2)
			{
				WrappedMap[(TKey)await (persister.IndexType.AssembleAsync(array[i], Session, owner, cancellationToken)).ConfigureAwait(false)] =
					(TValue)await (persister.ElementType.AssembleAsync(array[i + 1], Session, owner, cancellationToken)).ConfigureAwait(false);
			}
		}

		#region IDictionary<TKey,TValue> Members

		#endregion
		#region ICollection<KeyValuePair<TKey,TValue>> Members

		#endregion
		#region ICollection Members

		#endregion
		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		#endregion
		#region IEnumerable Members

		#endregion
		#region DelayedOperations

		#endregion
	}
}