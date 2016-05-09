using System;
using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Cache.Entry
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionCacheEntry
	{
		public virtual async Task AssembleAsync(IPersistentCollection collection, ICollectionPersister persister, object owner)
		{
			await (collection.InitializeFromCacheAsync(persister, state, owner));
			collection.AfterInitialize(persister);
		}
	}
}