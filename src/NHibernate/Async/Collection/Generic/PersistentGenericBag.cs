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
	public partial class PersistentGenericBag<T> : AbstractPersistentCollection, IList<T>, IList
	{
		public override async Task<object> DisassembleAsync(ICollectionPersister persister)
		{
			var length = _gbag.Count;
			var result = new object[length];
			for (var i = 0; i < length; i++)
			{
				result[i] = await (persister.ElementType.DisassembleAsync(_gbag[i], Session, null));
			}

			return result;
		}

		public override Task<bool> EqualsSnapshotAsync(ICollectionPersister persister)
		{
			try
			{
				return Task.FromResult<bool>(EqualsSnapshot(persister));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
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

		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			var sn = (ICollection)snapshot;
			return await (GetOrphansAsync(sn, (ICollection)_gbag, entityName, Session));
		}

		/// <summary>
		/// Initializes this PersistentBag from the cached values.
		/// </summary>
		/// <param name = "persister">The CollectionPersister to use to reassemble the PersistentBag.</param>
		/// <param name = "disassembled">The disassembled PersistentBag.</param>
		/// <param name = "owner">The owner object.</param>
		public override async Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner)
		{
			var array = (object[])disassembled;
			var size = array.Length;
			BeforeInitialize(persister, size);
			for (var i = 0; i < size; i++)
			{
				var element = await (persister.ElementType.AssembleAsync(array[i], Session, owner));
				if (element != null)
				{
					_gbag.Add((T)element);
				}
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

		public override Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType)
		{
			try
			{
				return Task.FromResult<bool>(NeedsUpdating(entry, i, elemType));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public override async Task<object> ReadFromAsync(DbDataReader reader, ICollectionPersister role, ICollectionAliases descriptor, object owner)
		{
			// note that if we load this collection from a cartesian product
			// the multiplicity would be broken ... so use an idbag instead
			var element = await (role.ReadElementAsync(reader, owner, descriptor.SuffixedElementAliases, Session));
			// NH Different behavior : we don't check for null
			// The NH-750 test show how checking for null we are ignoring the not-found tag and
			// the DB may have some records ignored by NH. This issue may need some more deep consideration.
			//if (element != null)
			_gbag.Add((T)element);
			return element;
		}
	}
}
#endif
