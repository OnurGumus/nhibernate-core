using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NHibernate.Collection.Generic.SetHelpers;
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
	public partial class PersistentGenericSet<T> : AbstractPersistentCollection, ISet<T>
	{
		public override async Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner)
		{
			var array = (object[])disassembled;
			int size = array.Length;
			BeforeInitialize(persister, size);
			for (int i = 0; i < size; i++)
			{
				var element = await (persister.ElementType.AssembleAsync(array[i], Session, owner));
				if (element != null)
				{
					WrappedSet.Add((T)element);
				}
			}

			SetInitialized();
		}

		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			var sn = new SetSnapShot<T>((IEnumerable<T>)snapshot);
			// TODO: Avoid duplicating shortcuts and array copy, by making base class GetOrphans() more flexible
			if (WrappedSet.Count == 0)
				return sn;
			if (((ICollection)sn).Count == 0)
				return sn;
			return GetOrphans(sn, WrappedSet.ToArray(), entityName, Session);
		}

		public override async Task<object> ReadFromAsync(IDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner)
		{
			var element = await (role.ReadElementAsync(rs, owner, descriptor.SuffixedElementAliases, Session));
			if (element != null)
			{
				_tempList.Add((T)element);
			}

			return element;
		}

		public override async Task<bool> EqualsSnapshotAsync(ICollectionPersister persister)
		{
			var elementType = persister.ElementType;
			var snapshot = (ISetSnapshot<T>)GetSnapshot();
			if (((ICollection)snapshot).Count != WrappedSet.Count)
			{
				return false;
			}

			foreach (T obj in WrappedSet)
			{
				T oldValue;
				if (!snapshot.TryGetValue(obj, out oldValue) || await (elementType.IsDirtyAsync(oldValue, obj, Session)))
					return false;
			}

			return true;
		}

		public override async Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula)
		{
			IType elementType = persister.ElementType;
			var sn = (ISetSnapshot<T>)GetSnapshot();
			var deletes = new List<T>(((ICollection<T>)sn).Count);
			deletes.AddRange(sn.Where(obj => !WrappedSet.Contains(obj)));
			foreach (var obj in WrappedSet)
			{
				T oldValue;
				if (sn.TryGetValue(obj, out oldValue) && await (elementType.IsDirtyAsync(obj, oldValue, Session)))
					deletes.Add(oldValue);
			}

			return deletes;
		}

		public override async Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType)
		{
			var sn = (ISetSnapshot<T>)GetSnapshot();
			T oldKey;
			// note that it might be better to iterate the snapshot but this is safe,
			// assuming the user implements equals() properly, as required by the PersistentSet
			// contract!
			return !sn.TryGetValue((T)entry, out oldKey) || await (elemType.IsDirtyAsync(oldKey, entry, Session));
		}

		public override async Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType)
		{
			return false;
		}

		public override async Task<object> GetSnapshotAsync(ICollectionPersister persister)
		{
			var entityMode = Session.EntityMode;
			var clonedSet = new SetSnapShot<T>(WrappedSet.Count);
			var enumerable =
				from object current in WrappedSet
				select await (persister.ElementType.DeepCopyAsync(current, entityMode, persister.Factory));
			foreach (var copied in enumerable)
			{
				clonedSet.Add((T)copied);
			}

			return clonedSet;
		}

		public override async Task<object> DisassembleAsync(ICollectionPersister persister)
		{
			var result = new object[WrappedSet.Count];
			int i = 0;
			foreach (object obj in WrappedSet)
			{
				result[i++] = await (persister.ElementType.DisassembleAsync(obj, Session, null));
			}

			return result;
		}
	}
}