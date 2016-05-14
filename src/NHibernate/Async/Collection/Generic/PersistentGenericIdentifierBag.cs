#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Collection.Generic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PersistentIdentifierBag<T> : AbstractPersistentCollection, IList<T>, IList
	{
		/// <summary>
		/// Initializes this Bag from the cached values.
		/// </summary>
		/// <param name = "persister">The CollectionPersister to use to reassemble the PersistentIdentifierBag.</param>
		/// <param name = "disassembled">The disassembled PersistentIdentifierBag.</param>
		/// <param name = "owner">The owner object.</param>
		public override async Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner)
		{
			object[] array = (object[])disassembled;
			int size = array.Length;
			BeforeInitialize(persister, size);
			for (int i = 0; i < size; i += 2)
			{
				_identifiers[i / 2] = await (persister.IdentifierType.AssembleAsync(array[i], Session, owner));
				_values.Add((T)await (persister.ElementType.AssembleAsync(array[i + 1], Session, owner)));
			}
		}

		public override async Task<object> DisassembleAsync(ICollectionPersister persister)
		{
			object[] result = new object[_values.Count * 2];
			int i = 0;
			for (int j = 0; j < _values.Count; j++)
			{
				object val = _values[j];
				result[i++] = await (persister.IdentifierType.DisassembleAsync(_identifiers[j], Session, null));
				result[i++] = await (persister.ElementType.DisassembleAsync(val, Session, null));
			}

			return result;
		}

		public override async Task<bool> EqualsSnapshotAsync(ICollectionPersister persister)
		{
			IType elementType = persister.ElementType;
			var snap = (ISet<SnapshotElement>)GetSnapshot();
			if (snap.Count != _values.Count)
			{
				return false;
			}

			for (int i = 0; i < _values.Count; i++)
			{
				object val = _values[i];
				object id = GetIdentifier(i);
				object old = snap.Where(x => Equals(x.Id, id)).Select(x => x.Value).FirstOrDefault();
				if (await (elementType.IsDirtyAsync(old, val, Session)))
				{
					return false;
				}
			}

			return true;
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
			if (entry == null)
			{
				return false;
			}

			var snap = (ISet<SnapshotElement>)GetSnapshot();
			object id = GetIdentifier(i);
			if (id == null)
			{
				return false;
			}

			object old = snap.Where(x => Equals(x.Id, id)).Select(x => x.Value).FirstOrDefault();
			return old != null && await (elemType.IsDirtyAsync(old, entry, Session));
		}

		public override async Task<object> ReadFromAsync(DbDataReader reader, ICollectionPersister persister, ICollectionAliases descriptor, object owner)
		{
			object element = await (persister.ReadElementAsync(reader, owner, descriptor.SuffixedElementAliases, Session));
			object id = await (persister.ReadIdentifierAsync(reader, descriptor.SuffixedIdentifierAlias, Session));
			// eliminate duplication if loaded in a cartesian product
			if (!_identifiers.ContainsValue(id))
			{
				_identifiers[_values.Count] = id;
				_values.Add((T)element);
			}

			return element;
		}

		public override async Task<object> GetSnapshotAsync(ICollectionPersister persister)
		{
			EntityMode entityMode = Session.EntityMode;
			var map = new HashSet<SnapshotElement>();
			int i = 0;
			foreach (object value in _values)
			{
				object id;
				_identifiers.TryGetValue(i++, out id);
				var valueCopy = await (persister.ElementType.DeepCopyAsync(value, entityMode, persister.Factory));
				map.Add(new SnapshotElement{Id = id, Value = valueCopy});
			}

			return map;
		}

		public override Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			try
			{
				return Task.FromResult<ICollection>(GetOrphans(snapshot, entityName));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<ICollection>(ex);
			}
		}

		public override async Task PreInsertAsync(ICollectionPersister persister)
		{
			if ((persister.IdentifierGenerator as IPostInsertIdentifierGenerator) != null)
			{
				// NH Different behavior (NH specific) : if we are using IdentityGenerator the PreInsert have no effect
				return;
			}

			try
			{
				int i = 0;
				foreach (object entry in _values)
				{
					int loc = i++;
					if (!_identifiers.ContainsKey(loc)) // TODO: native ids
					{
						object id = await (persister.IdentifierGenerator.GenerateAsync(Session, entry));
						_identifiers[loc] = id;
					}
				}
			}
			catch (Exception sqle)
			{
				throw new ADOException("Could not generate idbag row id.", sqle);
			}
		}
	}
}
#endif
