using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Action;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractFlushingEventListener
	{
		protected virtual async Task PerformExecutionsAsync(IEventSource session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("executing flush");
			}

			try
			{
				session.ConnectionManager.FlushBeginning();
				// IMPL NOTE : here we alter the flushing flag of the persistence context to allow
				//		during-flush callbacks more leniency in regards to initializing proxies and
				//		lazy collections during their processing.
				// For more information, see HHH-2763 / NH-1882
				session.PersistenceContext.Flushing = true;
				// we need to lock the collection caches before
				// executing entity inserts/updates in order to
				// account for bidi associations
				session.ActionQueue.PrepareActions();
				await (session.ActionQueue.ExecuteActionsAsync());
			}
			catch (HibernateException he)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("Could not synchronize database state with session", he);
				}

				throw;
			}
			finally
			{
				session.PersistenceContext.Flushing = false;
				session.ConnectionManager.FlushEnding();
			}
		}

		protected virtual async Task FlushEntitiesAsync(FlushEvent @event)
		{
			log.Debug("Flushing entities and processing referenced collections");
			// Among other things, updateReachables() will recursively load all
			// collections that are moving roles. This might cause entities to
			// be loaded.
			// So this needs to be safe from concurrent modification problems.
			// It is safe because of how IdentityMap implements entrySet()
			IEventSource source = @event.Session;
			ICollection list = IdentityMap.ConcurrentEntries(source.PersistenceContext.EntityEntries);
			foreach (DictionaryEntry me in list)
			{
				// Update the status of the object and if necessary, schedule an update
				EntityEntry entry = (EntityEntry)me.Value;
				Status status = entry.Status;
				if (status != Status.Loading && status != Status.Gone)
				{
					FlushEntityEvent entityEvent = new FlushEntityEvent(source, me.Key, entry);
					IFlushEntityEventListener[] listeners = source.Listeners.FlushEntityEventListeners;
					foreach (IFlushEntityEventListener listener in listeners)
					{
						await (listener.OnFlushEntityAsync(entityEvent));
					}
				}
			}

			source.ActionQueue.SortActions();
		}

		protected virtual async Task FlushEverythingToExecutionsAsync(FlushEvent @event)
		{
			log.Debug("flushing session");
			IEventSource session = @event.Session;
			IPersistenceContext persistenceContext = session.PersistenceContext;
			session.Interceptor.PreFlush((ICollection)persistenceContext.EntitiesByKey.Values);
			await (PrepareEntityFlushesAsync(session));
			await (PrepareCollectionFlushesAsync(session));
			// now, any collections that are initialized
			// inside this block do not get updated - they
			// are ignored until the next flush
			persistenceContext.Flushing = true;
			try
			{
				await (FlushEntitiesAsync(@event));
				await (FlushCollectionsAsync(session));
			}
			finally
			{
				persistenceContext.Flushing = false;
			}

			//some statistics
			if (log.IsDebugEnabled)
			{
				StringBuilder sb = new StringBuilder(100);
				sb.Append("Flushed: ").Append(session.ActionQueue.InsertionsCount).Append(" insertions, ").Append(session.ActionQueue.UpdatesCount).Append(" updates, ").Append(session.ActionQueue.DeletionsCount).Append(" deletions to ").Append(persistenceContext.EntityEntries.Count).Append(" objects");
				log.Debug(sb.ToString());
				sb = new StringBuilder(100);
				sb.Append("Flushed: ").Append(session.ActionQueue.CollectionCreationsCount).Append(" (re)creations, ").Append(session.ActionQueue.CollectionUpdatesCount).Append(" updates, ").Append(session.ActionQueue.CollectionRemovalsCount).Append(" removals to ").Append(persistenceContext.CollectionEntries.Count).Append(" collections");
				log.Debug(sb.ToString());
				await (new Printer(session.Factory).ToStringAsync(persistenceContext.EntitiesByKey.Values.ToArray().GetEnumerator(), session.EntityMode));
			}
		}

		protected virtual async Task PrepareCollectionFlushesAsync(ISessionImplementor session)
		{
			// Initialize dirty flags for arrays + collections with composite elements
			// and reset reached, doupdate, etc.
			log.Debug("dirty checking collections");
			ICollection list = IdentityMap.Entries(session.PersistenceContext.CollectionEntries);
			foreach (DictionaryEntry entry in list)
			{
				await (((CollectionEntry)entry.Value).PreFlushAsync((IPersistentCollection)entry.Key));
			}
		}

		protected virtual async Task FlushCollectionsAsync(IEventSource session)
		{
			log.Debug("Processing unreferenced collections");
			ICollection list = IdentityMap.Entries(session.PersistenceContext.CollectionEntries);
			foreach (DictionaryEntry me in list)
			{
				CollectionEntry ce = (CollectionEntry)me.Value;
				if (!ce.IsReached && !ce.IsIgnore)
				{
					await (Collections.ProcessUnreachableCollectionAsync((IPersistentCollection)me.Key, session));
				}
			}

			// Schedule updates to collections:
			log.Debug("Scheduling collection removes/(re)creates/updates");
			list = IdentityMap.Entries(session.PersistenceContext.CollectionEntries);
			ActionQueue actionQueue = session.ActionQueue;
			foreach (DictionaryEntry me in list)
			{
				IPersistentCollection coll = (IPersistentCollection)me.Key;
				CollectionEntry ce = (CollectionEntry)me.Value;
				if (ce.IsDorecreate)
				{
					session.Interceptor.OnCollectionRecreate(coll, ce.CurrentKey);
					actionQueue.AddAction(new CollectionRecreateAction(coll, ce.CurrentPersister, ce.CurrentKey, session));
				}

				if (ce.IsDoremove)
				{
					session.Interceptor.OnCollectionRemove(coll, ce.LoadedKey);
					actionQueue.AddAction(new CollectionRemoveAction(coll, ce.LoadedPersister, ce.LoadedKey, ce.IsSnapshotEmpty(coll), session));
				}

				if (ce.IsDoupdate)
				{
					session.Interceptor.OnCollectionUpdate(coll, ce.LoadedKey);
					actionQueue.AddAction(new CollectionUpdateAction(coll, ce.LoadedPersister, ce.LoadedKey, ce.IsSnapshotEmpty(coll), session));
				}
			}

			actionQueue.SortCollectionActions();
		}

		protected virtual async Task CascadeOnFlushAsync(IEventSource session, IEntityPersister persister, object key, object anything)
		{
			session.PersistenceContext.IncrementCascadeLevel();
			try
			{
				await (new Cascade(CascadingAction, CascadePoint.BeforeFlush, session).CascadeOnAsync(persister, key, anything));
			}
			finally
			{
				session.PersistenceContext.DecrementCascadeLevel();
			}
		}

		protected virtual async Task PrepareEntityFlushesAsync(IEventSource session)
		{
			log.Debug("processing flush-time cascades");
			ICollection list = IdentityMap.ConcurrentEntries(session.PersistenceContext.EntityEntries);
			//safe from concurrent modification because of how entryList() is implemented on IdentityMap
			foreach (DictionaryEntry me in list)
			{
				EntityEntry entry = (EntityEntry)me.Value;
				Status status = entry.Status;
				if (status == Status.Loaded || status == Status.Saving || status == Status.ReadOnly)
				{
					await (CascadeOnFlushAsync(session, entry.Persister, me.Key, Anything));
				}
			}
		}
	}
}