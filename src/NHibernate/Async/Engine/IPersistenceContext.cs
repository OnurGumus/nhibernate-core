using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Engine.Loading;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Engine
{
	/// <summary>
	/// Holds the state of the persistence context, including the
	/// first-level cache, entries, snapshots, proxies, etc.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPersistenceContext
	{
		/// <summary>
		/// Get the current state of the entity as known to the underlying
		/// database, or null if there is no corresponding row
		/// </summary>
		Task<object[]> GetDatabaseSnapshotAsync(object id, IEntityPersister persister);
		/// <summary>
		/// Get the values of the natural id fields as known to the underlying
		/// database, or null if the entity has no natural id or there is no
		/// corresponding row.
		/// </summary>
		Task<object[]> GetNaturalIdSnapshotAsync(object id, IEntityPersister persister);
		/// <summary>
		/// Get the entity instance underlying the given proxy, throwing
		/// an exception if the proxy is uninitialized. If the given object
		/// is not a proxy, simply return the argument.
		/// </summary>
		Task<object> UnproxyAsync(object maybeProxy);
		/// <summary>
		/// Possibly unproxy the given reference and reassociate it with the current session.
		/// </summary>
		/// <param name = "maybeProxy">The reference to be unproxied if it currently represents a proxy. </param>
		/// <returns> The unproxied instance. </returns>
		Task<object> UnproxyAndReassociateAsync(object maybeProxy);
		/// <summary> Get the ID for the entity that owned this persistent collection when it was loaded </summary>
		/// <param name = "collection">The persistent collection </param>
		/// <returns> the owner ID if available from the collection's loaded key; otherwise, returns null </returns>
		Task<object> GetLoadedCollectionOwnerIdOrNullAsync(IPersistentCollection collection);
		/// <summary> add a collection we just pulled out of the cache (does not need initializing)</summary>
		Task<CollectionEntry> AddInitializedCollectionAsync(ICollectionPersister persister, IPersistentCollection collection, object id);
		/// <summary>
		/// Force initialization of all non-lazy collections encountered during
		/// the current two-phase load (actually, this is a no-op, unless this
		/// is the "outermost" load)
		/// </summary>
		Task InitializeNonLazyCollectionsAsync();
		/// <summary>
		/// Search the persistence context for an owner for the child object,
		/// given a collection role
		/// </summary>
		Task<object> GetOwnerIdAsync(string entity, string property, object childObject, IDictionary mergeMap);
		/// <summary>
		/// Change the read-only status of an entity (or proxy).
		/// </summary>
		/// <remarks>
		/// <para>
		/// Read-only entities can be modified, but changes are not persisted. They are not dirty-checked 
		/// and snapshots of persistent state are not maintained. 
		/// </para>
		/// <para>
		/// Immutable entities cannot be made read-only.
		/// </para>
		/// <para>
		/// To set the <em>default</em> read-only setting for entities and proxies that are loaded 
		/// into the persistence context, see <see cref = "IPersistenceContext.DefaultReadOnly"/>.
		/// </para>
		/// </remarks>
		/// <param name = "entityOrProxy">An entity (or <see cref = "NHibernate.Proxy.INHibernateProxy"/>).</param>
		/// <param name = "readOnly">If <c>true</c>, the entity or proxy is made read-only; if <c>false</c>, it is made modifiable.</param>
		/// <seealso cref = "IPersistenceContext.DefaultReadOnly"/>
		/// <seealso cref = "IPersistenceContext.IsReadOnly(object)"/>
		Task SetReadOnlyAsync(object entityOrProxy, bool readOnly);
	}
}