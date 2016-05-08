using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractPersistentCollection : IPersistentCollection
	{
		protected virtual async Task<bool> ReadSizeAsync()
		{
			if (!initialized)
			{
				if (cachedSize != -1 && !HasQueuedOperations)
				{
					return true;
				}
				else
				{
					ThrowLazyInitializationExceptionIfNotConnected();
					CollectionEntry entry = session.PersistenceContext.GetCollectionEntry(this);
					ICollectionPersister persister = entry.LoadedPersister;
					if (persister.IsExtraLazy)
					{
						if (HasQueuedOperations)
						{
							session.Flush();
						}

						cachedSize = await (persister.GetSizeAsync(entry.LoadedKey, session));
						return true;
					}
				}
			}

			Read();
			return false;
		}

		/// <summary>
		/// Called before inserting rows, to ensure that any surrogate keys are fully generated
		/// </summary>
		/// <param name = "persister"></param>
		public virtual async Task PreInsertAsync(ICollectionPersister persister)
		{
		}
	/*
		 * These were needed by Hibernate because Java's collections provide methods
		 * to get sublists, modify a collection with an iterator - all things that 
		 * Hibernate needs to be made aware of.  If .net changes their collection interfaces
		 * then we can readd these back in.
		 */
	}
}