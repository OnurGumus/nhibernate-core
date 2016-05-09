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

			PreUpdate();
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
					throw new HibernateException("cannot recreate collection while filter is enabled: " + await (MessageHelper.CollectionInfoStringAsync(persister, collection, id, session)));
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

			await (Session.PersistenceContext.GetCollectionEntry(collection).AfterActionAsync(collection));
			Evict();
			PostUpdate();
			if (statsEnabled)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.UpdateCollection(Persister.Role, stopwatch.Elapsed);
			}
		}
	}
}