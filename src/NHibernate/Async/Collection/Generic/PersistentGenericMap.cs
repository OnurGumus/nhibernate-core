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
	public partial class PersistentGenericMap<TKey, TValue> : AbstractPersistentCollection, IDictionary<TKey, TValue>, ICollection
	{
		public override async Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner)
		{
			object[] array = (object[])disassembled;
			int size = array.Length;
			BeforeInitialize(persister, size);
			for (int i = 0; i < size; i += 2)
			{
				WrappedMap[(TKey)await (persister.IndexType.AssembleAsync(array[i], Session, owner))] = (TValue)await (persister.ElementType.AssembleAsync(array[i + 1], Session, owner));
			}
		}

		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			var sn = (IDictionary<TKey, TValue>)snapshot;
			return GetOrphans((ICollection)sn.Values, (ICollection)WrappedMap.Values, entityName, Session);
		}

		public override async Task<object> ReadFromAsync(IDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner)
		{
			object element = await (role.ReadElementAsync(rs, owner, descriptor.SuffixedElementAliases, Session));
			object index = await (role.ReadIndexAsync(rs, descriptor.SuffixedIndexAliases, Session));
			AddDuringInitialize(index, element);
			return element;
		}

		public override async Task<bool> EqualsSnapshotAsync(ICollectionPersister persister)
		{
			IType elementType = persister.ElementType;
			var xmap = (IDictionary<TKey, TValue>)GetSnapshot();
			if (xmap.Count != WrappedMap.Count)
			{
				return false;
			}

			foreach (KeyValuePair<TKey, TValue> entry in WrappedMap)
			{
				if (await (elementType.IsDirtyAsync(entry.Value, xmap[entry.Key], Session)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula)
		{
			IList deletes = new List<object>();
			var sn = (IDictionary<TKey, TValue>)GetSnapshot();
			foreach (var e in sn)
			{
				if (!WrappedMap.ContainsKey(e.Key))
				{
					object key = e.Key;
					deletes.Add(indexIsFormula ? e.Value : key);
				}
			}

			return deletes;
		}

		public override async Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType)
		{
			var sn = (IDictionary)GetSnapshot();
			var e = (KeyValuePair<TKey, TValue>)entry;
			return !sn.Contains(e.Key);
		}

		public override async Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType)
		{
			var sn = (IDictionary)GetSnapshot();
			var e = (KeyValuePair<TKey, TValue>)entry;
			var snValue = sn[e.Key];
			var isNew = !sn.Contains(e.Key);
			return e.Value != null && snValue != null && await (elemType.IsDirtyAsync(snValue, e.Value, Session)) || (!isNew && ((e.Value == null) != (snValue == null)));
		}

		public override async Task<object> GetSnapshotAsync(ICollectionPersister persister)
		{
			EntityMode entityMode = Session.EntityMode;
			Dictionary<TKey, TValue> clonedMap = new Dictionary<TKey, TValue>(WrappedMap.Count);
			foreach (KeyValuePair<TKey, TValue> e in WrappedMap)
			{
				object copy = await (persister.ElementType.DeepCopyAsync(e.Value, entityMode, persister.Factory));
				clonedMap[e.Key] = (TValue)copy;
			}

			return clonedMap;
		}

		public override async Task<object> DisassembleAsync(ICollectionPersister persister)
		{
			object[] result = new object[WrappedMap.Count * 2];
			int i = 0;
			foreach (KeyValuePair<TKey, TValue> e in WrappedMap)
			{
				result[i++] = await (persister.IndexType.DisassembleAsync(e.Key, Session, null));
				result[i++] = await (persister.ElementType.DisassembleAsync(e.Value, Session, null));
			}

			return result;
		}
	}
}