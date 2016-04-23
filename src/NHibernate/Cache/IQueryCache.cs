using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Type;

namespace NHibernate.Cache
{
	/// <summary>
	/// Defines the contract for caches capable of storing query results.  These
	/// caches should only concern themselves with storing the matching result ids.
	/// The transactional semantics are necessarily less strict than the semantics
	/// of an item cache.
	/// </summary>
	public interface IQueryCache
	{
		ICache Cache { get; }
		string RegionName { get; }

		void Clear();
		Task<bool> Put(QueryKey key, ICacheAssembler[] returnTypes, IList result, bool isNaturalKeyLookup, ISessionImplementor session);
		Task<IList> Get(QueryKey key, ICacheAssembler[] returnTypes, bool isNaturalKeyLookup, ISet<string> spaces, ISessionImplementor session);
		void Destroy();
	}
}
