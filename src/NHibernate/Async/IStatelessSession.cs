using System;
using System.Data;
using System.Linq.Expressions;
using NHibernate.Engine;
using System.Threading.Tasks;

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
		Task<
		/// <summary>Insert an entity.</summary>
		/// <param name = "entity">A new transient instance</param>
		/// <returns>The identifier of the instance</returns>
		object> InsertAsync(object entity);
		Task<
		/// <summary>Insert a row.</summary>
		/// <param name = "entityName">The name of the entity to be inserted</param>
		/// <param name = "entity">A new transient instance</param>
		/// <returns>The identifier of the instance</returns>
		object> InsertAsync(string entityName, object entity);
	}
}