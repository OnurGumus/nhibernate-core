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
		public async Task<ICollection> GetOrphansAsync(string entityName, IPersistentCollection collection)
		{
			if (snapshot == null)
			{
				throw new AssertionFailure("no collection snapshot for orphan delete");
			}

			return await (collection.GetOrphansAsync(snapshot, entityName));
		}

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

		public async Task AfterActionAsync(IPersistentCollection collection)
		{
			loadedKey = CurrentKey;
			SetLoadedPersister(CurrentPersister);
			bool resnapshot = collection.WasInitialized && (IsDoremove || IsDorecreate || IsDoupdate);
			if (resnapshot)
			{
				//re-snapshot
				snapshot = loadedPersister == null || !loadedPersister.IsMutable ? null : await (collection.GetSnapshotAsync(loadedPersister));
			}

			collection.PostAction();
		}

		public async Task PostInitializeAsync(IPersistentCollection collection)
		{
			snapshot = LoadedPersister.IsMutable ? await (collection.GetSnapshotAsync(LoadedPersister)) : null;
			collection.SetSnapshot(loadedKey, role, snapshot);
		}
	}
}