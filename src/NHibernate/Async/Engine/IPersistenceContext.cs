using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Engine.Loading;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	/// <summary>
	/// Holds the state of the persistence context, including the
	/// first-level cache, entries, snapshots, proxies, etc.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPersistenceContext
	{
		Task<
		/// <summary>
		/// Get the current state of the entity as known to the underlying
		/// database, or null if there is no corresponding row
		/// </summary>
		object[]> GetDatabaseSnapshotAsync(object id, IEntityPersister persister);
		Task<
		/// <summary>
		/// Get the values of the natural id fields as known to the underlying
		/// database, or null if the entity has no natural id or there is no
		/// corresponding row.
		/// </summary>
		object[]> GetNaturalIdSnapshotAsync(object id, IEntityPersister persister);
	}
}