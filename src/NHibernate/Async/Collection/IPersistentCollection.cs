using System.Collections;
using System.Data;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Collection
{
	/// <summary>
	/// <para>
	/// Persistent collections are treated as value objects by NHibernate.
	/// ie. they have no independent existence beyond the object holding
	/// a reference to them. Unlike instances of entity classes, they are
	/// automatically deleted when unreferenced and automatically become
	/// persistent when held by a persistent object. Collections can be
	/// passed between different objects (change "roles") and this might
	/// cause their elements to move from one database table to another.
	/// </para>
	/// <para>
	/// NHibernate "wraps" a collection in an instance of
	/// <see cref = "IPersistentCollection"/>. This mechanism is designed
	/// to support tracking of changes to the collection's persistent
	/// state and lazy instantiation of collection elements. The downside
	/// is that only certain abstract collection types are supported and
	/// any extra semantics are lost.
	/// </para>
	/// <para>
	/// Applications should <b>never</b> use classes in this namespace
	/// directly, unless extending the "framework" here.
	/// </para>
	/// <para>
	/// Changes to <b>structure</b> of the collection are recorded by the
	/// collection calling back to the session. Changes to mutable
	/// elements (ie. composite elements) are discovered by cloning their
	/// state when the collection is initialized and comparing at flush
	/// time.
	/// </para>
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPersistentCollection
	{
		/// <summary>
		/// Associate the collection with the given session.
		/// </summary>
		/// <param name = "session"></param>
		/// <returns>false if the collection was already associated with the session</returns>
		Task<bool> SetCurrentSessionAsync(ISessionImplementor session);
		/// <summary>
		/// Read the state of the collection from a disassembled cached value.
		/// </summary>
		/// <param name = "persister"></param>
		/// <param name = "disassembled"></param>
		/// <param name = "owner"></param>
		Task InitializeFromCacheAsync(ICollectionPersister persister, object disassembled, object owner);
		/// <summary>
		/// Reads the row from the <see cref = "IDataReader"/>.
		/// </summary>
		/// <remarks>
		/// This method should be prepared to handle duplicate elements caused by fetching multiple collections.
		/// </remarks>
		/// <param name = "reader">The IDataReader that contains the value of the Identifier</param>
		/// <param name = "role">The persister for this Collection.</param>
		/// <param name = "descriptor">The descriptor providing result set column names</param>
		/// <param name = "owner">The owner of this Collection.</param>
		/// <returns>The object that was contained in the row.</returns>
		Task<object> ReadFromAsync(IDataReader reader, ICollectionPersister role, ICollectionAliases descriptor, object owner);
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
		/// Return a new snapshot of the current state of the collection
		/// </summary>
		Task<object> GetSnapshotAsync(ICollectionPersister persister);
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