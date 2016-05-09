using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			var sn = (IList<T>)snapshot;
			return GetOrphans((ICollection)sn, (ICollection)WrappedList, entityName, Session);
		}

		public override async Task<object> ReadFromAsync(IDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner)
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

		public override async Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula)
		{
			IList deletes = new List<object>();
			var sn = (IList<T>)GetSnapshot();
			int end;
			if (sn.Count > WrappedList.Count)
			{
				for (int i = WrappedList.Count; i < sn.Count; i++)
				{
					deletes.Add(indexIsFormula ? (object)sn[i] : i);
				}

				end = WrappedList.Count;
			}
			else
			{
				end = sn.Count;
			}

			for (int i = 0; i < end; i++)
			{
				if (WrappedList[i] == null && sn[i] != null)
				{
					deletes.Add(indexIsFormula ? (object)sn[i] : i);
				}
			}

			return deletes;
		}

		public override async Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType)
		{
			var sn = (IList<T>)GetSnapshot();
			return WrappedList[i] != null && (i >= sn.Count || sn[i] == null);
		}

		public override async Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType)
		{
			var sn = (IList<T>)GetSnapshot();
			return i < sn.Count && sn[i] != null && WrappedList[i] != null && await (elemType.IsDirtyAsync(WrappedList[i], sn[i], Session));
		}

		public override async Task<object> GetSnapshotAsync(ICollectionPersister persister)
		{
			EntityMode entityMode = Session.EntityMode;
			var clonedList = new List<T>(WrappedList.Count);
			foreach (T current in WrappedList)
			{
				var deepCopy = (T)await (persister.ElementType.DeepCopyAsync(current, entityMode, persister.Factory));
				clonedList.Add(deepCopy);
			}

			return clonedList;
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
	}
}