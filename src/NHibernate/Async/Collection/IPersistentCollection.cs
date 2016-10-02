#if NET_4_5
using System.Collections;
using System.Data.Common;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPersistentCollection
	{
		/// <summary>
		/// Read the state of the collection from a disassembled cached value.
		/// </summary>
		/// <param name = "persister"></param>
		/// <param name = "disassembled"></param>
		/// <param name = "owner"></param>
		Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner);
		/// <summary>
		/// Reads the row from the <see cref = "DbDataReader"/>.
		/// </summary>
		/// <remarks>
		/// This method should be prepared to handle duplicate elements caused by fetching multiple collections.
		/// </remarks>
		/// <param name = "reader">The DbDataReader that contains the value of the Identifier</param>
		/// <param name = "role">The persister for this Collection.</param>
		/// <param name = "descriptor">The descriptor providing result set column names</param>
		/// <param name = "owner">The owner of this Collection.</param>
		/// <returns>The object that was contained in the row.</returns>
		Task<object> ReadFromAsync(DbDataReader reader, ICollectionPersister role, ICollectionAliases descriptor, object owner);
		/// <summary>
		/// Does the current state exactly match the snapshot?
		/// </summary>
		/// <param name = "persister">The <see cref = "ICollectionPersister"/> to compare the elements of the Collection.</param>
		/// <returns>
		/// <see langword = "true"/> if the wrapped collection is different than the snapshot
		/// of the collection or if one of the elements in the collection is
		/// dirty.
		/// </returns>
		Task<bool> EqualsSnapshotAsync(ICollectionPersister persister);
		/// <summary>
		/// Disassemble the collection, ready for the cache
		/// </summary>
		/// <param name = "persister">The <see cref = "ICollectionPersister"/> for this Collection.</param>
		/// <returns>The contents of the persistent collection in a cacheable form.</returns>
		Task<object> DisassembleAsync(ICollectionPersister persister);
		/// <summary>
		/// To be called internally by the session, forcing
		/// immediate initalization.
		/// </summary>
		/// <remarks>
		/// This method is similar to <see cref = "AbstractPersistentCollection.Initialize"/>, except that different exceptions are thrown.
		/// </remarks>
		Task ForceInitializationAsync();
		/// <summary>
		/// Do we need to insert this element?
		/// </summary>
		Task<bool> NeedsInsertingAsync(object entry, int i, IType elemType);
		/// <summary>
		/// Do we need to update this element?
		/// </summary>
		Task<bool> NeedsUpdatingAsync(object entry, int i, IType elemType);
		/// <summary>
		/// Get all the elements that need deleting
		/// </summary>
		Task<IEnumerable> GetDeletesAsync(ICollectionPersister persister, bool indexIsFormula);
		/// <summary> Get the "queued" orphans</summary>
		Task<ICollection> GetQueuedOrphansAsync(string entityName);
		/// <summary>
		/// Called before inserting rows, to ensure that any surrogate keys are fully generated
		/// </summary>
		/// <param name = "persister"></param>
		Task PreInsertAsync(ICollectionPersister persister);
		/// <summary>
		/// Get all "orphaned" elements
		/// </summary>
		/// <param name = "snapshot">The snapshot of the collection.</param>
		/// <param name = "entityName">The persistent class whose objects
		/// the collection is expected to contain.</param>
		/// <returns>
		/// An <see cref = "ICollection"/> that contains all of the elements
		/// that have been orphaned.
		/// </returns>
		Task<ICollection> GetOrphansAsync(object snapshot, string entityName);
	}
}
#endif
