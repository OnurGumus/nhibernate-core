using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;

namespace NHibernate.Collection
{
	/// <summary>
	/// A persistent wrapper for an array. lazy initialization is NOT supported
	/// </summary>
	/// <remarks> Use of Hibernate arrays is not really recommended. </remarks>
	[Serializable]
	[DebuggerTypeProxy(typeof (CollectionProxy))]
	public class PersistentArrayHolder : AbstractPersistentCollection, ICollection
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (PersistentArrayHolder));

		private Array array;

		[NonSerialized] private readonly System.Type elementClass;

		/// <summary>
		/// A temporary list that holds the objects while the PersistentArrayHolder is being
		/// populated from the database.
		/// </summary>
		[NonSerialized] private List<object> tempList;

		public PersistentArrayHolder(ISessionImplementor session, object array) : base(session)
		{
			this.array = (Array) array;
			SetInitialized();
		}

		public PersistentArrayHolder(ISessionImplementor session, ICollectionPersister persister) : base(session)
		{
			elementClass = persister.ElementClass;
		}

		/// <summary>
		/// Gets or sets the array.
		/// </summary>
		/// <value>The array.</value>
		public object Array
		{
			get { return array; }
			protected set
			{
				array = (Array)value;
			}
		}

		/// <summary>
		/// Returns the user-visible portion of the NHibernate PersistentArrayHolder.
		/// </summary>
		/// <returns>
		/// The array that contains the data, not the NHibernate wrapper.
		/// </returns>
		public override object GetValue()
		{
			return array;
		}

		public override object GetSnapshot(ICollectionPersister persister)
		{
			EntityMode entityMode = Session.EntityMode;

			int length = array.Length;
			Array result = System.Array.CreateInstance(persister.ElementClass, length);
			for (int i = 0; i < length; i++)
			{
				object elt = array.GetValue(i);
				try
				{
					result.SetValue(persister.ElementType.DeepCopy(elt, entityMode, persister.Factory), i);
				}
				catch (Exception e)
				{
					log.Error("Array element type error", e);
					throw new HibernateException("Array element type error", e);
				}
			}
			return result;
		}

		public override bool IsSnapshotEmpty(object snapshot)
		{
			return ((Array) snapshot).Length == 0;
		}

		public override async Task<ICollection> GetOrphans(object snapshot, string entityName)
		{
			object[] sn = (object[]) snapshot;
			object[] arr = (object[]) array;
			List<object> result = new List<object>(sn);
			for (int i = 0; i < sn.Length; i++)
			{
				await IdentityRemove(result, arr[i], entityName, Session).ConfigureAwait(false);
			}
			return result;
		}

		public override bool IsWrapper(object collection)
		{
			return array == collection;
		}

		public override async Task<bool> EqualsSnapshot(ICollectionPersister persister)
		{
			IType elementType = persister.ElementType;
			Array snapshot = (Array) GetSnapshot();

			int xlen = snapshot.Length;
			if (xlen != array.Length)
			{
				return false;
			}
			for (int i = 0; i < xlen; i++)
			{
				if (await elementType.IsDirty(snapshot.GetValue(i), array.GetValue(i), Session).ConfigureAwait(false))
				{
					return false;
				}
			}
			return true;
		}

		public ICollection Elements()
		{
			// NH Different implementation but same result
			return (ICollection) array.Clone();
		}

		public override bool Empty
		{
			get { return false; }
		}

		public override async Task<object> ReadFrom(IDataReader rs, ICollectionPersister role, ICollectionAliases descriptor, object owner)
		{
			object element = await role.ReadElement(rs, owner, descriptor.SuffixedElementAliases, Session).ConfigureAwait(false);
			int index = (int) await role.ReadIndex(rs, descriptor.SuffixedIndexAliases, Session).ConfigureAwait(false);
			for (int i = tempList.Count; i <= index; i++)
			{
				tempList.Add(null);
			}
			tempList[index] = element;
			return element;
		}

		public override IEnumerable Entries(ICollectionPersister persister)
		{
			return Elements();
		}

		/// <summary>
		/// Before <see cref="ReadFrom" /> is called the PersistentArrayHolder needs to setup 
		/// a temporary list to hold the objects.
		/// </summary>
		public override void BeginRead()
		{
			base.BeginRead();
			tempList = new List<object>();
		}

		/// <summary>
		/// Takes the contents stored in the temporary list created during <see cref="BeginRead" />
		/// that was populated during <see cref="ReadFrom" /> and write it to the underlying 
		/// array.
		/// </summary>
		public override bool EndRead(ICollectionPersister persister)
		{
			SetInitialized();
			array = System.Array.CreateInstance(elementClass, tempList.Count);
			for (int i = 0; i < tempList.Count; i++)
			{
				array.SetValue(tempList[i], i);
			}
			tempList = null;
			return true;
		}

		public override void BeforeInitialize(ICollectionPersister persister, int anticipatedSize) {}

		public override bool IsDirectlyAccessible
		{
			get { return true; }
		}

		/// <summary>
		/// Initializes this array holder from the cached values.
		/// </summary>
		/// <param name="persister">The CollectionPersister to use to reassemble the Array.</param>
		/// <param name="disassembled">The disassembled Array.</param>
		/// <param name="owner">The owner object.</param>
		public override async Task InitializeFromCache(ICollectionPersister persister, object disassembled, object owner)
		{
			object[] cached = (object[]) disassembled;

			array = System.Array.CreateInstance(persister.ElementClass, cached.Length);

			for (int i = 0; i < cached.Length; i++)
			{
				array.SetValue(await persister.ElementType.Assemble(cached[i], Session, owner).ConfigureAwait(false), i);
			}
		}

		public override async Task<object> Disassemble(ICollectionPersister persister)
		{
			int length = array.Length;
			object[] result = new object[length];
			for (int i = 0; i < length; i++)
			{
				result[i] = await persister.ElementType.Disassemble(array.GetValue(i), Session, null).ConfigureAwait(false);
			}
			return result;
		}

		public override Task<IEnumerable> GetDeletes(ICollectionPersister persister, bool indexIsFormula)
		{
			IList deletes = new List<object>();
			Array sn = (Array) GetSnapshot();
			int snSize = sn.Length;
			int arraySize = array.Length;
			int end;
			if (snSize > arraySize)
			{
				for (int i = arraySize; i < snSize; i++)
				{
					deletes.Add(i);
				}
				end = arraySize;
			}
			else
			{
				end = snSize;
			}
			for (int i = 0; i < end; i++)
			{
				if (array.GetValue(i) == null && sn.GetValue(i) != null)
				{
					deletes.Add(i);
				}
			}
			return Task.FromResult<IEnumerable>(deletes);
		}

		public override Task<bool> NeedsInserting(object entry, int i, IType elemType)
		{
			Array sn = (Array) GetSnapshot();
			return Task.FromResult(array.GetValue(i) != null && (i >= sn.Length || sn.GetValue(i) == null));
		}

		public override async Task<bool> NeedsUpdating(object entry, int i, IType elemType)
		{
			Array sn = (Array) GetSnapshot();
			return
				i < sn.Length && sn.GetValue(i) != null && array.GetValue(i) != null
				&& await elemType.IsDirty(array.GetValue(i), sn.GetValue(i), Session).ConfigureAwait(false);
		}

		public override object GetIndex(object entry, int i, ICollectionPersister persister)
		{
			return i;
		}

		public override object GetElement(object entry)
		{
			return entry;
		}

		public override object GetSnapshotElement(object entry, int i)
		{
			Array sn = (Array) GetSnapshot();
			return sn.GetValue(i);
		}

		public override bool EntryExists(object entry, int i)
		{
			return entry != null;
		}

		#region ICollection Members

		// NH Different : we implement one of the "minimal" interface the NET Array support
		void ICollection.CopyTo(Array array, int index)
		{
			this.array.CopyTo(array, index);
		}

		int ICollection.Count
		{
			get { return array.Length; }
		}

		object ICollection.SyncRoot
		{
			get { return this; }
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return array.GetEnumerator();
		}

		#endregion
	}
}