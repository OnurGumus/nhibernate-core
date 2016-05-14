#if NET_4_5
using System;
using System.Data;
using System.Linq.Expressions;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// A command-oriented API for performing bulk operations against a database.
	/// </summary>
	/// <remarks>
	/// A stateless session does not implement a first-level cache nor
	/// interact with any second-level cache, nor does it implement
	/// transactional write-behind or automatic dirty checking, nor do
	/// operations cascade to associated instances. Collections are
	/// ignored by a stateless session. Operations performed via a
	/// stateless session bypass NHibernate's event model and
	/// interceptors. Stateless sessions are vulnerable to data
	/// aliasing effects, due to the lack of a first-level cache.
	/// <para/>
	/// For certain kinds of transactions, a stateless session may
	/// perform slightly faster than a stateful session.
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IStatelessSession : IDisposable
	{
		/// <summary>Insert an entity.</summary>
		/// <param name = "entity">A new transient instance</param>
		/// <returns>The identifier of the instance</returns>
		Task<object> InsertAsync(object entity);
		/// <summary>Insert a row.</summary>
		/// <param name = "entityName">The name of the entity to be inserted</param>
		/// <param name = "entity">A new transient instance</param>
		/// <returns>The identifier of the instance</returns>
		Task<object> InsertAsync(string entityName, object entity);
		/// <summary>Update an entity.</summary>
		/// <param name = "entity">A detached entity instance</param>
		Task UpdateAsync(object entity);
		/// <summary>Update an entity.</summary>
		/// <param name = "entityName">The name of the entity to be updated</param>
		/// <param name = "entity">A detached entity instance</param>
		Task UpdateAsync(string entityName, object entity);
		/// <summary>Delete an entity.</summary>
		/// <param name = "entity">A detached entity instance</param>
		Task DeleteAsync(object entity);
		/// <summary>Delete an entity.</summary>
		/// <param name = "entityName">The name of the entity to be deleted</param>
		/// <param name = "entity">A detached entity instance</param>
		Task DeleteAsync(string entityName, object entity);
		/// <summary>Retrieve a entity.</summary>
		/// <returns>A detached entity instance</returns>
		Task<object> GetAsync(string entityName, object id);
		/// <summary>
		/// Retrieve an entity.
		/// </summary>
		/// <returns>A detached entity instance</returns>
		Task<T> GetAsync<T>(object id);
		/// <summary>
		/// Retrieve an entity, obtaining the specified lock mode.
		/// </summary>
		/// <returns>A detached entity instance</returns>
		Task<object> GetAsync(string entityName, object id, LockMode lockMode);
		/// <summary>
		/// Retrieve an entity, obtaining the specified lock mode.
		/// </summary>
		/// <returns>A detached entity instance</returns>
		Task<T> GetAsync<T>(object id, LockMode lockMode);
		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entity">The entity to be refreshed.</param>
		Task RefreshAsync(object entity);
		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entityName">The name of the entity to be refreshed.</param>
		/// <param name = "entity">The entity to be refreshed.</param>
		Task RefreshAsync(string entityName, object entity);
		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entity">The entity to be refreshed.</param>
		/// <param name = "lockMode">The LockMode to be applied.</param>
		Task RefreshAsync(object entity, LockMode lockMode);
		/// <summary>
		/// Refresh the entity instance state from the database.
		/// </summary>
		/// <param name = "entityName">The name of the entity to be refreshed.</param>
		/// <param name = "entity">The entity to be refreshed.</param>
		/// <param name = "lockMode">The LockMode to be applied.</param>
		Task RefreshAsync(string entityName, object entity, LockMode lockMode);
	}
}
#endif
