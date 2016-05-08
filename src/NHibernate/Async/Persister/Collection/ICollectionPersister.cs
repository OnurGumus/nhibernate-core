using System.Collections.Generic;
using System.Data;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Persister.Collection
{
	/// <summary>
	/// A strategy for persisting a collection role.
	/// </summary>
	/// <remarks>
	/// Defines a contract between the persistence strategy and the actual persistent collection framework
	/// and session. Does not define operations that are required for querying collections, or loading by outer join.
	/// <para/>
	/// Implements persistence of a collection instance while the instance is
	/// referenced in a particular role.
	/// <para/>
	/// This class is highly coupled to the <see cref = "IPersistentCollection"/>
	/// hierarchy, since double dispatch is used to load and update collection 
	/// elements.
	/// <para/>
	/// May be considered an immutable view of the mapping object
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ICollectionPersister
	{
		Task<int> GetSizeAsync(object key, ISessionImplementor session);
		Task<
		/// <summary>
		/// Try to find an element by a given index.
		/// </summary>
		/// <param name = "key">The key of the collection (collection-owner identifier)</param>
		/// <param name = "index">The given index.</param>
		/// <param name = "session">The active <see cref = "ISession"/>.</param>
		/// <param name = "owner">The owner of the collection.</param>
		/// <returns>The value of the element where available; otherwise <see cref = "NotFoundObject"/>.</returns>
		object> GetElementByIndexAsync(object key, object index, ISessionImplementor session, object owner);
	}
}