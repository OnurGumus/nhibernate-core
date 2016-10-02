#if NET_4_5
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
using System.Threading.Tasks;

namespace NHibernate.Collection.Generic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PersistentGenericList<T> : AbstractPersistentCollection, IList<T>, IList
	{
		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			var sn = (IList<T>)snapshot;
			return await (GetOrphansAsync((ICollection)sn, (ICollection)WrappedList, entityName, Session));
		}

		public override async Task<bool> EqualsSnapshotAsync(ICollectionPersister persister)
		{
			IType elementType = persister.ElementType;
			var sn = (IList<T>)GetSnapshot();
			if (sn.Count != WrappedList.Count)
			{
				return false;
			}

			for (int i = 0; i < WrappedList.Count; i++)
			{
				if (await (elementType.IsDirtyAsync(WrappedList[i], sn[i], Session)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<object> ReadFromAsync(DbDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner)
		{
			var element = (T)await (role.ReadElementAsync(rs, owner, descriptor.SuffixedElementAliases, Session));
			int index = (int)await (role.ReadIndexAsync(rs, descriptor.SuffixedIndexAliases, Session));
			//pad with nulls from the current last element up to the new index
			for (int i = WrappedList.Count; i <= index; i++)
			{
				WrappedList.Insert(i, DefaultForType);
			}

			WrappedList[index] = element;
			return element;
		}

		/// <summary>
		/// Initializes this PersistentGenericList from the cached values.
		/// </summary>
		/// <param name = "persister">The CollectionPersister to use to reassemble the PersistentGenericList.</param>
		/// <param name = "disassembled">The disassembled PersistentList.</param>
		/// <param name = "owner">The owner object.</param>
		public override async Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner)
		{
			object[] array = (object[])disassembled;
			int size = array.Length;
			BeforeInitialize(persister, size);
			for (int i = 0; i < size; i++)
			{
				var element = await (persister.ElementType.AssembleAsync(array[i], Session, owner));
				WrappedList.Add((T)(element ?? DefaultForType));
			}
		}

		public override async Task<object> DisassembleAsync(ICollectionPersister persister)
		{
			int length = WrappedList.Count;
			object[] result = new object[length];
			for (int i = 0; i < length; i++)
			{
				result[i] = await (persister.ElementType.DisassembleAsync(WrappedList[i], Session, null));
			}

			return result;
		}

		public override Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula)
		{
			try
			{
				return Task.FromResult<IEnumerable>(GetDeletes(persister, indexIsFormula));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IEnumerable>(ex);
			}
		}

		public override Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType)
		{
			try
			{
				return Task.FromResult<bool>(NeedsInserting(entry, i, elemType));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public override async Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType)
		{
			var sn = (IList<T>)GetSnapshot();
			return i < sn.Count && sn[i] != null && WrappedList[i] != null && await (elemType.IsDirtyAsync(WrappedList[i], sn[i], Session));
		}
	}
}
#endif
