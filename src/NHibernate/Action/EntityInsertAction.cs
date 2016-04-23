using System;
using System.Diagnostics;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Action
{
	[Serializable]
	public sealed class EntityInsertAction : EntityAction
	{
		private readonly object[] state;
		private object version;
		private object cacheEntry;

		public EntityInsertAction(object id, object[] state, object instance, object version, IEntityPersister persister, ISessionImplementor session)
			: base(session, id, instance, persister)
		{
			this.state = state;
			this.version = version;
		}

		public object[] State
		{
			get { return state; }
		}

		protected internal override bool HasPostCommitEventListeners
		{
			get
			{
				return Session.Listeners.PostCommitInsertEventListeners.Length > 0;
			}
		}

		public override async Task Execute()
		{
			IEntityPersister persister = Persister;
			ISessionImplementor session = Session;
			object instance = Instance;
			object id = Id;

			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			bool veto = await PreInsert().ConfigureAwait(false);

			// Don't need to lock the cache here, since if someone
			// else inserted the same pk first, the insert would fail
			if (!veto)
			{

				await persister.Insert(id, state, instance, Session).ConfigureAwait(false);

				EntityEntry entry = Session.PersistenceContext.GetEntry(instance);
				if (entry == null)
				{
					throw new AssertionFailure("Possible nonthreadsafe access to session");
				}

				entry.PostInsert();

				if (persister.HasInsertGeneratedProperties)
				{
					await persister.ProcessInsertGeneratedProperties(id, instance, state, Session).ConfigureAwait(false);
					if (persister.IsVersionPropertyGenerated)
					{
						version = Versioning.GetVersion(state, persister);
					}
					entry.PostUpdate(instance, state, version);
				}
			}

			ISessionFactoryImplementor factory = Session.Factory;

			if (IsCachePutEnabled(persister))
			{
				
				CacheEntry ce = new CacheEntry(persister, persister.HasUninitializedLazyProperties(instance, session.EntityMode), version,
					await TypeHelper.Disassemble(state, persister.PropertyTypes, null, session, instance).ConfigureAwait(false));
				cacheEntry = persister.CacheEntryStructure.Structure(ce);

				CacheKey ck = Session.GenerateCacheKey(id, persister.IdentifierType, persister.RootEntityName);
				bool put = persister.Cache.Insert(ck, cacheEntry, version);

				if (put && factory.Statistics.IsStatisticsEnabled)
				{
					factory.StatisticsImplementor.SecondLevelCachePut(Persister.Cache.RegionName);
				}
			}

			await PostInsert().ConfigureAwait(false);

			if (statsEnabled && !veto)
			{
				stopwatch.Stop();
				factory.StatisticsImplementor.InsertEntity(Persister.EntityName, stopwatch.Elapsed);
			}
		}

		protected override Task AfterTransactionCompletionProcessImpl(bool success)
		{
			//Make 100% certain that this is called before any subsequent ScheduledUpdate.afterTransactionCompletion()!!
			IEntityPersister persister = Persister;
			if (success && IsCachePutEnabled(persister))
			{
				CacheKey ck = Session.GenerateCacheKey(Id, persister.IdentifierType, persister.RootEntityName);
				bool put = persister.Cache.AfterInsert(ck, cacheEntry, version);

				if (put && Session.Factory.Statistics.IsStatisticsEnabled)
				{
					Session.Factory.StatisticsImplementor.SecondLevelCachePut(Persister.Cache.RegionName);
				}
			}
			if (success)
			{
				return PostCommitInsert();
			}
			return TaskHelper.CompletedTask;
		}

		private async Task PostInsert()
		{
			IPostInsertEventListener[] postListeners = Session.Listeners.PostInsertEventListeners;
			if (postListeners.Length > 0)
			{
				PostInsertEvent postEvent = new PostInsertEvent(Instance, Id, state, Persister, (IEventSource)Session);
				foreach (IPostInsertEventListener listener in postListeners)
				{
					await listener.OnPostInsert(postEvent).ConfigureAwait(false);
				}
			}
		}

		private async Task PostCommitInsert()
		{
			IPostInsertEventListener[] postListeners = Session.Listeners.PostCommitInsertEventListeners;
			if (postListeners.Length > 0)
			{
				PostInsertEvent postEvent = new PostInsertEvent(Instance, Id, state, Persister, (IEventSource)Session);
				foreach (IPostInsertEventListener listener in postListeners)
				{
					await listener.OnPostInsert(postEvent).ConfigureAwait(false);
				}
			}
		}

		private async Task<bool> PreInsert()
		{
			IPreInsertEventListener[] preListeners = Session.Listeners.PreInsertEventListeners;
			bool veto = false;
			if (preListeners.Length > 0)
			{
				var preEvent = new PreInsertEvent(Instance, Id, state, Persister, (IEventSource) Session);
				foreach (IPreInsertEventListener listener in preListeners)
				{
					veto |= await listener.OnPreInsert(preEvent).ConfigureAwait(false);
				}
			}
			return veto;
		}

		private bool IsCachePutEnabled(IEntityPersister persister)
		{
			return
				persister.HasCache && !persister.IsCacheInvalidationRequired
				&& ((Session.CacheMode & CacheMode.Put) == CacheMode.Put);
		}

		public override int CompareTo(EntityAction other)
		{
			return 0;
		}
	}
}
