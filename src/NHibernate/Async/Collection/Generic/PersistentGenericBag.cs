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
	public partial class PersistentGenericBag<T> : AbstractPersistentCollection, IList<T>, IList
	{
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

		public override async Task<ICollection> GetOrphansAsync(object snapshot, string entityName)
		{
			var sn = (ICollection)snapshot;
			return GetOrphans(sn, (ICollection)_gbag, entityName, Session);
		}

		public override async Task<object> ReadFromAsync(IDataReader reader, ICollectionPersister role, ICollectionAliases descriptor, object owner)
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

		private static async Task<int> CountOccurrencesAsync(object element, IEnumerable list, IType elementType, EntityMode entityMode)
		{
			var result = 0;
			foreach (var obj in list)
			{
				if (await (elementType.IsSameAsync(element, obj, entityMode)))
				{
					result++;
				}
			}

			return result;
		}

		public override async Task<bool> EqualsSnapshotAsync(ICollectionPersister persister)
		{
			var elementType = persister.ElementType;
			var entityMode = Session.EntityMode;
			var sn = (IList)GetSnapshot();
			if (sn.Count != _gbag.Count)
			{
				return false;
			}

			foreach (var elt in _gbag)
			{
				if (await (CountOccurrencesAsync(elt, _gbag, elementType, entityMode)) != await (CountOccurrencesAsync(elt, sn, elementType, entityMode)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula)
		{
			var elementType = persister.ElementType;
			var entityMode = Session.EntityMode;
			var deletes = new List<object>();
			var sn = (IList)GetSnapshot();
			var i = 0;
			foreach (var old in sn)
			{
				var found = false;
				if (_gbag.Count > i && await (elementType.IsSameAsync(old, _gbag[i++], entityMode)))
				{
					//a shortcut if its location didn't change!
					found = true;
				}
				else
				{
					foreach (object newObject in _gbag)
					{
						if (await (elementType.IsSameAsync(old, newObject, entityMode)))
						{
							found = true;
							break;
						}
					}
				}

				if (!found)
				{
					deletes.Add(old);
				}
			}

			return deletes;
		}

		public override async Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType)
		{
			var sn = (IList)GetSnapshot();
			var entityMode = Session.EntityMode;
			if (sn.Count > i && await (elemType.IsSameAsync(sn[i], entry, entityMode)))
			{
				// a shortcut if its location didn't change
				return false;
			}

			//search for it
			foreach (var old in sn)
			{
				if (await (elemType.IsEqualAsync(old, entry, entityMode)))
				{
					return false;
				}
			}

			return true;
		}

		public override async Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType)
		{
			return false;
		}

		public override async Task<object> GetSnapshotAsync(ICollectionPersister persister)
		{
			var entityMode = Session.EntityMode;
			var clonedList = new List<object>(_gbag.Count);
			foreach (object current in _gbag)
			{
				clonedList.Add(await (persister.ElementType.DeepCopyAsync(current, entityMode, persister.Factory)));
			}

			return clonedList;
		}

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
	}
}