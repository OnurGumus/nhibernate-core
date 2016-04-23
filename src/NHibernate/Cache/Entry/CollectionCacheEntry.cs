using System;
using System.Threading.Tasks;
using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Util;

namespace NHibernate.Cache.Entry
{
	[Serializable]
	public class CollectionCacheEntry
	{
		private readonly object state;

		internal CollectionCacheEntry(object state)
		{
			this.state = state;
		}

		public virtual object[] State
		{
			get { return (object[])state; }//TODO: assumes all collections disassemble to an array!
		}

		public virtual async Task Assemble(IPersistentCollection collection, ICollectionPersister persister, object owner)
		{
			await collection.InitializeFromCache(persister, state, owner).ConfigureAwait(false);
			collection.AfterInitialize(persister);
		}

		public override string ToString()
		{
			return "CollectionCacheEntry" + ArrayHelper.ToString(State);
		}
	}
}
