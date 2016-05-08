using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Tuple.Entity;
using NHibernate.Type;
using System.Collections;
using System.Threading.Tasks;

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
		Task InsertAsync(object id, object[] fields, object obj, ISessionImplementor session);
		Task<
		/// <summary>
		/// Persist an instance, using a natively generated identifier (optional operation)
		/// </summary>
		object> InsertAsync(object[] fields, object obj, ISessionImplementor session);
		Task<
		/// <summary>
		/// Get the current version of the object, or return null if there is no row for
		/// the given identifier. In the case of unversioned data, return any object
		/// if the row exists.
		/// </summary>
		/// <param name = "id"></param>
		/// <param name = "session"></param>
		/// <returns></returns>
		object> GetCurrentVersionAsync(object id, ISessionImplementor session);
		Task ProcessUpdateGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session);
		Task ProcessInsertGeneratedPropertiesAsync(object id, object entity, object[] state, ISessionImplementor session);
		Task<
		/// <summary>
		/// Get the current database state of the object, in a "hydrated" form, without resolving identifiers
		/// </summary>
		/// <param name = "id"></param>
		/// <param name = "session"></param>
		/// <returns><see langword = "null"/> if select-before-update is not enabled or not supported</returns>
		object[]> GetDatabaseSnapshotAsync(object id, ISessionImplementor session);
		Task<
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
		object[]> GetNaturalIdentifierSnapshotAsync(object id, ISessionImplementor session);
		Task LockAsync(object id, object version, object obj, LockMode lockMode, ISessionImplementor session);
	}
}