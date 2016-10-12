#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class CollectionUpdateAction : CollectionAction
	{
		public override async Task ExecuteAsync()
		{
			object id = Key;
			ISessionImplementor session = Session;
			ICollectionPersister persister = Persister;
			IPersistentCollection collection = Collection;
			bool affectedByFilters = persister.IsAffectedByEnabledFilters(session);
			bool statsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			await (PreUpdateAsync());
			if (!collection.WasInitialized)
			{
				if (!collection.HasQueuedOperations)
				{
					throw new AssertionFailure("no queued adds");
				}
			//do nothing - we only need to notify the cache...
			}
			else if (!affectedByFilters && collection.Empty)
			{
				if (!emptySnapshot)
				{
					await (persister.RemoveAsync(id, session));
				}
			}
			else if (collection.NeedsRecreate(persister))
			{
				if (affectedByFilters)
				{
					throw new HibernateException("cannot recreate collection while filter is enabled: " + MessageHelper.CollectionInfoString(persister, collection, id, session));
				}

				if (!emptySnapshot)
				{
					await (persister.RemoveAsync(id, session));
				}

				await (persister.RecreateAsync(collection, id, session));
			}
			else
			{
				await (persister.DeleteRowsAsync(collection, id, session));
				await (persister.UpdateRowsAsync(collection, id, session));
				await (persister.InsertRowsAsync(collection, id, session));
			}

			Session.PersistenceContext.GetCollectionEntry(collection).AfterAction(collection);
			Evict();
			await (PostUpdateAsync());
			if (statsEnabled)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.UpdateCollection(Persister.Role, stopwatch.Elapsed);
			}
		}

		private async Task PreUpdateAsync()
		{
			IPreCollectionUpdateEventListener[] preListeners = Session.Listeners.PreCollectionUpdateEventListeners;
			if (preListeners.Length > 0)
			{
				PreCollectionUpdateEvent preEvent = new PreCollectionUpdateEvent(Persister, Collection, (IEventSource)Session);
				for (int i = 0; i < preListeners.Length; i++)
				{
					await (preListeners[i].OnPreUpdateCollectionAsync(preEvent));
				}
			}
		}

		private async Task PostUpdateAsync()
		{
			IPostCollectionUpdateEventListener[] postListeners = Session.Listeners.PostCollectionUpdateEventListeners;
			if (postListeners.Length > 0)
			{
				PostCollectionUpdateEvent postEvent = new PostCollectionUpdateEvent(Persister, Collection, (IEventSource)Session);
				for (int i = 0; i < postListeners.Length; i++)
				{
					await (postListeners[i].OnPostUpdateCollectionAsync(postEvent));
				}
			}
		}
	}
}
#endif
