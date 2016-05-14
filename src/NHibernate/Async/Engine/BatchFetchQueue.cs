#if NET_4_5
using System.Collections;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BatchFetchQueue
	{
		/// <summary>
		/// Get a batch of uninitialized collection keys for a given role
		/// </summary>
		/// <param name = "collectionPersister">The persister for the collection role.</param>
		/// <param name = "id">A key that must be included in the batch fetch</param>
		/// <param name = "batchSize">the maximum number of keys to return</param>
		/// <returns>an array of collection keys, of length batchSize (padded with nulls)</returns>
		public async Task<object[]> GetCollectionBatchAsync(ICollectionPersister collectionPersister, object id, int batchSize)
		{
			object[] keys = new object[batchSize];
			keys[0] = id;
			int i = 1;
			int end = -1;
			bool checkForEnd = false;
			// this only works because collection entries are kept in a sequenced
			// map by persistence context (maybe we should do like entities and
			// keep a separate sequences set...)
			foreach (DictionaryEntry me in context.CollectionEntries)
			{
				CollectionEntry ce = (CollectionEntry)me.Value;
				IPersistentCollection collection = (IPersistentCollection)me.Key;
				if (!collection.WasInitialized && ce.LoadedPersister == collectionPersister)
				{
					if (checkForEnd && i == end)
					{
						return keys; //the first key found after the given key
					}

					//if ( end == -1 && count > batchSize*10 ) return keys; //try out ten batches, max
					bool isEqual = await (collectionPersister.KeyType.IsEqualAsync(id, ce.LoadedKey, context.Session.EntityMode, collectionPersister.Factory));
					if (isEqual)
					{
						end = i;
					//checkForEnd = false;
					}
					else if (!IsCached(ce.LoadedKey, collectionPersister))
					{
						keys[i++] = ce.LoadedKey;
					//count++;
					}

					if (i == batchSize)
					{
						i = 1; //end of array, start filling again from start
						if (end != -1)
						{
							checkForEnd = true;
						}
					}
				}
			}

			return keys; //we ran out of keys to try
		}

		/// <summary>
		/// Get a batch of unloaded identifiers for this class, using a slightly
		/// complex algorithm that tries to grab keys registered immediately after
		/// the given key.
		/// </summary>
		/// <param name = "persister">The persister for the entities being loaded.</param>
		/// <param name = "id">The identifier of the entity currently demanding load.</param>
		/// <param name = "batchSize">The maximum number of keys to return</param>
		/// <returns>an array of identifiers, of length batchSize (possibly padded with nulls)</returns>
		public async Task<object[]> GetEntityBatchAsync(IEntityPersister persister, object id, int batchSize)
		{
			object[] ids = new object[batchSize];
			ids[0] = id; //first element of array is reserved for the actual instance we are loading!
			int i = 1;
			int end = -1;
			bool checkForEnd = false;
			foreach (EntityKey key in batchLoadableEntityKeys.Keys)
			{
				if (key.EntityName.Equals(persister.EntityName))
				{
					//TODO: this needn't exclude subclasses...
					if (checkForEnd && i == end)
					{
						//the first id found after the given id
						return ids;
					}

					if (await (persister.IdentifierType.IsEqualAsync(id, key.Identifier, context.Session.EntityMode)))
					{
						end = i;
					}
					else
					{
						if (!IsCached(key, persister))
						{
							ids[i++] = key.Identifier;
						}
					}

					if (i == batchSize)
					{
						i = 1; //end of array, start filling again from start
						if (end != -1)
							checkForEnd = true;
					}
				}
			}

			return ids; //we ran out of ids to try
		}
	}
}
#endif
