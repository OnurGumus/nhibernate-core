#if NET_4_5
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Tuple.Entity;
using NHibernate.Type;
using System.Collections;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Persister.Entity
{
	/// <summary>
	/// Concrete <c>IEntityPersister</c>s implement mapping and persistence logic for a particular class.
	/// </summary>
	/// <remarks>
	/// Implementors must be threadsafe (preferably immutable) and must provide a constructor of type
	/// matching the signature of: (PersistentClass, SessionFactoryImplementor)
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IEntityPersister : IOptimisticCacheSource
	{
		/// <summary>
		/// Finish the initialization of this object, once all <c>ClassPersisters</c> have been
		/// instantiated. Called only once, before any other method.
		/// </summary>
		Task PostInstantiateAsync();
		/// <summary> Locate the property-indices of all properties considered to be dirty. </summary>
		/// <param name = "currentState">The current state of the entity (the state to be checked). </param>
		/// <param name = "previousState">The previous state of the entity (the state to be checked against). </param>
		/// <param name = "entity">The entity for which we are checking state dirtiness. </param>
		/// <param name = "session">The session in which the check is occurring. </param>
		/// <returns> <see langword = "null"/> or the indices of the dirty properties </returns>
		Task<int[]> FindDirtyAsync(object[] currentState, object[] previousState, object entity, ISessionImplementor session);
		/// <summary> Locate the property-indices of all properties considered to be dirty. </summary>
		/// <param name = "old">The old state of the entity.</param>
		/// <param name = "current">The current state of the entity. </param>
		/// <param name = "entity">The entity for which we are checking state modification. </param>
		/// <param name = "session">The session in which the check is occurring. </param>
		/// <returns>return <see langword = "null"/> or the indicies of the modified properties</returns>
		Task<int[]> FindModifiedAsync(object[] old, object[] current, object entity, ISessionImplementor session);
		/// <summary> 
		/// Retrieve the current state of the natural-id properties from the database. 
		/// </summary>
		/// <param name = "id">
		/// The identifier of the entity for which to retrieve the natural-id values.
		/// </param>
		/// <param name = "session">
		/// The session from which the request originated.
		/// </param>
		/// <returns> The natural-id snapshot. </returns>
		Task<object[]> GetNaturalIdentifierSnapshotAsync(object id, ISessionImplementor session);
		/// <summary>
		/// Load an instance of the persistent class.
		/// </summary>
		Task<object> LoadAsync(object id, object optionalObject, LockMode lockMode, ISessionImplementor session);
		/// <summary>
		/// Do a version check (optional operation)
		/// </summary>
		Task LockAsync(object id, object version, object obj, LockMode lockMode, ISessionImplementor session);
		/// <summary>
		/// Persist an instance
		/// </summary>
		Task InsertAsync(object id, object[] fields, object obj, ISessionImplementor session);
		/// <summary>
		/// Persist an instance, using a natively generated identifier (optional operation)
		/// </summary>
		Task<object> InsertAsync(object[] fields, object obj, ISessionImplementor session);
		/// <summary>
		/// Delete a persistent instance
		/// </summary>
		Task DeleteAsync(object id, object version, object obj, ISessionImplementor session);
		/// <summary>
		/// Update a persistent instance
		/// </summary>
		/// <param name = "id">The id.</param>
		/// <param name = "fields">The fields.</param>
		/// <param name = "dirtyFields">The dirty fields.</param>
		/// <param name = "hasDirtyCollection">if set to <see langword = "true"/> [has dirty collection].</param>
		/// <param name = "oldFields">The old fields.</param>
		/// <param name = "oldVersion">The old version.</param>
		/// <param name = "obj">The obj.</param>
		/// <param name = "rowId">The rowId</param>
		/// <param name = "session">The session.</param>
		Task UpdateAsync(object id, object[] fields, int[] dirtyFields, bool hasDirtyCollection, object[] oldFields, object oldVersion, object obj, object rowId, ISessionImplementor session);
		/// <summary>
		/// Get the current database state of the object, in a "hydrated" form, without resolving identifiers
		/// </summary>
		/// <param name = "id"></param>
		/// <param name = "session"></param>
		/// <returns><see langword = "null"/> if select-before-update is not enabled or not supported</returns>
		Task<object[]> GetDatabaseSnapshotAsync(object id, ISessionImplementor session);
		/// <summary>
		/// Get the current version of the object, or return null if there is no row for
		/// the given identifier. In the case of unversioned data, return any object
		/// if the row exists.
		/// </summary>
		/// <param name = "id"></param>
		/// <param name = "session"></param>
		/// <returns></returns>
		Task<object> GetCurrentVersionAsync(object id, ISessionImplementor session);
		Task<object> ForceVersionIncrementAsync(object id, object currentVersion, ISessionImplementor session);
		/// <summary> Is this a new transient instance?</summary>
		Task<bool ? > IsTransientAsync(object obj, ISessionImplementor session);
		/// <summary> Return the values of the insertable properties of the object (including backrefs)</summary>
		Task<object[]> GetPropertyValuesToInsertAsync(object obj, IDictionary mergeMap, ISessionImplementor session);
		/// <summary>
		/// Perform a select to retrieve the values of any generated properties
		/// back from the database, injecting these generated values into the
		/// given entity as well as writing this state to the persistence context.
		/// </summary>
		/// <remarks>
		/// Note, that because we update the persistence context here, callers
		/// need to take care that they have already written the initial snapshot
		/// to the persistence context before calling this method. 
		/// </remarks>
		/// <param name = "id">The entity's id value.</param>
		/// <param name = "entity">The entity for which to get the state.</param>
		/// <param name = "state">The entity state (at the time of Save).</param>
		/// <param name = "session">The session.</param>
		Task ProcessInsertGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session);
		/// <summary>
		/// Perform a select to retrieve the values of any generated properties
		/// back from the database, injecting these generated values into the
		/// given entity as well as writing this state to the persistence context.
		/// </summary>
		/// <remarks>
		/// Note, that because we update the persistence context here, callers
		/// need to take care that they have already written the initial snapshot
		/// to the persistence context before calling this method. 
		/// </remarks>
		/// <param name = "id">The entity's id value.</param>
		/// <param name = "entity">The entity for which to get the state.</param>
		/// <param name = "state">The entity state (at the time of Save).</param>
		/// <param name = "session">The session.</param>
		Task ProcessUpdateGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session);
		/// <summary>
		/// Get the identifier of an instance ( throw an exception if no identifier property)
		/// </summary>
		Task<object> GetIdentifierAsync(object obj, EntityMode entityMode);
		/// <summary>
		/// Set the identifier of an instance (or do nothing if no identifier property)
		/// </summary>
		/// <param name = "obj">The object to set the Id property on.</param>
		/// <param name = "id">The value to set the Id property to.</param>
		/// <param name = "entityMode">The EntityMode</param>
		Task SetIdentifierAsync(object obj, object id, EntityMode entityMode);
		/// <summary>
		/// Create a class instance initialized with the given identifier
		/// </summary>
		Task<object> InstantiateAsync(object id, EntityMode entityMode);
		/// <summary> 
		/// Set the identifier and version of the given instance back
		/// to its "unsaved" value, returning the id
		/// </summary>
		Task ResetIdentifierAsync(object entity, object currentId, object currentVersion, EntityMode entityMode);
	}
}
#endif
