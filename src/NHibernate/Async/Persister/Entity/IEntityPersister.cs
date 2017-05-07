﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Tuple.Entity;
using NHibernate.Type;
using System.Collections;

namespace NHibernate.Persister.Entity
{
	using System.Threading.Tasks;

	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial interface IEntityPersister : IOptimisticCacheSource
	{

		#region stuff that is persister-centric and/or EntityInfo-centric

		/// <summary> 
		/// Retrieve the current state of the natural-id properties from the database. 
		/// </summary>
		/// <param name="id">
		/// The identifier of the entity for which to retrieve the natural-id values.
		/// </param>
		/// <param name="session">
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
		/// <param name="id">The id.</param>
		/// <param name="fields">The fields.</param>
		/// <param name="dirtyFields">The dirty fields.</param>
		/// <param name="hasDirtyCollection">if set to <see langword="true" /> [has dirty collection].</param>
		/// <param name="oldFields">The old fields.</param>
		/// <param name="oldVersion">The old version.</param>
		/// <param name="obj">The obj.</param>
		/// <param name="rowId">The rowId</param>
		/// <param name="session">The session.</param>
		Task UpdateAsync(
			object id,
			object[] fields,
			int[] dirtyFields,
			bool hasDirtyCollection,
			object[] oldFields,
			object oldVersion,
			object obj,
			object rowId,
			ISessionImplementor session);

		/// <summary>
		/// Get the current database state of the object, in a "hydrated" form, without resolving identifiers
		/// </summary>
		/// <param name="id"></param>
		/// <param name="session"></param>
		/// <returns><see langword="null" /> if select-before-update is not enabled or not supported</returns>
		Task<object[]> GetDatabaseSnapshotAsync(object id, ISessionImplementor session);

		/// <summary>
		/// Get the current version of the object, or return null if there is no row for
		/// the given identifier. In the case of unversioned data, return any object
		/// if the row exists.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="session"></param>
		/// <returns></returns>
		Task<object> GetCurrentVersionAsync(object id, ISessionImplementor session);

		Task<object> ForceVersionIncrementAsync(object id, object currentVersion, ISessionImplementor session);

		#endregion
		#region stuff that is tuplizer-centric, but is passed a session

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
		/// <param name="id">The entity's id value.</param>
		/// <param name="entity">The entity for which to get the state.</param>
		/// <param name="state">The entity state (at the time of Save).</param>
		/// <param name="session">The session.</param>
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
		/// <param name="id">The entity's id value.</param>
		/// <param name="entity">The entity for which to get the state.</param>
		/// <param name="state">The entity state (at the time of Save).</param>
		/// <param name="session">The session.</param>
		Task ProcessUpdateGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session);

		#endregion
		#region stuff that is Tuplizer-centric

		#endregion
	}
}
