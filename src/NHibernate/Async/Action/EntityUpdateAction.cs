#if NET_4_5
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

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class EntityUpdateAction : EntityAction
	{
		public override async Task ExecuteAsync()
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

			bool veto = PreUpdate();
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
				await (persister.UpdateAsync(id, state, dirtyFields, hasDirtyCollection, previousState, previousVersion, instance, null, session));
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
					await (persister.ProcessUpdateGeneratedPropertiesAsync(id, instance, state, Session));
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
					CacheEntry ce = new CacheEntry(state, persister, persister.HasUninitializedLazyProperties(instance, session.EntityMode), nextVersion, Session, instance);
					cacheEntry = persister.CacheEntryStructure.Structure(ce);
					bool put = persister.Cache.Update(ck, cacheEntry, nextVersion, previousVersion);
					if (put && factory.Statistics.IsStatisticsEnabled)
					{
						factory.StatisticsImplementor.SecondLevelCachePut(Persister.Cache.RegionName);
					}
				}
			}

			PostUpdate();
			if (statsEnabled && !veto)
			{
				stopwatch.Stop();
				factory.StatisticsImplementor.UpdateEntity(Persister.EntityName, stopwatch.Elapsed);
			}
		}
	}
}
#endif
