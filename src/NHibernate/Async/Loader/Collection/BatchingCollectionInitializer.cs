#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Loader.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BatchingCollectionInitializer : ICollectionInitializer
	{
		public async Task InitializeAsync(object id, ISessionImplementor session)
		{
			object[] batch = session.PersistenceContext.BatchFetchQueue.GetCollectionBatch(collectionPersister, id, batchSizes[0]);
			for (int i = 0; i < batchSizes.Length; i++)
			{
				int smallBatchSize = batchSizes[i];
				if (batch[smallBatchSize - 1] != null)
				{
					object[] smallBatch = new object[smallBatchSize];
					Array.Copy(batch, 0, smallBatch, 0, smallBatchSize);
					await (loaders[i].LoadCollectionBatchAsync(session, smallBatch, collectionPersister.KeyType));
					return; //EARLY EXIT!
				}
			}

			await (loaders[batchSizes.Length - 1].LoadCollectionAsync(session, id, collectionPersister.KeyType));
		}
	}
}
#endif
