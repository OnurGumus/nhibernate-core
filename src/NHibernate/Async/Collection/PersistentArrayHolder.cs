#if NET_4_5
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
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PersistentArrayHolder : AbstractPersistentCollection, ICollection
	{
		public override async Task<object> GetSnapshotAsync(ICollectionPersister persister)
		{
			EntityMode entityMode = Session.EntityMode;
			int length = array.Length;
			Array result = System.Array.CreateInstance(persister.ElementClass, length);
			for (int i = 0; i < length; i++)
			{
				object elt = array.GetValue(i);
				try
				{
					result.SetValue(await (persister.ElementType.DeepCopyAsync(elt, entityMode, persister.Factory)), i);
				}
				catch (Exception e)
				{
					log.Error("Array element type error", e);
					throw new HibernateException("Array element type error", e);
				}
			}

			return result;
		}

		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			object[] sn = (object[])snapshot;
			object[] arr = (object[])array;
			List<object> result = new List<object>(sn);
			for (int i = 0; i < sn.Length; i++)
			{
				await (IdentityRemoveAsync(result, arr[i], entityName, Session));
			}

			return result;
		}

		public override async Task<bool> EqualsSnapshotAsync(ICollectionPersister persister)
		{
			IType elementType = persister.ElementType;
			Array snapshot = (Array)GetSnapshot();
			int xlen = snapshot.Length;
			if (xlen != array.Length)
			{
				return false;
			}

			for (int i = 0; i < xlen; i++)
			{
				if (await (elementType.IsDirtyAsync(snapshot.GetValue(i), array.GetValue(i), Session)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<object> ReadFromAsync(IDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner)
		{
			object element = await (role.ReadElementAsync(rs, owner, descriptor.SuffixedElementAliases, Session));
			int index = (int)await (role.ReadIndexAsync(rs, descriptor.SuffixedIndexAliases, Session));
			for (int i = tempList.Count; i <= index; i++)
			{
				tempList.Add(null);
			}

			tempList[index] = element;
			return element;
		}

		/// <summary>
		/// Initializes this array holder from the cached values.
		/// </summary>
		/// <param name = "persister">The CollectionPersister to use to reassemble the Array.</param>
		/// <param name = "disassembled">The disassembled Array.</param>
		/// <param name = "owner">The owner object.</param>
		public override async Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner)
		{
			object[] cached = (object[])disassembled;
			array = System.Array.CreateInstance(persister.ElementClass, cached.Length);
			for (int i = 0; i < cached.Length; i++)
			{
				array.SetValue(await (persister.ElementType.AssembleAsync(cached[i], Session, owner)), i);
			}
		}

		public override async Task<object> DisassembleAsync(ICollectionPersister persister)
		{
			int length = array.Length;
			object[] result = new object[length];
			for (int i = 0; i < length; i++)
			{
				result[i] = await (persister.ElementType.DisassembleAsync(array.GetValue(i), Session, null));
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
			Array sn = (Array)GetSnapshot();
			return i < sn.Length && sn.GetValue(i) != null && array.GetValue(i) != null && await (elemType.IsDirtyAsync(array.GetValue(i), sn.GetValue(i), Session));
		}
	}
}
#endif
