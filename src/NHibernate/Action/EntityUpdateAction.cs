using System;
using System.Diagnostics;
using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Action
{
	[Serializable]
	public sealed class EntityUpdateAction : EntityAction
	{
		private readonly object[] state;
		private readonly object[] previousState;
		private object previousVersion;
		private object nextVersion;
		private readonly int[] dirtyFields;
		private readonly bool hasDirtyCollection;
		private object cacheEntry;
		private ISoftLock slock;

		public EntityUpdateAction(object id, object[] state, 
			int[] dirtyProperties, bool hasDirtyCollection, 
			object[] previousState, object previousVersion, object nextVersion, object instance, 
			IEntityPersister persister, ISessionImplementor session)
			: base(session, id, instance, persister)
		{
			this.state = state;
			this.previousState = previousState;
			this.previousVersion = previousVersion;
			this.nextVersion = nextVersion;
			dirtyFields = dirtyProperties;
			this.hasDirtyCollection = hasDirtyCollection;
		}

		protected internal override bool HasPostCommitEventListeners
		{
			get { return Session.Listeners.PostCommitUpdateEventListeners.Length > 0; }
		}

		public override async Task Execute()
		{
			ISessionImplementor session = Session;
			object id = Id;
			IEntityPersister persister = Persister;
			object instance = Instance;

			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			bool veto = await PreUpdate().ConfigureAwait(false);

			ISessionFactoryImplementor factory = Session.Factory;

			if (persister.IsVersionPropertyGenerated)
			{
				// we need to grab the version value from the entity, otherwise
				// we have issues with generated-version entities that may have
				// multiple actions queued during the same flush
				previousVersion = persister.GetVersion(instance, session.EntityMode);
			}

			CacheKey ck = null;
			if (persister.HasCache)
			{
				ck = session.GenerateCacheKey(id, persister.IdentifierType, persister.RootEntityName);
				slock = persister.Cache.Lock(ck, previousVersion);
			}

			if (!veto)
			{
				await persister.Update(id, state, dirtyFields, hasDirtyCollection, previousState, previousVersion, instance, null, session).ConfigureAwait(false);
			}

			EntityEntry entry = Session.PersistenceContext.GetEntry(instance);
			if (entry == null)
			{
				throw new AssertionFailure("Possible nonthreadsafe access to session");
			}

			if (entry.Status == Status.Loaded || persister.IsVersionPropertyGenerated)
			{
				// get the updated snapshot of the entity state by cloning current state;
				// it is safe to copy in place, since by this time no-one else (should have)
				// has a reference  to the array
				TypeHelper.DeepCopy(state, persister.PropertyTypes, persister.PropertyCheckability, state, Session);
				if (persister.HasUpdateGeneratedProperties)
				{
					// this entity defines property generation, so process those generated
					// values...
					await persister.ProcessUpdateGeneratedProperties(id, instance, state, Session).ConfigureAwait(false);
					if (persister.IsVersionPropertyGenerated)
					{
						nextVersion = Versioning.GetVersion(state, persister);
					}
				}
				// have the entity entry perform post-update processing, passing it the
				// update state and the new version (if one).
				entry.PostUpdate(instance, state, nextVersion);
			}

			if (persister.HasCache)
			{
				if (persister.IsCacheInvalidationRequired || entry.Status != Status.Loaded)
				{
					persister.Cache.Evict(ck);
				}
				else
				{
					CacheEntry ce = new CacheEntry(persister, persister.HasUninitializedLazyProperties(instance, session.EntityMode), nextVersion,
						await TypeHelper.Disassemble(state, persister.PropertyTypes, null, session, instance).ConfigureAwait(false));
					cacheEntry = persister.CacheEntryStructure.Structure(ce);

					bool put = persister.Cache.Update(ck, cacheEntry, nextVersion, previousVersion);

					if (put && factory.Statistics.IsStatisticsEnabled)
					{
						factory.StatisticsImplementor.SecondLevelCachePut(Persister.Cache.RegionName);
					}
				}
			}

			await PostUpdate().ConfigureAwait(false);

			if (statsEnabled && !veto)
			{
				stopwatch.Stop();
				factory.StatisticsImplementor.UpdateEntity(Persister.EntityName, stopwatch.Elapsed);
			}
		}

		protected override Task AfterTransactionCompletionProcessImpl(bool success)
		{
			IEntityPersister persister = Persister;
			if (persister.HasCache)
			{
				CacheKey ck = Session.GenerateCacheKey(Id, persister.IdentifierType, persister.RootEntityName);

				if (success && cacheEntry != null)
				{
					bool put = persister.Cache.AfterUpdate(ck, cacheEntry, nextVersion, slock);

					if (put && Session.Factory.Statistics.IsStatisticsEnabled)
					{
						Session.Factory.StatisticsImplementor.SecondLevelCachePut(Persister.Cache.RegionName);
					}
				}
				else
				{
					persister.Cache.Release(ck, slock);
				}
			}
			if (success)
			{
				return PostCommitUpdate();
			}
			return TaskHelper.CompletedTask;
		}
		
		private async Task PostUpdate()
		{
			IPostUpdateEventListener[] postListeners = Session.Listeners.PostUpdateEventListeners;
			if (postListeners.Length > 0)
			{
				PostUpdateEvent postEvent = new PostUpdateEvent(Instance, Id, state, previousState, Persister, (IEventSource)Session);
				foreach (IPostUpdateEventListener listener in postListeners)
				{
					await listener.OnPostUpdate(postEvent).ConfigureAwait(false);
				}
			}
		}

		private async Task PostCommitUpdate()
		{
			IPostUpdateEventListener[] postListeners = Session.Listeners.PostCommitUpdateEventListeners;
			if (postListeners.Length > 0)
			{
				PostUpdateEvent postEvent = new PostUpdateEvent(Instance, Id, state, previousState, Persister, (IEventSource)Session);
				foreach (IPostUpdateEventListener listener in postListeners)
				{
					await listener.OnPostUpdate(postEvent).ConfigureAwait(false);
				}
			}
		}

		private async Task<bool> PreUpdate()
		{
			IPreUpdateEventListener[] preListeners = Session.Listeners.PreUpdateEventListeners;
			bool veto = false;
			if (preListeners.Length > 0)
			{
				var preEvent = new PreUpdateEvent(Instance, Id, state, previousState, Persister, (IEventSource) Session);
				foreach (IPreUpdateEventListener listener in preListeners)
				{
					veto |= await listener.OnPreUpdate(preEvent).ConfigureAwait(false);
				}
			}
			return veto;
		}
	}
}
