#if NET_4_5
using System;
using System.Collections;
using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionEntry
	{
		/// <summary> 
		/// Determine if the collection is "really" dirty, by checking dirtiness
		/// of the collection elements, if necessary
		/// </summary>
		private async Task DirtyAsync(IPersistentCollection collection)
		{
			// if the collection is initialized and it was previously persistent
			// initialize the dirty flag
			bool forceDirty = collection.WasInitialized && !collection.IsDirty && LoadedPersister != null && LoadedPersister.IsMutable && (collection.IsDirectlyAccessible || LoadedPersister.ElementType.IsMutable) && !await (collection.EqualsSnapshotAsync(LoadedPersister));
			if (forceDirty)
			{
				collection.Dirty();
			}
		}

		/// <summary>
		/// Prepares this CollectionEntry for the Flush process.
		/// </summary>
		/// <param name = "collection">The <see cref = "IPersistentCollection"/> that this CollectionEntry will be responsible for flushing.</param>
		public async Task PreFlushAsync(IPersistentCollection collection)
		{
			bool nonMutableChange = collection.IsDirty && LoadedPersister != null && !LoadedPersister.IsMutable;
			if (nonMutableChange)
			{
				throw new HibernateException("changed an immutable collection instance: " + MessageHelper.InfoString(LoadedPersister.Role, LoadedKey));
			}

			await (DirtyAsync(collection));
			if (log.IsDebugEnabled && collection.IsDirty && loadedPersister != null)
			{
				log.Debug("Collection dirty: " + MessageHelper.CollectionInfoString(loadedPersister, loadedKey));
			}

			// reset all of these values so any previous flush status 
			// information is cleared from this CollectionEntry
			doupdate = false;
			doremove = false;
			dorecreate = false;
			reached = false;
			processed = false;
		}

		public async Task<ICollection> GetOrphansAsync(string entityName, IPersistentCollection collection)
		{
			if (snapshot == null)
			{
				throw new AssertionFailure("no collection snapshot for orphan delete");
			}

			return await (collection.GetOrphansAsync(snapshot, entityName));
		}
	}
}
#endif
