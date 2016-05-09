using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NHibernate.DebugHelpers;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Collection.Generic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PersistentIdentifierBag<T> : AbstractPersistentCollection, IList<T>, IList
	{
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

		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			var sn = (ISet<SnapshotElement>)GetSnapshot();
			return GetOrphans(sn.Select(x => x.Value).ToArray(), (ICollection)_values, entityName, Session);
		}

		public override async Task<object> ReadFromAsync(IDataReader reader, ICollectionPersister persister, ICollectionAliases descriptor, object owner)
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

		public override async Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula)
		{
			var snap = (ISet<SnapshotElement>)GetSnapshot();
			ArrayList deletes = new ArrayList(snap.Select(x => x.Id).ToArray());
			for (int i = 0; i < _values.Count; i++)
			{
				if (_values[i] != null)
				{
					deletes.Remove(GetIdentifier(i));
				}
			}

			return deletes;
		}

		public override async Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType)
		{
			var snap = (ISet<SnapshotElement>)GetSnapshot();
			object id = GetIdentifier(i);
			object valueFound = snap.Where(x => Equals(x.Id, id)).Select(x => x.Value).FirstOrDefault();
			return entry != null && (id == null || valueFound == null);
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
	}
}