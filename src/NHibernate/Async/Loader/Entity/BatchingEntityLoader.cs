#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Loader.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BatchingEntityLoader : IUniqueEntityLoader
	{
		public async Task<object> LoadAsync(object id, object optionalObject, ISessionImplementor session)
		{
			object[] batch = session.PersistenceContext.BatchFetchQueue.GetEntityBatch(persister, id, batchSizes[0]);
			for (int i = 0; i < batchSizes.Length - 1; i++)
			{
				int smallBatchSize = batchSizes[i];
				if (batch[smallBatchSize - 1] != null)
				{
					object[] smallBatch = new object[smallBatchSize];
					Array.Copy(batch, 0, smallBatch, 0, smallBatchSize);
					IList results = await (loaders[i].LoadEntityBatchAsync(session, smallBatch, idType, optionalObject, persister.EntityName, id, persister));
					return GetObjectFromList(results, id, session); //EARLY EXIT
				}
			}

			return await (((IUniqueEntityLoader)loaders[batchSizes.Length - 1]).LoadAsync(id, optionalObject, session));
		}
	}
}
#endif
