#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// The main runtime interface between a .NET application and NHibernate. This is the central
	/// API class abstracting the notion of a persistence service.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The lifecycle of a <c>ISession</c> is bounded by the beginning and end of a logical
	/// transaction. (Long transactions might span several database transactions.)
	/// </para>
	/// <para>
	/// The main function of the <c>ISession</c> is to offer create, find, update, and delete operations
	/// for instances of mapped entity classes. Instances may exist in one of two states:
	/// <list type = "bullet">
	/// <item>transient: not associated with any <c>ISession</c></item>
	/// <item>persistent: associated with a <c>ISession</c></item>
	/// </list>
	/// </para>
	/// <para>
	/// Transient instances may be made persistent by calling <c>Save()</c>, <c>Insert()</c>,
	/// or <c>Update()</c>. Persistent instances may be made transient by calling <c>Delete()</c>.
	/// Any instance returned by a <c>List()</c>, <c>Enumerable()</c>, <c>Load()</c>, or <c>Create()</c>
	/// method is persistent.
	/// </para>
	/// <para>
	/// <c>Save()</c> results in an SQL <c>INSERT</c>, <c>Delete()</c>
	/// in an SQL <c>DELETE</c> and <c>Update()</c> in an SQL <c>UPDATE</c>. Changes to
	/// <em>persistent</em> instances are detected at flush time and also result in an SQL
	/// <c>UPDATE</c>.
	/// </para>
	/// <para>
	/// It is not intended that implementors be threadsafe. Instead each thread/transaction should obtain
	/// its own instance from an <c>ISessionFactory</c>.
	/// </para>
	/// <para>
	/// A <c>ISession</c> instance is serializable if its persistent classes are serializable
	/// </para>
	/// <para>
	/// A typical transaction should use the following idiom:
	/// <code>
	///		using (ISession session = factory.OpenSession())
	///		using (ITransaction tx = session.BeginTransaction())
	///		{
	///			try
	///			{
	///				// do some work
	///				...
	///				tx.Commit();
	///			}
	///			catch (Exception e)
	///			{
	///				if (tx != null) tx.Rollback();
	///				throw;
	///			}
	///		}
	/// </code>
	/// </para>
	/// <para>
	/// If the <c>ISession</c> throws an exception, the transaction must be rolled back and the session
	/// discarded. The internal state of the <c>ISession</c> might not be consistent with the database
	/// after the exception occurs.
	/// </para>
	/// <seealso cref = "ISessionFactory"/>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ISession : IDisposable
	{
		/// <summary>
		/// Force the <c>ISession</c> to flush.
		/// </summary>
		/// <remarks>
		/// Must be called at the end of a unit of work, before committing the transaction and closing
		/// the session (<c>Transaction.Commit()</c> calls this method). <i>Flushing</i> is the process
		/// of synchronizing the underlying persistent store with persistable state held in memory.
		/// </remarks>
		Task FlushAsync();
		/// <summary>
		/// Does this <c>ISession</c> contain any changes which must be
		/// synchronized with the database? Would any SQL be executed if
		/// we flushed this session?
		/// </summary>
		Task<bool> IsDirtyAsync();
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
		/// into the session, see <see cref = "ISession.DefaultReadOnly"/>.
		/// </para>
		/// <para>
		/// This method a facade for <see cref = "IPersistenceContext.SetReadOnly(object, bool)"/>.
		/// </para>
		/// </remarks>
		/// <param name = "entityOrProxy">An entity (or <see cref = "NHibernate.Proxy.INHibernateProxy"/>).</param>
		/// <param name = "readOnly">If <c>true</c>, the entity or proxy is made read-only; if <c>false</c>, it is made modifiable.</param>
		/// <seealso cref = "ISession.DefaultReadOnly"/>
		/// <seealso cref = "ISession.IsReadOnly(object)"/>
		Task SetReadOnlyAsync(object entityOrProxy, bool readOnly);
		/// <summary>
		/// Is this instance associated with this Session?
		/// </summary>
		/// <param name = "obj">an instance of a persistent class</param>
		/// <returns>true if the given instance is associated with this Session</returns>
		Task<bool> ContainsAsync(object obj);
		/// <summary>
		/// Remove this instance from the session cache.
		/// </summary>
		/// <remarks>
		/// Changes to the instance will not be synchronized with the database.
		/// This operation cascades to associated instances if the association is mapped
		/// with <c>cascade="all"</c> or <c>cascade="all-delete-orphan"</c>.
		/// </remarks>
		/// <param name = "obj">a persistent instance</param>
		Task EvictAsync(Object obj);
		/// <summary>
		/// Return the persistent instance of the given entity class with the given identifier,
		/// obtaining the specified lock mode.
		/// </summary>
		/// <param name = "theType">A persistent class</param>
		/// <param name = "id">A valid identifier of an existing persistent instance of the class</param>
		/// <param name = "lockMode">The lock level</param>
		/// <returns>the persistent instance</returns>
		Task<object> LoadAsync(System.Type theType, object id, LockMode lockMode);
		/// <summary>
		/// Return the persistent instance of the given entity class with the given identifier,
		/// obtaining the specified lock mode, assuming the instance exists.
		/// </summary>
		/// <param name = "entityName">The entity-name of a persistent class</param>
		/// <param name = "id">a valid identifier of an existing persistent instance of the class </param>
		/// <param name = "lockMode">the lock level </param>
		/// <returns> the persistent instance or proxy </returns>
		Task<object> LoadAsync(string entityName, object id, LockMode lockMode);
		/// <summary>
		/// Return the persistent instance of the given entity class with the given identifier,
		/// assuming that the instance exists.
		/// </summary>
		/// <remarks>
		/// You should not use this method to determine if an instance exists (use a query or
		/// <see cref = "Get(System.Type, object)"/> instead). Use this only to retrieve an instance
		/// that you assume exists, where non-existence would be an actual error.
		/// </remarks>
		/// <param name = "theType">A persistent class</param>
		/// <param name = "id">A valid identifier of an existing persistent instance of the class</param>
		/// <returns>The persistent instance or proxy</returns>
		Task<object> LoadAsync(System.Type theType, object id);
		/// <summary>
		/// Return the persistent instance of the given entity class with the given identifier,
		/// obtaining the specified lock mode.
		/// </summary>
		/// <typeparam name = "T">A persistent class</typeparam>
		/// <param name = "id">A valid identifier of an existing persistent instance of the class</param>
		/// <param name = "lockMode">The lock level</param>
		/// <returns>the persistent instance</returns>
		Task<T> LoadAsync<T>(object id, LockMode lockMode);
		/// <summary>
		/// Return the persistent instance of the given entity class with the given identifier,
		/// assuming that the instance exists.
		/// </summary>
		/// <remarks>
		/// You should not use this method to determine if an instance exists (use a query or
		/// <see cref = "Get{T}(object)"/> instead). Use this only to retrieve an instance that you
		/// assume exists, where non-existence would be an actual error.
		/// </remarks>
		/// <typeparam name = "T">A persistent class</typeparam>
		/// <param name = "id">A valid identifier of an existing persistent instance of the class</param>
		/// <returns>The persistent instance or proxy</returns>
		Task<T> LoadAsync<T>(object id);
		/// <summary>
		/// Return the persistent instance of the given <paramref name = "entityName"/> with the given identifier,
		/// assuming that the instance exists.
		/// </summary>
		/// <param name = "entityName">The entity-name of a persistent class</param>
		/// <param name = "id">a valid identifier of an existing persistent instance of the class </param>
		/// <returns> The persistent instance or proxy </returns>
		/// <remarks>
		/// You should not use this method to determine if an instance exists (use <see cref = "Get(string, object)"/>
		/// instead). Use this only to retrieve an instance that you assume exists, where non-existence
		/// would be an actual error.
		/// </remarks>
		Task<object> LoadAsync(string entityName, object id);
		/// <summary>
		/// Read the persistent state associated with the given identifier into the given transient
		/// instance.
		/// </summary>
		/// <param name = "obj">An "empty" instance of the persistent class</param>
		/// <param name = "id">A valid identifier of an existing persistent instance of the class</param>
		Task LoadAsync(object obj, object id);
		/// <summary>
		/// Persist all reachable transient objects, reusing the current identifier
		/// values. Note that this will not trigger the Interceptor of the Session.
		/// </summary>
		/// <param name = "obj">a detached instance of a persistent class</param>
		/// <param name = "replicationMode"></param>
		Task ReplicateAsync(object obj, ReplicationMode replicationMode);
		/// <summary>
		/// Persist the state of the given detached instance, reusing the current
		/// identifier value.  This operation cascades to associated instances if
		/// the association is mapped with <tt>cascade="replicate"</tt>.
		/// </summary>
		/// <param name = "entityName"></param>
		/// <param name = "obj">a detached instance of a persistent class </param>
		/// <param name = "replicationMode"></param>
		Task ReplicateAsync(string entityName, object obj, ReplicationMode replicationMode);
		/// <summary>
		/// Persist the given transient instance, first assigning a generated identifier.
		/// </summary>
		/// <remarks>
		/// Save will use the current value of the identifier property if the <c>Assigned</c>
		/// generator is used.
		/// </remarks>
		/// <param name = "obj">A transient instance of a persistent class</param>
		/// <returns>The generated identifier</returns>
		Task<object> SaveAsync(object obj);
		/// <summary>
		/// Persist the given transient instance, using the given identifier.
		/// </summary>
		/// <param name = "obj">A transient instance of a persistent class</param>
		/// <param name = "id">An unused valid identifier</param>
		Task SaveAsync(object obj, object id);
		/// <summary>
		/// Persist the given transient instance, first assigning a generated identifier. (Or
		/// using the current value of the identifier property if the <tt>assigned</tt>
		/// generator is used.)
		/// </summary>
		/// <param name = "entityName">The Entity name.</param>
		/// <param name = "obj">a transient instance of a persistent class </param>
		/// <returns> the generated identifier </returns>
		/// <remarks>
		/// This operation cascades to associated instances if the
		/// association is mapped with <tt>cascade="save-update"</tt>.
		/// </remarks>
		Task<object> SaveAsync(string entityName, object obj);
		/// <summary>
		/// Persist the given transient instance, using the given identifier.
		/// </summary>
		/// <param name = "entityName">The Entity name.</param>
		/// <param name = "obj">a transient instance of a persistent class </param>
		/// <param name = "id">An unused valid identifier</param>
		/// <remarks>
		/// This operation cascades to associated instances if the
		/// association is mapped with <tt>cascade="save-update"</tt>.
		/// </remarks>
		Task SaveAsync(string entityName, object obj, object id);
		/// <summary>
		/// Either <c>Save()</c> or <c>Update()</c> the given instance, depending upon the value of
		/// its identifier property.
		/// </summary>
		/// <remarks>
		/// By default the instance is always saved. This behaviour may be adjusted by specifying
		/// an <c>unsaved-value</c> attribute of the identifier property mapping
		/// </remarks>
		/// <param name = "obj">A transient instance containing new or updated state</param>
		Task SaveOrUpdateAsync(object obj);
		/// <summary>
		/// Either <see cref = "Save(String, Object)"/> or <see cref = "Update(String, Object)"/>
		/// the given instance, depending upon resolution of the unsaved-value checks
		/// (see the manual for discussion of unsaved-value checking).
		/// </summary>
		/// <param name = "entityName">The name of the entity </param>
		/// <param name = "obj">a transient or detached instance containing new or updated state </param>
		/// <seealso cref = "ISession.Save(String, Object)"/>
		/// <seealso cref = "ISession.Update(String, Object)"/>
		/// <remarks>
		/// This operation cascades to associated instances if the association is mapped
		/// with <tt>cascade="save-update"</tt>.
		/// </remarks>
		Task SaveOrUpdateAsync(string entityName, object obj);
		/// <summary>
		/// Either <c>Save()</c> or <c>Update()</c> the given instance, depending upon the value of
		/// its identifier property.
		/// </summary>
		/// <remarks>
		/// By default the instance is always saved. This behaviour may be adjusted by specifying
		/// an <c>unsaved-value</c> attribute of the identifier property mapping
		/// </remarks>
		/// <param name = "entityName">The name of the entity</param>      
		/// <param name = "obj">A transient instance containing new or updated state</param>
		/// <param name = "id">Identifier of persistent instance</param>
		Task SaveOrUpdateAsync(string entityName, object obj, object id);
		/// <summary>
		/// Update the persistent instance with the identifier of the given transient instance.
		/// </summary>
		/// <remarks>
		/// If there is a persistent instance with the same identifier, an exception is thrown. If
		/// the given transient instance has a <see langword = "null"/> identifier, an exception will be thrown.
		/// </remarks>
		/// <param name = "obj">A transient instance containing updated state</param>
		Task UpdateAsync(object obj);
		/// <summary>
		/// Update the persistent state associated with the given identifier.
		/// </summary>
		/// <remarks>
		/// An exception is thrown if there is a persistent instance with the same identifier
		/// in the current session.
		/// </remarks>
		/// <param name = "obj">A transient instance containing updated state</param>
		/// <param name = "id">Identifier of persistent instance</param>
		Task UpdateAsync(object obj, object id);
		/// <summary>
		/// Update the persistent instance with the identifier of the given detached
		/// instance.
		/// </summary>
		/// <param name = "entityName">The Entity name.</param>
		/// <param name = "obj">a detached instance containing updated state </param>
		/// <remarks>
		/// If there is a persistent instance with the same identifier,
		/// an exception is thrown. This operation cascades to associated instances
		/// if the association is mapped with <tt>cascade="save-update"</tt>.
		/// </remarks>
		Task UpdateAsync(string entityName, object obj);
		/// <summary>
		/// Update the persistent instance associated with the given identifier.
		/// </summary>
		/// <param name = "entityName">The Entity name.</param>
		/// <param name = "obj">a detached instance containing updated state </param>
		/// <param name = "id">Identifier of persistent instance</param>
		/// <remarks>
		/// If there is a persistent instance with the same identifier,
		/// an exception is thrown. This operation cascades to associated instances
		/// if the association is mapped with <tt>cascade="save-update"</tt>.
		/// </remarks>
		Task UpdateAsync(string entityName, object obj, object id);
		/// <summary>
		/// Copy the state of the given object onto the persistent object with the same
		/// identifier. If there is no persistent instance currently associated with
		/// the session, it will be loaded. Return the persistent instance. If the
		/// given instance is unsaved, save a copy of and return it as a newly persistent
		/// instance. The given instance does not become associated with the session.
		/// This operation cascades to associated instances if the association is mapped
		/// with <tt>cascade="merge"</tt>.<br/>
		/// The semantics of this method are defined by JSR-220.
		/// </summary>
		/// <param name = "obj">a detached instance with state to be copied </param>
		/// <returns> an updated persistent instance </returns>
		Task<object> MergeAsync(object obj);
		/// <summary>
		/// Copy the state of the given object onto the persistent object with the same
		/// identifier. If there is no persistent instance currently associated with
		/// the session, it will be loaded. Return the persistent instance. If the
		/// given instance is unsaved, save a copy of and return it as a newly persistent
		/// instance. The given instance does not become associated with the session.
		/// This operation cascades to associated instances if the association is mapped
		/// with <tt>cascade="merge"</tt>.<br/>
		/// The semantics of this method are defined by JSR-220.
		/// <param name = "entityName">Name of the entity.</param>
		/// <param name = "obj">a detached instance with state to be copied </param>
		/// <returns> an updated persistent instance </returns>
		/// </summary>
		/// <returns></returns>
		Task<object> MergeAsync(string entityName, object obj);
		/// <summary>
		/// Copy the state of the given object onto the persistent object with the same
		/// identifier. If there is no persistent instance currently associated with
		/// the session, it will be loaded. Return the persistent instance. If the
		/// given instance is unsaved, save a copy of and return it as a newly persistent
		/// instance. The given instance does not become associated with the session.
		/// This operation cascades to associated instances if the association is mapped
		/// with <tt>cascade="merge"</tt>.<br/>
		/// The semantics of this method are defined by JSR-220.
		/// </summary>
		/// <param name = "entity">a detached instance with state to be copied </param>
		/// <returns> an updated persistent instance </returns>
		Task<T> MergeAsync<T>(T entity)where T : class;
		/// <summary>
		/// Copy the state of the given object onto the persistent object with the same
		/// identifier. If there is no persistent instance currently associated with
		/// the session, it will be loaded. Return the persistent instance. If the
		/// given instance is unsaved, save a copy of and return it as a newly persistent
		/// instance. The given instance does not become associated with the session.
		/// This operation cascades to associated instances if the association is mapped
		/// with <tt>cascade="merge"</tt>.<br/>
		/// The semantics of this method are defined by JSR-220.
		/// <param name = "entityName">Name of the entity.</param>
		/// <param name = "entity">a detached instance with state to be copied </param>
		/// <returns> an updated persistent instance </returns>
		/// </summary>
		/// <returns></returns>
		Task<T> MergeAsync<T>(string entityName, T entity)where T : class;
		/// <summary>
		/// Make a transient instance persistent. This operation cascades to associated
		/// instances if the association is mapped with <tt>cascade="persist"</tt>.<br/>
		/// The semantics of this method are defined by JSR-220.
		/// </summary>
		/// <param name = "obj">a transient instance to be made persistent </param>
		Task PersistAsync(object obj);
		/// <summary>
		/// Make a transient instance persistent. This operation cascades to associated
		/// instances if the association is mapped with <tt>cascade="persist"</tt>.<br/>
		/// The semantics of this method are defined by JSR-220.
		/// </summary>
		/// <param name = "entityName">Name of the entity.</param>
		/// <param name = "obj">a transient instance to be made persistent</param>
		Task PersistAsync(string entityName, object obj);
		/// <summary>
		/// Remove a persistent instance from the datastore.
		/// </summary>
		/// <remarks>
		/// The argument may be an instance associated with the receiving <c>ISession</c> or a
		/// transient instance with an identifier associated with existing persistent state.
		/// </remarks>
		/// <param name = "obj">The instance to be removed</param>
		Task DeleteAsync(object obj);
		/// <summary>
		/// Remove a persistent instance from the datastore. The <b>object</b> argument may be
		/// an instance associated with the receiving <see cref = "ISession"/> or a transient
		/// instance with an identifier associated with existing persistent state.
		/// This operation cascades to associated instances if the association is mapped
		/// with <tt>cascade="delete"</tt>.
		/// </summary>
		/// <param name = "entityName">The entity name for the instance to be removed. </param>
		/// <param name = "obj">the instance to be removed </param>
		Task DeleteAsync(string entityName, object obj);
		/// <summary>
		/// Delete all objects returned by the query.
		/// </summary>
		/// <param name = "query">The query string</param>
		/// <returns>Returns the number of objects deleted.</returns>
		Task<int> DeleteAsync(string query);
		/// <summary>
		/// Delete all objects returned by the query.
		/// </summary>
		/// <param name = "query">The query string</param>
		/// <param name = "value">A value to be written to a "?" placeholer in the query</param>
		/// <param name = "type">The hibernate type of value.</param>
		/// <returns>The number of instances deleted</returns>
		Task<int> DeleteAsync(string query, object value, IType type);
		/// <summary>
		/// Delete all objects returned by the query.
		/// </summary>
		/// <param name = "query">The query string</param>
		/// <param name = "values">A list of values to be written to "?" placeholders in the query</param>
		/// <param name = "types">A list of Hibernate types of the values</param>
		/// <returns>The number of instances deleted</returns>
		Task<int> DeleteAsync(string query, object[] values, IType[] types);
		/// <summary>
		/// Obtain the specified lock level upon the given object.
		/// </summary>
		/// <param name = "obj">A persistent instance</param>
		/// <param name = "lockMode">The lock level</param>
		Task LockAsync(object obj, LockMode lockMode);
		/// <summary>
		/// Obtain the specified lock level upon the given object.
		/// </summary>
		/// <param name = "entityName">The Entity name.</param>
		/// <param name = "obj">a persistent or transient instance </param>
		/// <param name = "lockMode">the lock level </param>
		/// <remarks>
		/// This may be used to perform a version check (<see cref = "LockMode.Read"/>), to upgrade to a pessimistic
		/// lock (<see cref = "LockMode.Upgrade"/>), or to simply reassociate a transient instance
		/// with a session (<see cref = "LockMode.None"/>). This operation cascades to associated
		/// instances if the association is mapped with <tt>cascade="lock"</tt>.
		/// </remarks>
		Task LockAsync(string entityName, object obj, LockMode lockMode);
		/// <summary>
		/// Re-read the state of the given instance from the underlying database.
		/// </summary>
		/// <remarks>
		/// <para>
		/// It is inadvisable to use this to implement long-running sessions that span many
		/// business tasks. This method is, however, useful in certain special circumstances.
		/// </para>
		/// <para>
		/// For example,
		/// <list>
		///		<item>Where a database trigger alters the object state upon insert or update</item>
		///		<item>After executing direct SQL (eg. a mass update) in the same session</item>
		///		<item>After inserting a <c>Blob</c> or <c>Clob</c></item>
		/// </list>
		/// </para>
		/// </remarks>
		/// <param name = "obj">A persistent instance</param>
		Task RefreshAsync(object obj);
		/// <summary>
		/// Re-read the state of the given instance from the underlying database, with
		/// the given <c>LockMode</c>.
		/// </summary>
		/// <remarks>
		/// It is inadvisable to use this to implement long-running sessions that span many
		/// business tasks. This method is, however, useful in certain special circumstances.
		/// </remarks>
		/// <param name = "obj">a persistent or transient instance</param>
		/// <param name = "lockMode">the lock mode to use</param>
		Task RefreshAsync(object obj, LockMode lockMode);
		/// <summary>
		/// Determine the current lock mode of the given object
		/// </summary>
		/// <param name = "obj">A persistent instance</param>
		/// <returns>The current lock mode</returns>
		Task<LockMode> GetCurrentLockModeAsync(object obj);
		/// <summary>
		/// Create a new instance of <c>Query</c> for the given collection and filter string
		/// </summary>
		/// <param name = "collection">A persistent collection</param>
		/// <param name = "queryString">A hibernate query</param>
		/// <returns>A query</returns>
		Task<IQuery> CreateFilterAsync(object collection, string queryString);
		/// <summary>
		/// Return the persistent instance of the given entity class with the given identifier, or null
		/// if there is no such persistent instance. (If the instance, or a proxy for the instance, is
		/// already associated with the session, return that instance or proxy.)
		/// </summary>
		/// <param name = "clazz">a persistent class</param>
		/// <param name = "id">an identifier</param>
		/// <returns>a persistent instance or null</returns>
		Task<object> GetAsync(System.Type clazz, object id);
		/// <summary>
		/// Return the persistent instance of the given entity class with the given identifier, or null
		/// if there is no such persistent instance. Obtain the specified lock mode if the instance
		/// exists.
		/// </summary>
		/// <param name = "clazz">a persistent class</param>
		/// <param name = "id">an identifier</param>
		/// <param name = "lockMode">the lock mode</param>
		/// <returns>a persistent instance or null</returns>
		Task<object> GetAsync(System.Type clazz, object id, LockMode lockMode);
		/// <summary>
		/// Return the persistent instance of the given named entity with the given identifier,
		/// or null if there is no such persistent instance. (If the instance, or a proxy for the
		/// instance, is already associated with the session, return that instance or proxy.)
		/// </summary>
		/// <param name = "entityName">the entity name </param>
		/// <param name = "id">an identifier </param>
		/// <returns> a persistent instance or null </returns>
		Task<object> GetAsync(string entityName, object id);
		/// <summary>
		/// Strongly-typed version of <see cref = "Get(System.Type, object)"/>
		/// </summary>
		Task<T> GetAsync<T>(object id);
		/// <summary>
		/// Strongly-typed version of <see cref = "Get(System.Type, object, LockMode)"/>
		/// </summary>
		Task<T> GetAsync<T>(object id, LockMode lockMode);
		/// <summary>
		/// Return the entity name for a persistent entity
		/// </summary>
		/// <param name = "obj">a persistent entity</param>
		/// <returns> the entity name </returns>
		Task<string> GetEntityNameAsync(object obj);
	}
}
#endif
